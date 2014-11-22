using System;
using System.IO;
using System.Text;
using System.Net.Sockets;

namespace TDGame.Kommunikation
{
    /// <summary>
    /// Verwaltet für den Server oder Clienten verschiedene Objektverweiße oder werte
    /// </summary>
    public class Verwaltung
    {
        /// <summary>
        /// Benötigtes Socket für die Datenübertragung
        /// </summary>
        public NetworkStream stream;
        /// <summary>
        /// Benötigtes Socket für das Senden von Daten
        /// </summary>
        public LockedBinaryWriter streamw;
        /// <summary>
        /// Benötigtes Socket für das Empfangen von Daten
        /// </summary>
        public BinaryReader streamr;
        /// <summary>
        /// Speichert den zugehörigen Spieler
        /// </summary>
        public Spieler spieler;
        /// <summary>
        /// Speichert das von dem spieler bessessene Geld
        /// </summary>
        public int geld;

        /// <summary>
        /// erstellt das Verwaltungs Objekt
        /// </summary>
        public Verwaltung()
        {

        }

    }
}
