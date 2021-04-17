using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace BlackboardsBane
{
    [Serializable]
    public class ClassDetails
    {
        [XmlAttribute("ClassName")]
        public string ClassName { get; set; }
        [XmlAttribute("ClassColor")]
        public Brush ClassColor { get; set; }
        [XmlAttribute("ClassTextColor")]
        public Brush ClassTextColor { get; set; }
        [XmlAttribute("ClassUrl")]
        public string ClassUrl { get; set; }
        [XmlAttribute("ClassEnabled")]
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
