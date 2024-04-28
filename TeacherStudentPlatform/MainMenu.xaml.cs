using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace TeacherStudentPlatform
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl, INotifyPropertyChanged
    {
        private string _currentDay;

    public string CurrentDay
    {
        get { return _currentDay; }
        set
        {
            if (_currentDay != value)
            {
                _currentDay = value;
                OnPropertyChanged("CurrentDay");
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
        public MainMenu()
        {
            InitializeComponent();
            CurrentDay = DateTime.Now.DayOfWeek.ToString().ToUpper();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);
            StudentHome student = new StudentHome();
            parent.Content = student;
        }

        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);
            Calendar calendar = new Calendar();
            parent.Content = calendar;

        }

        private void LearningHubButton_Click(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);
            LearningPlatform learningPlatform = new LearningPlatform();
            parent.Content = learningPlatform;
        }

        private void NewsButton_Click(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);
            News news = new News();
            parent.Content = news;
        }

        private void ChatButton_Click(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);
            Chat chat = new Chat();
            parent.Content = chat;
        }

        private void NotificationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);

            LogIn login = new LogIn();
            login.Show();

            parentWindow.Close();
        }

    }
}
