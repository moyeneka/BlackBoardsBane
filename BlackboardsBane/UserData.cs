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
    }
}
