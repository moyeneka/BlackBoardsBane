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
    /// Interaction logic for FinishAll.xaml
    /// </summary>
    public partial class FinishAll : Page
    {
        private Setup setup;
        public FinishAll(Setup setup)
        {
            this.setup = setup;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            setup.CloseWin();
        }
    }
}
