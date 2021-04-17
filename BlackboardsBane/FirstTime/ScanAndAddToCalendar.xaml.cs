using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using static BlackboardsBane.DragonFlyCEF;

namespace BlackboardsBane.FirstTime
{
    /// <summary>
    /// Interaction logic for ScanAndAddToCalendar.xaml
    /// </summary>
    // despite the name this doesn't add to the calendar yet
    public partial class ScanAndAddToCalendar : Page
    {
        public DragonFlyCEF df;
        public UserData ud;
        public Setup setup;
        public FakeApi fapi;
        private bool finished = false;
        public ScanAndAddToCalendar(DragonFlyCEF df, UserData ud, Setup setup)
        {
            this.df = df;
            this.ud = ud;
            this.setup = setup;
            fapi = new FakeApi(df);
            InitializeComponent();
        }

        private async void StartScan()
        {
            List<ClassDetails> classDetails = ud.classDetails;
            List<AssignmentDetails> assignmentDetails = ud.assignmentDetails;

            AutoResetEvent waitHandle = new AutoResetEvent(false);
            EmptyEventHandler eventHandler = delegate (object sender)
            {
                waitHandle.Set();
            };
            df.DOMLoaded += eventHandler;

            //test
            foreach (ClassDetails det in classDetails)
            {
                if (!det.Enabled)
                    continue;

                ////////WAIT FOR SITE TO LOAD (HACK)
                df.browser.Load(det.ClassUrl);
                waitHandle.WaitOne();
                waitHandle.Reset();

                bool hasAssignments = false;
                for (int c = 0; c < 4; c++)
                {
                    await Task.Delay(300); //amazing programming skill

                    hasAssignments = await df.ElementExists("dueView");
                    if (hasAssignments)
                        break;
                }
                ////////WAIT FOR SITE TO LOAD (HACK)

                if (!hasAssignments)
                {
                    //return;
                    continue;
                }

                await Task.Delay(300); //extra delay for the actual items I guess

                //Application.Current.Dispatcher.Invoke(delegate
                //{
                //    MessageBox.Show("loading stuff for class " + det.ClassName + " at " + det.ClassUrl);
                //});
                //assignmentDetails.Add(new AssignmentDetails(det.ClassName, Brushes.Red, ""));

                List<AssignmentDetails> thisClassAssignments = new List<AssignmentDetails>();
                var assignmentToInfo = new Dictionary<AssignmentDetails, (string, string)>();
                for (int k = 0; k < 4; k++)
                {
                    FakeApi_DueDatePeriod ddp = (FakeApi_DueDatePeriod)k;
                    int dueAssignmentCount = await fapi.GetDueAssignmentCount(ddp);
                    for (int i = 0; i < dueAssignmentCount; i++)
                    {
                        string dueAssignmentName = await fapi.GetDueAssignmentTitle(ddp, i);
                        string dueAssignmentJs = await fapi.GetDueAssignmentJS(ddp, i);
                        string dueAssignmentUrl = "idk yet lol just be patient";

                        //Application.Current.Dispatcher.Invoke(delegate
                        //{
                        //    MessageBox.Show(dueAssignmentJs);
                        //});

                        AssignmentDetails ad = new AssignmentDetails(dueAssignmentName, Brushes.White, dueAssignmentUrl, det);
                        thisClassAssignments.Add(ad);
                        assignmentDetails.Add(ad);
                        MatchCollection urlReg = Regex.Matches(dueAssignmentJs, "'(?:[^']+|\\\\.)*'");

                        var nid = urlReg[0].Value;
                        var ak = urlReg[1].Value;

                        assignmentToInfo[ad] = (nid, ak);
                    }
                }

                foreach (AssignmentDetails ad in thisClassAssignments)
                {
                    (string nid2, string ak2) = assignmentToInfo[ad];
                    string url2 = await fapi.GetAssignmentPage(nid2, ak2);
                    url2 = "https://learn.uark.edu/" + url2;
                    ad.AssignmentUrl = url2;
                }

                foreach (AssignmentDetails ad in thisClassAssignments)
                {
                    df.browser.Load(ad.AssignmentUrl);
                    waitHandle.WaitOne();
                    waitHandle.Reset();

                    //Monday, April 19, 2021
                    //11:59 PM
                    string assignmentDueDate = await fapi.GetAssignmentDueDate();
                    if (assignmentDueDate == null)
                        continue;

                    string dateLine = assignmentDueDate.Split('\n')[0];
                    string timeLine = assignmentDueDate.Split('\n')[1];
                    DateTime date = DateTime.Parse(dateLine + " " + timeLine); //eyyy thanks c#
                    ad.AssignmentDate = date;
                }
            }

            Application.Current.Dispatcher.Invoke(delegate
            {
                assignmentListBox.ItemsSource = assignmentDetails;
                infoLabel.Text = "Done scanning...";
                startBtn.Content = "Finish";
                finished = true;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!finished)
                StartScan();
            else
                setup.FinishScanClasses();
        }
    }
}
