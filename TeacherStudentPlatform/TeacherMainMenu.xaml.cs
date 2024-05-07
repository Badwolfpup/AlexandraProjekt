using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    public partial class TeacherMainMenu : UserControl, INotifyPropertyChanged
    {
        private string _currentDay;
        private string _currentDateTime;

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

        public string CurrentDateTime
        {
            get { return _currentDateTime; }
            set
            {
                if (_currentDateTime != value)
                {
                    _currentDateTime = value;
                    OnPropertyChanged("CurrentDateTime");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private System.Timers.Timer timer;
        public TeacherMainMenu()
        {
            InitializeComponent();
            DataContext = this;

            // Create a timer with a one-second interval
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += TimerElapsed;
            timer.Start();

        }

        private void TeacherHomeButton_Click(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);
            TeacherHome student = new TeacherHome();
            parent.Content = student;
        }

        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);
            TeacherCalendar teacherCalendar = new TeacherCalendar();
            parent.Content = teacherCalendar;
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

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Update the CurrentDateTime property with the current date and time
            CurrentDateTime = $"{DateTime.Now:dddd, d MMMM yyyy HH:mm}".ToUpper();
        }

    }
}