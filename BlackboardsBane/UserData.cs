using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlackboardsBane
{
    public class UserData
    {
        public List<ClassDetails> classDetails = new List<ClassDetails>();
        public List<AssignmentDetails> assignmentDetails = new List<AssignmentDetails>();
        public void Load()
        {
            XmlSerializer serc = new XmlSerializer(classDetails.GetType());
            using (TextReader tr = new StreamReader("classdetails.xml"))
            {
                classDetails = (List<ClassDetails>)serc.Deserialize(tr);
            }
            XmlSerializer sera = new XmlSerializer(assignmentDetails.GetType());
            using (TextReader tr = new StreamReader("assignmentdetails.xml"))
            {
                classDetails = (List<ClassDetails>)sera.Deserialize(tr);
            }
        }
        public void Save()
        {
            XmlSerializer sera = new XmlSerializer(classDetails.GetType());
            using (TextWriter tw = new StreamWriter("classdetails.xml"))
            {
                sera.Serialize(tw, classDetails);
            }
            XmlSerializer serc = new XmlSerializer(assignmentDetails.GetType());
            using (TextWriter tw = new StreamWriter("assignmentdetails.xml"))
            {
                serc.Serialize(tw, assignmentDetails);
            }
        }
    }
}
