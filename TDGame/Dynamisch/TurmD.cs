using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Dynamisch
{
    /// <summary>
    /// Klasse des Dynamischen Turms, Weitere Dynamischen Turm versionen erben von diesem.
    /// </summary>
    public class TurmD
    {
        /// <summary>
        /// Der schaden den eine kugel macht
        /// </summary>
        public int schaden;
        /// <summary>
        /// Speichert die momentan vorhandene energie. Wird durch manche spezialtürme aufgefüllt und sehr langsam regeneriert. Maximum 1000!.
        /// </summary>
        public int energie;
        /// <summary>
        /// Gibt die energiekosten von diesem Turm an
        /// </summary>
        public int energiekosten;
        /// <summary>
        /// Gibt die zeit an die der Turm zum abkühlen brauch um erneut zu feuern.
        /// </summary>
        public int Abklingzeit;
        /// <summary>
        /// Gibt die Zeit an die der Turm aktuell noch benötigt um das nächste mal feuern zu können
        /// </summary>
        public int Abklingzeit2;
        /// <summary>
        /// Gibt an ob der Turm gerade feuern soll oder nicht.
        /// </summary>
        public bool feuerbefehl;
        /// <summary>
        /// x und y Koordianten in Pixel
        /// </summary>
        protected Point koordinatenBild;
        /// <summary>
        /// X und Y Koordinaten der Mitte des Bildes in Pixel
        /// </summary>
        protected Point koordinatenMitte;
        /// <summary>
        /// Ausrichutngsrvektor X
        /// </summary>
        public float vektorX;
        /// <summary>
        /// AusrichtungsVektor Y
        /// </summary>
        public float vektorY;
        /// <summary>
        /// Gibt den Faktor an mit dem der Vektor multipliziert wird, um den bewegungsvektor der Kugeln zu erhalten
        /// </summary>
        protected int speed;
        /// <summary>
        /// Der spieler dem der dynamische turmteil gehört, und dem die kugel gehören die der turm abfeuert
        /// </summary>
        public Spieler spieler;
        /// <summary>
        /// Das Bild mit dem der Dynamische Turmteil gezeichnet wird.
        /// </summary>
        protected Bitmap bild;
        /// <summary>
        /// Das Bild der Kugeln
        /// </summary>
        public Bitmap kugelBild;

        /// <summary>
        /// Für Berechnung der BezierKurve
        /// </summary>
        public Point P1;
        /// <summary>
        /// Für Berechnung der BezierKurve
        /// </summary>
        public Point P2;
        /// <summary>
        /// Für Berechnung der BezierKurve
        /// </summary>
        public Point P3;
        /// <summary>
        /// Für Berechnung der BezierKurve
        /// </summary>
        public Point P4;

        /// <summary>
        /// gibt an ob der Turm nach der Bezierart feuert
        /// </summary>
        public bool bezier;

        /// <summary>
        /// Erstellt das Objekt und Initialisert einen teil der Attribute. Danach muss die initialisieren Methode aufgerufen werden
        /// </summary>
        /// <param name="neueKoordinaten"></param>
        /// <param name="neuSpieler"></param>
        public TurmD(Point neueKoordinaten, Spieler neuSpieler)
        {
            koordinatenBild = new Point(neueKoordinaten.X, neueKoordinaten.Y);
            spieler = neuSpieler;
            if (koordinatenBild.X > 600)
            {
                vektorX = -1;
            }
            else vektorX = 1;
            vektorY = 0;
            energie = Global.startenergie;
            feuerbefehl = false;
            bezier = false;
        }

        /// <summary>
        /// Initialisiert die Attribute mit werten.
        /// </summary>
        /// <param name="neuschaden">Der Schaden</param>//
        /// <param name="neuespeed">Die Fluggeschwindigkeit</param>
        /// <param name="neueAbklingzeit">Die Abklingzeit zwischen den Schüssen</param>
        /// <param name="neueEnergiekosten">Die Energiekosten</param>
        /// <param name="neueBildD">Das Bild des DynamischenTurmes</param>
        /// <param name="neueKugelBild">Das Bild der Kugeln</param>
        public virtual void initialisieren(int neuschaden, int neuespeed, int neueAbklingzeit, int neueEnergiekosten, Bitmap neueBildD, Bitmap neueKugelBild)
        {
            schaden = neuschaden;
            speed = neuespeed;
            Abklingzeit = neueAbklingzeit;
            energiekosten = neueEnergiekosten;

            bild = neueBildD;
            kugelBild = neueKugelBild;
            koordinatenMitte = new Point(koordinatenBild.X + Global.blockgroesse / 2, koordinatenBild.Y + Global.blockgroesse / 2);

            if (koordinatenBild.X > 600)
            {
                koordinatenBild.X += (Global.blockgroesse - bild.Width) / 2 + 1;
            }
            else
            {
                koordinatenBild.X += (Global.blockgroesse - bild.Width) / 2;
            }
            koordinatenBild.Y += (Global.blockgroesse - bild.Height) / 2 + 1;
        }

        /// <summary>
        /// Wird aufgerufen Wenn ein Turm neu ausgerichtet wird.
        /// </summary>
        /// <param name="neuVektorX">Der X Teil des Vektors</param>
        /// <param name="neuVektorY">Der Y Teil des Vektors</param>
        public void ausrichten(float neuVektorX, float neuVektorY)
        {
            vektorX = neuVektorX;
            vektorY = neuVektorY;
        }

        /// <summary>
        /// Überprüft ob der Turm bereits wider feuern kann, Wird bei manchen türmen verändert. WIRKLICH??????????????????????????????ß
        /// </summary>
        /// <returns>Gibt true zurück wenn der Turm bereits wieder feuern kann</returns>
        public virtual bool angreifen()
        {
            if (energie < 1000) energie++;

            if (feuerbefehl)
            {
                if (Abklingzeit2 >= Abklingzeit)
                {
                    if (energie - energiekosten >= 0)
                    {
                        energie -= energiekosten;
                        Abklingzeit2 = 0;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Abklingzeit2++;
                    return false;
                }
            }
            else
            {
                if (Abklingzeit2 <= Abklingzeit) Abklingzeit2++;
                return false;
            }
        }

        /// <summary>
        /// Feuert die Kugel ab
        /// </summary>
        /// <returns>Gibt das Kugel Objekt zurück</returns>
        public virtual Kugel feuern()
        {
            return new Kugel(schaden, vektorX, vektorY, speed, koordinatenMitte.X, koordinatenMitte.Y, spieler, Projektileffekt.schaden, kugelBild);
        }

        /// <summary>
        /// Zeichnet den eigenenen Dynamischen Turmteil während des Baumodus, Je nachdem in welche Richtung er zeigt.
        /// </summary>
        /// <param name="g">Graphics zum zeichnen</param>
        /// <param name="spieleraktuell">Der aktuelle Spieler, sodass nur das gezeichnet wird was man sehen soll</param>
        public void zeichnen(Graphics g, Spieler spieleraktuell)
        {
            if (spieler == spieleraktuell)
            {
                g.DrawImage(Global.RotateImage(bild, (float)(Math.Atan2(vektorY, vektorX) * 180 / Math.PI)), koordinatenBild);
            }
        }

        /// <summary>
        /// Zeichnet jeden Dynamischen Turmteil, Je nachdem in welche Richtung er zeigt.
        /// </summary>
        /// <param name="g">Graphics zum zeichnen</param>
        public void zeichnen(Graphics g)
        {
            g.DrawImage(Global.RotateImage(bild, (float)(Math.Atan2(vektorY, vektorX) * 180 / Math.PI)), koordinatenBild);
        }
    }
}
