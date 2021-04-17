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
    /// Interaction logic for PlsLogin.xaml
    /// </summary>
    public partial class ZoomCollaborate : Window
    {
        //root client
        public DragonFlyCEF df;
        public bool loadedYet = false;

        public ZoomCollaborate()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
