using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Drawing;

namespace TDGame.Kommunikation
{
    /// <summary>
    /// Klasse die den Kommunikationsteil des Server übernimmt
    /// </summary>
    public class Server
    {
        /// <summary>
        /// Benötigtes Socket für die Datenübertragung
        /// </summary>
        private TcpListener tcpserver;
        /// <summary>
        /// Benötigtes Socket für die Datenübertragung
        /// </summary>
        private TcpClient tcpclient;
        /// <summary>
        /// Macht den Server in allen eigenen Netzwerken sichtbar
        /// </summary>
        public UDPMulticast udpm;
        /// <summary>
        /// Liste aller verbundenen Spieler
        /// </summary>
        public List<Verwaltung> con_list;
        /// <summary>
        ///Thread der neue Spieler verbindet
        ///</summary>
        private Thread search;
        /// <summary>
        /// Verwaltet alle Asynchronen und Synchronen Datenübertragungen an die Clienten
        /// </summary>
        public BinaryWriterCollection BWC;
        /// <summary>
        /// Für die nummerierung der verwaltungsklassen und Spieler
        /// </summary>
        public byte zaehler;
        /// <summary>
        /// Zeit an ob der Server noch lauft, bei false werden alle dauerschleifen beendet
        /// </summary>
        public bool running;
        /// <summary>
        /// Objektreferenz zu dem Server
        /// </summary>
        public Form2 form;
        /// <summary>
        /// Gibt an ob bereits 2 Spieler mit dem server verbunden sind, wenn ja wird sie true und der Server wird nicht mehr in der Server-Such-Liste angezeit
        /// </summary>
        public bool voll;

        /// <summary>
        /// Erstellt und initialisiert den Server
        /// </summary>
        /// <param name="neueform">Objektreferenz zu der Form</param>
        public Server(Form2 neueform)
        {
            form = neueform;
            con_list = new List<Verwaltung>();
            BWC = new BinaryWriterCollection(con_list);
            udpm = new UDPMulticast();
            tcpserver = udpm.ServerView("TDG"); // nicht benötigt momentan ??
            zaehler = 0; //wird bei spielervergabe verwendet, wenn zähler 2 --> spiel momentan voll 
            running = true;

            search = new Thread(Mainproc);
            startThread();
            voll = false;
        }

        /// <summary>
        /// Startet den Thread für den Datenaustausch
        /// </summary>
        public void startThread()
        {
            search.Start();
        }

        /// <summary>
        /// Wird vom search-Thread (UDPM) ausgeführt um neue Spieler zu verbinden
        /// </summary>
        private void Mainproc()
        {
            while (running && !voll)
            {
                try
                {
                    tcpclient = tcpserver.AcceptTcpClient();
                }
                catch
                {
                    return;
                }

                Verwaltung c = new Verwaltung();
                if (zaehler == 0) c.spieler = Spieler.spielerA;
                else if (zaehler == 1) c.spieler = Spieler.spielerB;
                c.geld = Global.startgeld;

                zaehler++;
                c.stream = tcpclient.GetStream();
                c.streamr = new BinaryReader(c.stream);
                c.streamw = new LockedBinaryWriter(c.stream);

                con_list.Add(c);
                c.streamw.Lock();
                c.streamw.Write((byte)c.spieler);
                c.streamw.Unlock();

                Thread t = new Thread(delegate() { warten(c); });
                t.Start();

                if (zaehler == 2)
                {
                    voll = true;
                    udpm.Close();
                }
            }
        }

        /// <summary>
        /// Methode zum sicheren Schreiben von Nachrichten
        /// </summary>
        /// <param name="nachricht">Die zu schreibende String nachricht</param>
        public void schreiben(string nachricht)
        {
            if (running)
            {
                BWC.Lock();
                BWC.Write(nachricht);
                BWC.Unlock();
            }
        }

        /// <summary>
        /// Methode die aufgerufen wird wenn den clienten etwas gesendet wird (allen clienten !?!) PS: vieleicht mit return result und andere methode im thread -------------------------------------f alsche beschreibung
        /// </summary>
        /// <param name="con"></param>
        private void warten(Verwaltung con)
        {
            String result;
            while (running)
            {
                try
                {
                    result = con.streamr.ReadString();
                    Global.entschluesseln(result, this);
                }
                catch
                {
                    if (running)
                    {
                        running = false;
                        try
                        {
                            con_list.Remove(con);
                        }
                        catch (Exception)
                        {
                        }

                        form.reset();
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// Beendet alle laufende threads, damit der Server geschloßen werden kann
        /// </summary>
        public void schliessen()
        {
            running = false;
            tcpserver.Stop();
            search.Abort();
            udpm.Close();
            foreach (Verwaltung v in con_list)
            {
                v.stream.Close();
            }
        }
    }
}
