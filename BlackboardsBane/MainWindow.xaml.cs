using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public class ClassDetails
    {
        public string ClassName { get; set; }
        public Brush ClassColor { get; set; }
        public Brush ClassTextColor { get; set; }

        public ClassDetails(string className, Brush classColor)
        {
            ClassName = className;
            ClassColor = classColor;
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

    public partial class MainWindow : Window
    {
        public DragonFlyCEF df;
        public FakeApi fapi;
        public ObservableCollection<ClassDetails> ClassList;
        public MainWindow(DragonFlyCEF df)
        {
            this.df = df;
            this.fapi = new FakeApi(df);
            ClassList = new ObservableCollection<ClassDetails>();
            InitializeComponent();
            classList.ItemsSource = ClassList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ClassList.Add(new ClassDetails("Class 1", Brushes.Red));
            ClassList.Add(new ClassDetails("Class 2", Brushes.Lime));
            ClassList.Add(new ClassDetails("Class 3", Brushes.LightBlue));
        }
    }
}
