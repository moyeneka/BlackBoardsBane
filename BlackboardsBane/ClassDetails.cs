using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace BlackboardsBane
{
    public class ClassDetails
    {
        public string ClassName { get; set; }
        public Brush ClassColor { get; set; }
        public Brush ClassTextColor { get; set; }
        public int ClassCalColor { get; set; }
        public string ClassUrl { get; set; }
        public bool Enabled { get; set; }

        public ClassDetails() { }

        public ClassDetails(string className, Brush classColor, string classUrl)
        {
            ClassName = className;
            ClassColor = classColor;
            ClassUrl = classUrl;
            Color brushColor = (Color)classColor.GetValue(SolidColorBrush.ColorProperty);
            int r = brushColor.R;
            int g = brushColor.G;
            int b = brushColor.B;
            double y = 0.2126 * Math.Pow(r / 255, 2.2) + 0.7151 * Math.Pow(g / 255, 2.2) + 0.0721 * Math.Pow(b / 255, 2.2);
            if (y < 0.5)
            {
                ClassTextColor = Brushes.White;
            }
            else
            {
                ClassTextColor = Brushes.Black;
            }
        }
    }
}
