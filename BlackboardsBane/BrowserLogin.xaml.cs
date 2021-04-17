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

namespace BlackboardsBane
{
    /// <summary>
    /// Interaction logic for BrowserLogin.xaml
    /// </summary>
    public partial class BrowserLogin : Window
    {
        public BrowserLogin()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            Browser.Address = "https://learn.uark.edu/auth-saml/saml/login?apId=_156_1&redirectUrl=https%3A%2F%2Flearn.uark.edu%2Fwebapps%2Fportal%2Fexecute%2FdefaultTab";
            Browser.AddressChanged += Browser_AddressChanged;
        }

        private void Browser_AddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Browser.Address.StartsWith("https://learn.uark.edu/webapps/portal/execute"))
            {
                DialogResult = true;
                Close();
            }
        }
    }
}
