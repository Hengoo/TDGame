using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace TDGame.Kommunikation
{
    /// <summary>
    /// Der CLientteil der die Kommunikation mit dem Server übernimmt
    /// </summary>
    public class Client
    {

        /// <summary>
        /// Das was der Client von dem Server empfängt
        /// </summary>
        public TcpClient tcpclient;
        /// <summary>
        /// UDPM Objekt zum Finden von Servern im Netzwerk
        /// </summary>
        private UDPMulticast udpm;
        /// <summary>
        /// Liste der offenen, gefundenen Server
        /// </summary>
        private List<UDPMulticast.server> udpms;
        /// <summary>
        /// Speichert verschiendene attribute und Objekte die zur kommunikation nötig sind
        /// </summary>
        public Verwaltung me;
        /// <summary>
        /// Objektreferenz zum Clienten
        /// </summary>
        public Form1 form;
        /// <summary>
        /// Thread für das Empfangen von Nachrichten.
        /// </summary>
        public Thread t;

        /// <summary>
        /// Erstellt das Client objekt und initialisiert es
        /// </summary>
        /// <param name="neueform"></param>
        public Client(Form1 neueform)
        {
            form = neueform;
            udpm = new UDPMulticast();
            udpms = new List<UDPMulticast.server>();
            tcpclient = new TcpClient();
            me = new Verwaltung();
        }

        /// <summary>
        /// Sendet den in String codierten befehl
        /// </summary>
        public void Senden(String code)
        {
            try
            {
                me.streamw.Lock();
                me.streamw.Write(code);
                me.streamw.Unlock();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Thread um auf eingehende nachrichten zu warten, und entschlüßenln zu lassen.
        /// </summary>
        private void Empfangen()
        {
            String nachricht;
            try
            {
                while (form.running)
                {
                    Thread.Sleep(1);
                    nachricht = me.streamr.ReadString();
                    Global.entschluesseln(nachricht, this);
                }
            }
            catch (Exception)
            {
                form.verbindungGetrennt = true;
                form.spielLauft = false;
            }
        }

        /// <summary>
        /// Aktualisiert die Serverliste
        /// </summary>
        public void neuLaden()
        {
            if (udpm.working)
            {
                return;
            }
            form.listBox1.Items.Clear();
            udpms.Clear();
            udpm.ServerSearch(udpms, null);
            while (udpm.working)
            {
                Thread.Sleep(1);
            }
            foreach (UDPMulticast.server s in udpms)
            {
                form.listBox1.Items.Add(s.ToString());
            }
        }


        /// <summary>
        /// Verbindet mit dem in der Listbox ausgewählten Server
        /// </summary>
        public void Verbinden()
        {
            if (tcpclient != null)
            {
                tcpclient.Close();
                tcpclient = new TcpClient();
            }
            if (form.listBox1.SelectedIndex != -1)
            {
                UDPMulticast.server s = udpms[form.listBox1.SelectedIndex];
                try
                {
                    tcpclient.Connect(s.ip, s.port);
                }
                catch (Exception)
                {
                    return; //Probleme aufgetreten, wie z.b wenn kein Server in der Adresse Gefunden wurde.
                }

                me.stream = tcpclient.GetStream();
                me.streamw = new LockedBinaryWriter(me.stream);
                me.streamr = new BinaryReader(me.stream);

                me.spieler = (Spieler)me.streamr.ReadByte();
                form.spieler = me.spieler;

                t = new Thread(Empfangen);
                t.Start();
                form.gewonnen = false;
                form.verloren = false;
                form.karteStatisch = new Statisch.KarteS();
                form.karteDynamisch = new Dynamisch.KarteD();

                form.panel1.Visible = false;
                form.zeichenfenster.Focus();
            }
        }

        /// <summary>
        /// überladene Methode zum verbinden mit ip und port
        /// </summary>
        public void Verbinden(String adresse, int port)
        {
            IPAddress ip = IPAddress.Parse(adresse);

            if (tcpclient != null)
            {
                tcpclient.Close();
                tcpclient = new TcpClient();
            }

            try
            {
                tcpclient.Connect(ip, port);
            }
            catch (Exception)
            {
                return;//Kein Server gefunden.
            }

            me.stream = tcpclient.GetStream();
            me.streamw = new LockedBinaryWriter(me.stream);
            me.streamr = new BinaryReader(me.stream);

            me.spieler = (Spieler)me.streamr.ReadByte();
            form.spieler = me.spieler;

            t = new Thread(Empfangen);
            t.Start();
            form.gewonnen = false;
            form.verloren = false;
            form.karteStatisch = new Statisch.KarteS();
            form.karteDynamisch = new Dynamisch.KarteD();

            form.panel1.Visible = false;
            form.zeichenfenster.Focus();

        }
    }
}
