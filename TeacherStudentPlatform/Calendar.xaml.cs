using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace TeacherStudentPlatform
{
    /// <summary>
    /// Interaction logic for Calendar.xaml
    /// </summary>
    public partial class Calendar : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private DateTime currentDate;
        private Dictionary<DateTime, List<string>> tasksByDate;

        public DateTime CurrentDate
        {
            get { return currentDate; }
            set
            {
                currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
                OnPropertyChanged(nameof(CurrentMonthYear));
            }
        }

        public string CurrentMonthYear
        {
            get { return $"{currentDate:MMMM yyyy}"; }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Calendar()
        {
            InitializeComponent();
            currentDate = DateTime.Today;
            tasksByDate = new Dictionary<DateTime, List<string>>();
            DisplayCalendar(currentDate);
        }

        public void DisplayCalendar(DateTime date)
        {
            CalendarStackPanel.Children.Clear();

            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime currentDate = new DateTime(date.Year, date.Month, day);

                var dateTextBlock = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.Black,
                    Width = 30
                };

                dateTextBlock.Inlines.Add(new Run(day.ToString("00"))
                {
                    FontSize = 16
                });

                dateTextBlock.Inlines.Add(new Run("\n" + currentDate.ToString("ddd")));

                var taskTextBlock = new TextBlock
                {
                    Text = GetTasksForDate(currentDate),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.Black,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(30, 0, 0, 0)
                };

                var grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                grid.Children.Add(dateTextBlock);
                grid.Children.Add(taskTextBlock);
                Grid.SetColumn(taskTextBlock, 1);

                var border = new Border
                {
                    Child = grid,
                    Style = (Style)FindResource("DayBorderStyle")
                };
                CalendarStackPanel.Children.Add(border);
            }
        }

        private string GetTasksForDate(DateTime date)
        {
            if (tasksByDate.ContainsKey(date))
            {
                return string.Join("\n", tasksByDate[date]);
            }
            else
            {
                return "";
            }
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            currentDate = currentDate.AddMonths(1);
            OnPropertyChanged(nameof(CurrentDate)); // Notify that CurrentDate has changed
            OnPropertyChanged(nameof(CurrentMonthYear)); // Notify that CurrentMonthYear has changed
            DisplayCalendar(currentDate);
        }

        private void PrevMonth_Click(object sender, RoutedEventArgs e)
        {
            currentDate = currentDate.AddMonths(-1);
            OnPropertyChanged(nameof(CurrentDate)); // Notify that CurrentDate has changed
            OnPropertyChanged(nameof(CurrentMonthYear)); // Notify that CurrentMonthYear has changed
            DisplayCalendar(currentDate);
        }

        private void AddTaskWindow_Click(object sender, RoutedEventArgs e)
        {
            AddTaskWindow.Visibility = Visibility.Visible;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the task description from the TextBox
            string taskDescription = TaskDescriptionTextBox.Text;

            // Get the selected date from the DatePicker
            DateTime selectedDate = TaskDatePicker.SelectedDate ?? DateTime.Today;

            // Add the task to the calendar
            if (!tasksByDate.ContainsKey(selectedDate))
            {
                tasksByDate[selectedDate] = new List<string>();
            }
            tasksByDate[selectedDate].Add(taskDescription);

            // Update the calendar display
            DisplayCalendar(currentDate);

            // Hide the AddTaskWindow after adding the task
            AddTaskWindow.Visibility = Visibility.Hidden;
        }
    }
}
