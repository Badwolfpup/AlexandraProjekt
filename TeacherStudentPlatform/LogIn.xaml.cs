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

namespace TeacherStudentPlatform
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        private CredentialManager credentialManager;

        public LogIn()
        {
            InitializeComponent();
            // Instantiate CredentialManager only once in the constructor
            credentialManager = new CredentialManager(@"C:\Users\alexandra.f-elev1\source\repos\TeacherStudentPlatform\TeacherStudentPlatform\obj\Admin.txt");
            IDTextbox.Focus();
        }

        public void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            string username = IDTextbox.Text;
            string password = PasswordBox.Password;

            if (credentialManager.ValidateCredentials(username, password))
            {
                if (username.Contains(".utb"))
                {
                    StudentHome loginPage = new StudentHome();
                    MainFrame.NavigationService.Navigate(loginPage);

                    LogInStackpanel.Visibility = Visibility.Hidden;
                    WindowState = WindowState.Maximized;
                }

                else if (username.Contains(".admin"))
                {
                    AdminHome loginPage = new AdminHome();
                    MainFrame.NavigationService.Navigate(loginPage);

                    LogInStackpanel.Visibility = Visibility.Hidden;
                    WindowState = WindowState.Maximized;
                }

                else
                {
                    TeacherHome loginPage = new TeacherHome();
                    MainFrame.NavigationService.Navigate(loginPage);

                    LogInStackpanel.Visibility = Visibility.Hidden;
                    WindowState = WindowState.Maximized;
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }

        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LogInButton_Click(sender, e);
            }
        }
    }
}
