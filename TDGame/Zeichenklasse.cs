using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TDGame
{
    /// <summary>
    /// Diese Klasse erzeugt das Zeichenfenster mit dem das Spiel gezeichnet wird
    /// </summary>
    public class Zeichenklasse : Control
    {
        /// <summary>
        /// Das objekt dieser klasse wird zum zeichnen des fensters verwendet.
        /// </summary>
        public Zeichenklasse()
        {
            //Damit die bildberechnung schneller lauft.
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }


    }
}
