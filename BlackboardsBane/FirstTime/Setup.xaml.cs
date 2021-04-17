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
using System.Windows.Shapes;

namespace BlackboardsBane.FirstTime
{
    /// <summary>
    /// Interaction logic for Setup.xaml
    /// </summary>
    public partial class Setup : Window
    {
        private DragonFlyCEF df;
        private UserData ud;
        private MainWindow mw;

        public Setup(DragonFlyCEF df, MainWindow mw)
        {
            this.df = df;
            this.ud = new UserData();
            this.mw = mw;

            InitializeComponent();
            Content = new AddClasses(df, ud, this);
        }

        public void FinishAddClasses()
        {
            Content = new ScanAndAddToCalendar(df, ud, this);
        }

        public void FinishScanClasses()
        {
            Content = new AddToCalendar(df, ud, this);
        }

        public void FinishAll()
        {
            Content = new FinishAll(this);
        }

        public void CloseWin()
        {
            mw.ud = ud;
            mw.PopulateClassDetailList();
            Close();
        }
    }
}
