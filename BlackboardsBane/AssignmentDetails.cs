using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace BlackboardsBane
{
    public class AssignmentDetails
    {
        public string AssignmentName { get; set; }
        public Brush AssignmentColor { get; set; }
        public Brush AssignmentTextColor { get; set; }
        public string AssignmentUrl { get; set; }
        public ClassDetails AssignmentClass { get; set; }
        public DateTime AssignmentDate { get; set; } = DateTime.MinValue;

        public AssignmentDetails() { }

        public AssignmentDetails(string assignmentName, Brush assignmentColor, string assignmentUrl, ClassDetails classDetails)
        {
            AssignmentName = assignmentName;
            AssignmentColor = assignmentColor;
            AssignmentUrl = assignmentUrl;
            AssignmentClass = classDetails;
            Color brushColor = (Color)assignmentColor.GetValue(SolidColorBrush.ColorProperty);
            int r = brushColor.R;
            int g = brushColor.G;
            int b = brushColor.B;
            double y = 0.2126 * Math.Pow(r / 255, 2.2) + 0.7151 * Math.Pow(g / 255, 2.2) + 0.0721 * Math.Pow(b / 255, 2.2);
            if (y < 0.5)
            {
                AssignmentTextColor = Brushes.White;
            }
            else
            {
                AssignmentTextColor = Brushes.Black;
            }
        }

        public override string ToString()
        {
            if (AssignmentDate == DateTime.MinValue)
                return AssignmentName;
            else
                return AssignmentName + " due " + AssignmentDate;
        }
    }
}
