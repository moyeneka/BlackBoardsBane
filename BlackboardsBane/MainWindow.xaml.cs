using BlackboardsBane.FirstTime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

namespace BlackboardsBane
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    //public class MainWindowMVVM
    //{
    //    public List<ClassDetails> ClassList
    //    {
    //        get
    //        {
    //            List<ClassDetails> ClassList = new List<ClassDetails>();
    //            ClassList.Add(new ClassDetails("Class 1", Brushes.Red));
    //            ClassList.Add(new ClassDetails("Class 2", Brushes.Lime));
    //            ClassList.Add(new ClassDetails("Class 3", Brushes.Blue));
    //            return ClassList;
    //        }
    //    }
    //}

    public class AssignmentDetails
    {
        public string AssignmentName { get; set; }
        public Brush AssignmentColor { get; set; }
        public Brush AssignmentTextColor { get; set; }
        public string AssignmentUrl { get; set; }

        public AssignmentDetails(string assignmentName, Brush assignmentColor, string assignmentUrl)
        {
            AssignmentName = assignmentName;
            AssignmentColor = assignmentColor;
            AssignmentUrl = assignmentUrl;
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
    }

    public partial class MainWindow : Window
    {
        public DragonFlyCEF df;
        public FakeApi fapi;
        public ObservableCollection<ClassDetails> ClassList;
        public ObservableCollection<AssignmentDetails> AssignmentList;
        public bool loadedYet = false;

        public MainWindow(DragonFlyCEF df)
        {
            this.df = df;
            this.fapi = new FakeApi(df);
            ClassList = new ObservableCollection<ClassDetails>();
            AssignmentList = new ObservableCollection<AssignmentDetails>();
            InitializeComponent();
            classList.ItemsSource = ClassList;
            assignmentList.ItemsSource = AssignmentList;

            df.DOMLoaded += DomLoaded;
            df.browser.Load("https://learn.uark.edu/webapps/portal/execute/tabs/tabAction?tab_tab_group_id=_1_1");
        }

        private void DomLoaded(object sender)
        {
            if (!loadedYet)
            {
                loadedYet = true;
                Application.Current.Dispatcher.Invoke(delegate
                {
                    Setup setup = new Setup(df);
                    setup.Show();
                });
                //PopulateClassDetailList();
            }
        }

        private async void PopulateClassDetailList()
        {
            List<ClassDetails> classListTemp = new List<ClassDetails>();
            List<AssignmentDetails> assignmentListTemp = new List<AssignmentDetails>();
            await Task.Delay(1000); //wait for dumb blackboard

            int classCount = await fapi.GetClassCount();
            for (int i = 0; i < classCount; i++)
            {
                string className = await fapi.GetClassNameAtIndex(i);
                string classUrl = await fapi.GetClassURLAtIndex(i);
                classListTemp.Add(new ClassDetails(className, Brushes.White, classUrl));
            }
            Application.Current.Dispatcher.Invoke(delegate
            {
                foreach (var i in classListTemp)
                    ClassList.Add(i);
            });

            foreach (ClassDetails det in classListTemp)
            {
                ////////WAIT FOR SITE TO LOAD (HACK)
                AutoResetEvent waitHandle = new AutoResetEvent(false);
                EmptyEventHandler eventHandler = delegate (object sender)
                {
                    waitHandle.Set();
                };
                df.DOMLoaded += eventHandler;
                df.browser.Load(det.ClassUrl);
                waitHandle.WaitOne();
                ////////WAIT FOR SITE TO LOAD (HACK)

                bool hasAssignments = false;
                for (int c = 0; c < 5; c++)
                {
                    await Task.Delay(300); //amazing programming skill

                    hasAssignments = await df.ElementExists("dueView");
                    if (hasAssignments)
                        break;
                }

                if (!hasAssignments)
                {
                    continue;
                }

                await Task.Delay(300); //extra delay for the actual items I guess

                //Application.Current.Dispatcher.Invoke(delegate
                //{
                //    MessageBox.Show("loading stuff for class " + det.ClassName + " at " + det.ClassUrl);
                //});
                assignmentListTemp.Add(new AssignmentDetails(det.ClassName, Brushes.Red, ""));

                for (int k = 0; k < 4; k++)
                {
                    FakeApi_DueDatePeriod ddp = (FakeApi_DueDatePeriod)k;
                    int dueAssignmentCount = await fapi.GetDueAssignmentCount(ddp);
                    for (int i = 0; i < dueAssignmentCount; i++)
                    {
                        string dueAssignmentName = await fapi.GetDueAssignmentTitle(ddp, i);
                        string dueAssignmentUrl = "lol idk";
                        assignmentListTemp.Add(new AssignmentDetails(dueAssignmentName, Brushes.White, dueAssignmentUrl));
                    }
                }

                Application.Current.Dispatcher.Invoke(delegate
                {
                    AssignmentList.Clear();
                    foreach (var i in assignmentListTemp)
                        AssignmentList.Add(i);
                });
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
