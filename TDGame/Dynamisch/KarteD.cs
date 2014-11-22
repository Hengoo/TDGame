using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDGame.Dynamisch
{
    /// <summary>
    /// Objekt das die beiden Dynamischen Listen für die Dynamischen Objekte der Kugeln und der Türme speichert
    /// </summary>
    public class KarteD
    {
        /// <summary>
        /// Die liste aller Dynamischen Mobilen Objekte
        /// </summary>
        public List<Kugel> karteKD;
        /// <summary>
        /// Die Liste aller Dynamischen Turm Objekte
        /// </summary>
        public List<TurmD> karteTD;

        /// <summary>
        /// erstellt das Karten Objekt und initialisiert die Listen
        /// </summary>
        public KarteD()
        {
            karteKD = new List<Kugel>();
            karteTD = new List<TurmD>();
        }
    }
}
