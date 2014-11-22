using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Dynamisch.VerbesserungenKugel
{
	/// <summary>
	/// Klasse die für die Bewegung der BezierKurvenKugel verwendet wird.
	/// </summary>
	class KugelBezier : Dynamisch.Kugel
	{
		private Point P1;
		private Point P2;
		private Point P3;
		private Point P4;
		private Point zwischenspeicher1;
		private double t;

		public KugelBezier(int neuSchaden, float neuXGeschwindigkeit, float neuYGeschwindigkeit, int Geschwindigkeit, Spieler neuSpieler, Projektileffekt neuProjektil, Bitmap neuBild, Point neuP1, Point neuP2, Point neuP3, Point neuP4)
			: base(neuSchaden, neuXGeschwindigkeit, neuYGeschwindigkeit, Geschwindigkeit, neuP1.X, neuP1.Y, neuSpieler, neuProjektil, neuBild)
		{
			P1 = neuP1;
			P2 = neuP2;
			P3 = neuP3;
			P4 = neuP4;
			t = 0;
			bezierBerechnen(0);
		}

		/// <summary>
		/// Berechnet den Vektor zwischen 2 Punkten
		/// </summary>
		/// <param name="PAnfang">Der AnfangsPunkt</param>
		/// <param name="PEnde">Der Endpunkt</param>
		/// <returns>Der Vektor</returns>
		private Point vektorerrechnen(Point PAnfang, Point PEnde)
		{
			return new Point(PEnde.X - PAnfang.X, PEnde.Y - PAnfang.Y);
		}

		/// <summary>
		/// Berechnet den Punkt zwischen 2 Punkten mit einem Faktor t mithilfe des Vektors.
		/// </summary>
		/// <param name="PAnfang">Der AnfangsPunkt</param>
		/// <param name="PEnde">Der Endpunkt</param>
		/// <param name="tberechnen">Der Faktor, 0 bedeutet beim Anfangspunkt, 1 beim Endpunkt. </param>
		/// <returns>die Koordinaten des Punktes</returns>
		private Point punkterrechnen(Point PAnfang, Point PEnde, double tberechnen)
		{
			Point vektor = vektorerrechnen(PAnfang, PEnde);
			return new Point((int)(vektor.X * tberechnen + PAnfang.X), (int)(vektor.Y * tberechnen + PAnfang.Y));
		}

		/// <summary>
		/// Berechnet den Punkt an der Bezierkurve iterativ abhängig von t, dem Faktor. Berechnung mithilfe von Vektoren. Problem: formel verwendet 2 mal den Anfangspunkt, macht zusammenfassen des Berechnungsschritts in eine Formel unnötig schwer
		/// </summary>
		/// <param name="tberechnen">Der Faktor, wenn er zwischen 0 und 1 wird der Punkt auf der Kurveliegen, ansonsten auf geraden hinter/vor der Kurve</param>
		/// <returns>Die Korrdinaten des Punktes der BezierKurve</returns>
		private Point bezierBerechnen(double tberechnen)
		{
			Point zwischenspeicher2;
			zwischenspeicher2 = zwischenspeicher1;
			//P5-P9 sind die Imaginären Punkte die zur Berechnung des Bezierkurvenpunktes P10 notwendig sind.
			Point P5 = punkterrechnen(P1, P2, tberechnen);
			Point P6 = punkterrechnen(P2, P3, tberechnen);
			Point P7 = punkterrechnen(P3, P4, tberechnen);
			Point P8 = punkterrechnen(P5, P6, tberechnen);
			Point P9 = punkterrechnen(P6, P7, tberechnen);
			Point P10 = punkterrechnen(P8, P9, tberechnen);

			zwischenspeicher1 = P10;

			//wird benötigt um die Momentane flugrichtung für das zeichnen zu errechnen
			xGeschwindigkeit = zwischenspeicher1.X - zwischenspeicher2.X;
			yGeschwindigkeit = zwischenspeicher1.Y - zwischenspeicher2.Y;

			return zwischenspeicher2;
		}

		/// <summary>
		/// Formel für das Berechnen eines Punktes mit einem Faktor und 2 Punkten. Berechnung geschieht ohne Vektorberechnung.
		/// </summary>
		/// <param name="PAnfang"></param>
		/// <param name="PEnde"></param>
		/// <param name="tberechnen"></param>
		/// <returns></returns>
		private Point punkterrechnen2(Point PAnfang, Point PEnde, double tberechnen)
		{
			return new Point((int)((1 - tberechnen) * PAnfang.X + tberechnen * PEnde.X), (int)((1 - tberechnen) * PAnfang.Y + tberechnen * PEnde.Y));
		}

		/// <summary>
		/// Direkte Berechnung der Punkte, ohne den Umweg mit den Vektoren, macht das zusammenfassen in eine Formel sehr viel einfacher. Immernoch Iterative berechnung des Endpunktes nötig.
		/// </summary>
		/// <param name="tberechnen"></param>
		/// <returns></returns>
		private Point bezierBerechnen2(double tberechnen)
		{
			Point zwischenspeicher2;
			zwischenspeicher2 = zwischenspeicher1;
			//P5-P9 sind die Imaginären Punkte die zur Berechnung des Bezierkurvenpunktes P10 notwendig sind.
			Point P5 = punkterrechnen2(P1, P2, tberechnen);
			Point P6 = punkterrechnen2(P2, P3, tberechnen);
			Point P7 = punkterrechnen2(P3, P4, tberechnen);
			Point P8 = punkterrechnen2(P5, P6, tberechnen);
			Point P9 = punkterrechnen2(P6, P7, tberechnen);
			Point P10 = punkterrechnen2(P8, P9, tberechnen);

			zwischenspeicher1 = P10;

			//wird benötigt um die Momentane flugrichtung für das zeichnen zu errechnen
			xGeschwindigkeit = zwischenspeicher1.X - zwischenspeicher2.X;
			yGeschwindigkeit = zwischenspeicher1.Y - zwischenspeicher2.Y;

			return zwischenspeicher2;
		}

		/// <summary>
		/// Berechnen direkt den Punkt auf der BezierKurve, mit dem ansatz aus berechnen2, nur in eine Formel zusammengefasst. Keine iteration mehr nötig.
		/// </summary>
		/// <param name="tb">mit welchem t der Punkt berechnet werden soll</param>
		/// <returns></returns>
		private Point bezierBerechnenRichtig(double tb)
		{
			Point zwischenspeicher2;
			zwischenspeicher2 = zwischenspeicher1;

			//brechnung des 10. Punktes ohne der Imaginären Punkte.
			Point P10 = new Point((int)((1 - tb) * (1 - tb) * (1 - tb) * P1.X + 3 * tb * (1 - tb) * (1 - tb) * P2.X + 3 * tb * tb * (1 - tb) * P3.X + P4.X * tb * tb * tb), (int)((1 - tb) * (1 - tb) * (1 - tb) * P1.Y + 3 * tb * (1 - tb) * (1 - tb) * P2.Y + 3 * tb * tb * (1 - tb) * P3.Y + P4.Y * tb * tb * tb));

			zwischenspeicher1 = P10;

			//wird benötigt um die Momentane flugrichtung für das zeichnen zu errechnen
			xGeschwindigkeit = zwischenspeicher1.X - zwischenspeicher2.X;
			yGeschwindigkeit = zwischenspeicher1.Y - zwischenspeicher2.Y;

			return zwischenspeicher2;

		}

		/// <summary>
		/// Berechnet den Punkt an der BezierKurve, und verschiebt das Projektil dorthin
		/// </summary>
		/// <returns>Die Koordinaten des Projektils</returns>
		public override Point bewegen()
		{
			if (t <= 1)
			{
				t += 0.005;
				koordinaten = bezierBerechnenRichtig(t + 0.005);
				koordinateX = koordinaten.X;
				koordinateY = koordinaten.Y;
				return koordinaten;
			}
			else
			{
				t += 0.001;
				Point vektor = vektorerrechnen(P3, P4);
				koordinateX += (float)(((1 / Math.Sqrt(vektor.X * vektor.X + vektor.Y * vektor.Y)) * vektor.X) * t * 3);
				koordinateY += (float)(((1 / Math.Sqrt(vektor.X * vektor.X + vektor.Y * vektor.Y)) * vektor.Y) * t * 3);
				koordinaten.X = (int)koordinateX;
				koordinaten.Y = (int)koordinateY;
				return koordinaten;
			}
		}

		/// <summary>
		/// Berechnet die threoretischen Koordinaten des Projektils beim nächsten zug.
		/// </summary>
		/// <returns>Die Threoretischen Koordinaten</returns>
		public override Point naechsteKoordinaten()
		{
			if (t <= 1) return zwischenspeicher1;// bezierBerechnen(t + 0.001);
			else
			{
				Point vektor = vektorerrechnen(P3, P4);
				return new Point((int)(koordinateX + (((1 / Math.Sqrt(vektor.X * vektor.X + vektor.Y * vektor.Y)) * vektor.X) * (t + 0.001) * 3)), (int)(koordinateY + (((1 / Math.Sqrt(vektor.X * vektor.X + vektor.Y * vektor.Y)) * vektor.Y) * (t + 0.001) * 3)));
			}
		}
	}
}
