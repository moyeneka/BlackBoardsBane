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

namespace BlackboardsBane.FirstTime
{
    /// <summary>
    /// Interaction logic for AddClasses.xaml
    /// </summary>
    public partial class AddClasses : Page
    {
        public DragonFlyCEF df;
        public FakeApi fapi;
        public ObservableCollection<ClassDetails> ClassList;
        public bool loadedYet = false;

        public AddClasses(DragonFlyCEF df)
        {
            this.df = df;
            this.fapi = new FakeApi(df);
            ClassList = new ObservableCollection<ClassDetails>();
            InitializeComponent();
            classList.ItemsSource = ClassList;

            PopulateClassDetailList();
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
                ClassDetails dets = new ClassDetails(className, Brushes.White, classUrl);
                dets.Enabled = true;
                classListTemp.Add(dets);
            }
            Application.Current.Dispatcher.Invoke(delegate
            {
                foreach (var i in classListTemp)
                    ClassList.Add(i);
            });
        }
    }
}
