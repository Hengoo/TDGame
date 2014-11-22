using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace TDGame.Kommunikation
{
	/// <summary>Die UDPMulticast-Klasse vereinfacht das automatische finden von Servern im Netzwerk</summary>
	public class UDPMulticast
	{
		/// <summary>Beschreibt den Status der UDPMulticast Klasse</summary>
		private enum status
		{
			/// <summary>Noch nicht gewählt</summary>
			none,
			/// <summary>Wird auf Server festgelegt</summary>
			server,
			/// <summary>Wird auf Client festgelegt</summary>
			client
		}
		/// <summary>Speichert einen gefundenen Server in diesem Konstrukt</summary>
		public struct server
		{
			/// <summary>Ip des Servers</summary>
			public string ip;
			/// <summary>Name des Servers</summary>
			public string name;
			/// <summary>Port des Servers</summary>
			public int port;
			/// <summary>Gibt den Server im Format "name - ip:port" aus</summary>
			public override string ToString()
			{
				return name + " - " + ip + ":" + port;
			}
		}
		/// <summary>Benötigtes Socket für die Datenübertragung</summary>
		private TcpListener tcpserver;
		/// <summary>Konstante Adresse für das Multicast Packet</summary>
		private IPAddress multicastAdr;
		/// <summary>Konstanter EndPunkt für das Multicast Packet</summary>
		private IPEndPoint multicastEnd;
		/// <summary>Port des eigenen Servers (Nur als Server verwendet)</summary>
		public int port;
		/// <summary>Status der Klasse; einmal festgesetzt kann dies nicht mehr geändert werden</summary>
		private status stat;
		/// <summary>Liste aller gefundenen Server (Nur als Client verwendet)</summary>
		private List<server> serverList;
		/// <summary>Socket um Daten zu empfangen</summary>
		private Socket sockRec;
		/// <summary>Name des eigenen Servers (Nur als Server verwendet)</summary>
		private string myName;
		/// <summary>Beschreibt ob der Server gerade auf Anfragen wartet (Nur als Server verwendet)</summary>
		private bool listening;
		/// <summary>Optionnale Referenz zu einer Listbox um die Ergebnisse sofort anzeigen zu lassen</summary>
		private ListBox lbox;
		/// <summary>Beschreibt ob gerade gesucht wird (Nur als Client verwendbar, readonly für andere Klassen)</summary>
		public bool working { get; private set; }

		/// <summary>Erstellt eine neue UDPMulticast-Klasse zum vereinfachten Server aufstellen und finden</summary>
		public UDPMulticast()
		{
			multicastAdr = new IPAddress(new Byte[] { 224, 0, 0, 0 });
			multicastEnd = new IPEndPoint(multicastAdr, 8000);
		}

		/// <summary>Sucht nach einem freien Port auf dem Server geöffnet werden kann + öffnet ihn</summary>
		private bool AnyConnect()
		{
			port = 13337;
			IPEndPoint anyEnd = new IPEndPoint(IPAddress.Any, port);
			while (true)
			{
				try
				{
					tcpserver = new TcpListener(anyEnd);
					tcpserver.Start();
					string tmp = anyEnd.ToString();
					int left = 16 - tmp.Length;
					int right = left / 2;
					left = left - right;
					Console.WriteLine("[-{1} Hosting on: {0} {2}-]", anyEnd.ToString(), new string('-', left), new string('-', right));
					return true;
				}
				catch
				{
					if (port >= 65536)
						return false;
					else
					{
						port++;
						anyEnd = new IPEndPoint(IPAddress.Any, port);
					}
				}
			}
		}

		/// <summary>Eröffnet einen eigenen Server der auf Suchantfragen wartet + antwortet</summary>
		/// <param name="_name">Name des eigenen Server</param>
		public TcpListener ServerView(string _name)
		{
			if (stat != status.none && stat != status.server)
				return null;

			stat = status.server;
			if (!AnyConnect())
			{
				// server aufstellen fehlgeschlagen - sollte nicht vorkommen
				return null;
			}
			myName = _name;
			listen();
			return tcpserver;

			// clientask: client|aksforserver
			// serverresponse: server|<port>|<name>
			// clientconnect client|connect
		}

		/// <summary>Schließt das Empfangssocket und bricht somit das Warten auf Anfragen ab</summary>
		public void Close()
		{
			if (stat == status.none)
				return;

			try { sockRec.Close(); }
			catch { }
		}

		/// <summary>Sucht nach allen verfügbaren Servern und gibt sie in einen 'List' von 'server' wieder aus</summary>
		/// <param name="_serverList">Speichert in einer Liste von 'server' die Ergebnisse</param>
		/// <param name="_lbox">Zeigt die Ergebnisse direkt in einer GUIListbox an</param>
		public void ServerSearch(List<server> _serverList, ListBox _lbox)
		{
			if ((stat != status.none && stat != status.client) || working)
				return;

			working = true;
			lbox = _lbox;
			serverList = _serverList;
			stat = status.client;
			listening = false;
			listen();
			while (!listening)
				System.Threading.Thread.Sleep(1);
			sendMsg("client|aksforserver");
			// starte den timeoutThread, damit die such nach einer bestimmten zeit beendet wird
			System.Threading.Thread timeoutThread = new System.Threading.Thread(timeout);
			timeoutThread.Start();
			return;
		}

		/// <summary>Bricht die Suche nach 5 sekunden ab [VARIEABEL EINSTELLEN]</summary>
		private void timeout()
		{
			DateTime dt = DateTime.Now;
			while ((DateTime.Now.ToUniversalTime() - dt.ToUniversalTime()).TotalMilliseconds < 3000) ; //------------------------------------------------
			sockRec.Close();

			if (lbox != null && serverList.Count == 0)
				lbox.Items.Add("Es wurden keine offenen Server gefunden");

			working = false;
		}

		/// <summary>Wartet auf Fragen/Antworten und beantwortet/verarbeitet sie</summary>
		private void listenThread()
		{
			sockRec = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			sockRec.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
			sockRec.Bind(new IPEndPoint(IPAddress.Any, 8000));
			sockRec.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(multicastAdr, IPAddress.Any));
			EndPoint tempReceivePoint = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
			if (serverList == null)
				serverList = new List<server>();
			serverList.Clear();

			while (true)
			{
				byte[] packet = new byte[1024];
				try
				{
					listening = true;
					sockRec.ReceiveFrom(packet, ref tempReceivePoint);
					listening = false;
				}
				catch
				{
					listening = false;
					return;
				}

				string msg = Encoding.ASCII.GetString(packet);
				if (msg.CompareTo("client|aksforserver") == 0 && stat == status.server)
					sendMsg("server|" + myName + "|" + port);
				else if (msg.StartsWith("server|") && stat == status.client)
				{
					try
					{
						server srvr = new server();
						srvr.ip = tempReceivePoint.ToString().Substring(0, tempReceivePoint.ToString().IndexOf(':'));
						srvr.name = msg.Split('|')[1];
						srvr.port = Convert.ToInt16(msg.Split('|')[2]);
						serverList.Add(srvr);
						if (lbox != null)
							lbox.Items.Add(srvr.ToString());
					}
					catch { }
				}
			}
			//sockRec.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership, new MulticastOption(ipAdr, IPAddress.Any));
			//sockRec.Close();
		}

		/// <summary>Startet den ListenThread() in einem Thread</summary>
		private void listen()
		{
			System.Threading.Thread thr = new System.Threading.Thread(listenThread);
			thr.Start();
		}

		/// <summary>Sendet ein neues Broadcast Packet</summary>
		/// <param name="text">Der Text der gesendet werden soll</param>
		private void sendMsg(string text)
		{
			Socket sockSend = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			sockSend.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 1);
			byte[] send_buffer = Encoding.ASCII.GetBytes(text);
			try
			{
				sockSend.SendTo(send_buffer, 0, send_buffer.Length, SocketFlags.None, multicastEnd);
				sockSend.Close();
			}
			catch (Exception e)
			{ Console.WriteLine("das musticastpacket konnte nicht gesendet werden: " + e.Message); }
		}
	}
}
