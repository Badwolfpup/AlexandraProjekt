using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// Interaction logic for TeacherHome.xaml
    /// </summary>
    public partial class TeacherHome : Page, INotifyPropertyChanged
    {
        private DateTime _currentDateTime;
        public DateTime CurrentDateTime
        {
            get { return _currentDateTime; }
            set
            {
                if (_currentDateTime != value)
                {
                    _currentDateTime = value;
                    OnPropertyChanged(nameof(CurrentDateTime));
                }
            }
        }
        public TeacherHome()
        {
            InitializeComponent();
            DataContext = this;
            CurrentDateTime = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
    public class TeacherTimeToGreetingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime currentTime = (DateTime)value;
            int hour = currentTime.Hour;

            if (hour >= 4 && hour < 12)
            {
                return "God morgon, lärare";
            }
            else if (hour >= 12 && hour < 18)
            {
                return "God eftermiddag, lärare";
            }
            else if (hour >= 18 && hour < 24)
            {
                return "God kväll, lärare";
            }
            else
            {
                return "God natt, lärare";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
