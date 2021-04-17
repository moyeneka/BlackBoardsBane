using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlackboardsBane.FirstTime
{
    /// <summary>
    /// Interaction logic for AddToCalendar.xaml
    /// </summary>
    public partial class AddToCalendar : Page
    {
        public DragonFlyCEF df;
        public UserData ud;
        public Setup setup;
        public GCalendar gc;
        public AddToCalendar(DragonFlyCEF df, UserData ud, Setup setup)
        {
            this.df = df;
            this.ud = ud;
            this.setup = setup;
            gc = new GCalendar();
            InitializeComponent();
        }

        private async void addCalendarBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!await gc.Init())
            {
                MessageBox.Show("oof, couldn't connect to calendar api, skipping step");
                setup.FinishAll();
            }
            string cid = gc.AddOrFindCalendar("BBB Calendar", "Blackboard assignments and links here");

            foreach (var a in ud.assignmentDetails)
            {
                DateTime start;
                DateTime end;
                DateTime assignmentDate = a.AssignmentDate;
                if (assignmentDate == DateTime.MinValue)
                {
                    start = DateTime.Today;
                    end = DateTime.Today.AddHours(1);
                }
                else
                {
                    start = a.AssignmentDate.AddHours(-1);
                    end = a.AssignmentDate;
                }
                gc.AddItemToCalendar(a.AssignmentName, "Link: " + a.AssignmentUrl, a.AssignmentClass.ClassCalColor, start, end, cid);
            }
            setup.FinishAll();
        }

        private void skipBtn_Click(object sender, RoutedEventArgs e)
        {
            setup.FinishAll();
        }
    }
}
