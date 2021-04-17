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
    /// Interaction logic for PlsLogin.xaml
    /// </summary>
    public partial class PlsLogin : Window
    {
        //root client
        public DragonFlyCEF df;
        public bool loadedYet = false;

        public PlsLogin()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            df = new DragonFlyCEF();
            df.BrowserLoaded += (object s) =>
            {
                df.Connect("https://learn.uark.edu/auth-saml/saml/login?apId=_156_1&redirectUrl=https%3A%2F%2Flearn.uark.edu%2Fwebapps%2Fportal%2Fexecute%2FdefaultTab");
            };
            df.DOMLoaded += DomLoaded;
        }

        private void DomLoaded(object sender)
        {
            if (!loadedYet)
            {
                loadedYet = true;
                CheckForLogin();
            }
        }

        private async void CheckForLogin()
        {
            var loggedIn = false;
            if (!loggedIn)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    signingIn.Visibility = Visibility.Hidden;
                    signInPls.Visibility = Visibility.Visible;
                    signInBtn.IsEnabled = true;
                    cancelBtn.IsEnabled = true;
                });
            }
            else
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    MainWindow blackboard = new MainWindow(df);
                    blackboard.Show();
                    Close();
                });
            }
        }

        private void signInBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                BrowserLogin blogin = new BrowserLogin();
                bool? success = blogin.ShowDialog();
                if (success != null && success.Value)
                {
                    MessageBox.Show("login successful!");
                    MainWindow blackboard = new MainWindow(df);
                    blackboard.Show();
                    Close();
                }
                else
                {
                    Close();
                }
            });
        }
    }
}
