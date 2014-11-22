using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Statisch
{
    /// <summary>
    /// Speichert das spielfeld
    /// </summary>
    public class KarteS
    {
        /// <summary>
        /// Spielfeldarray mit dem Block datentyp. Felder mit null bedeutet ein nicht besetzes feld.
        /// </summary>
        public Block[,] karteS;

        /// <summary>
        /// erstellt das Karten Statisch objekt und initiiiialisiert das Kartenarray
        /// </summary>
        public KarteS()
        {
            karteS = new Block[Global.feldgroesse * 2, Global.feldgroesse];
        }

        /// <summary>
        /// Testet ob an den koordinaten ein feindliches gebaude steht.
        /// </summary>
        /// <param name="koordinaten">koordinaten in pixel</param>
        /// <param name="ursprungskoordinaten">Die Ursprungskoordinaten der Kugel.(Feldgröße)</param>
        /// <param name="spieler">Der Spieler dem das projektil gehört</param>
        /// <param name="projektileffekt">Der Projektileffekt, damit positive effekte auch bei den eigenen türmen ankommen</param>
        /// <returns>True bedeutet kollision, false keine kollision</returns>
        public bool testeKollision(Point koordinaten, Point ursprungskoordinaten, Spieler spieler, Projektileffekt projektileffekt)//testet ob spielfeld leer, wenn leer, return false
        {
            if (projektileffekt != Projektileffekt.schaden)
            {
                if (karteS[koordinaten.X / Global.blockgroesse, koordinaten.Y / Global.blockgroesse] != null && !(koordinaten.X / Global.blockgroesse == ursprungskoordinaten.X && koordinaten.Y / Global.blockgroesse == ursprungskoordinaten.Y))
                {
                    return true;
                }
                else return false;
            }
            else
            {
                if (karteS[koordinaten.X / Global.blockgroesse, koordinaten.Y / Global.blockgroesse] != null && spieler != karteS[koordinaten.X / Global.blockgroesse, koordinaten.Y / Global.blockgroesse].spieler)
                {
                    return true;
                }
                else return false;
            }

        }
    }
}
