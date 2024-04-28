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
            get { return $"{currentDate:MMMM yyyy}".ToUpper(); }
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
            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            int startingDayOfWeek = (int)firstDayOfMonth.DayOfWeek; // 0 is Sunday, 1 is Monday, ..., 6 is Saturday

            var grid = new Grid();

            // Add rows for days of the week and for each week in the month
            for (int i = 0; i < 6; i++)
            {
                // Set a fixed height for each row
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) }); // Adjust the height as needed
            }

            // Add columns for each day of the week
            for (int i = 0; i < 7; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Add day names to the grid
            string[] dayNames = { "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY", "SUNDAY" };
            string dayColor = "#4d5566";
            for (int i = 0; i < 7; i++)
            {
                var dayNameTextBlock = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = (Brush)new BrushConverter().ConvertFrom(dayColor),
                    FontSize = 11,
                    Text = dayNames[i],
                    Margin = new Thickness(0, 0, 0, -45) // Adjust the top margin as needed
                };

                Grid.SetColumn(dayNameTextBlock, i);
                Grid.SetRow(dayNameTextBlock, 0);

                grid.Children.Add(dayNameTextBlock);
            }

            // Add empty cells for days before the first day of the month
            int emptyCells = startingDayOfWeek == 0 ? 6 : startingDayOfWeek - 1;
            int currentDay = 1 - emptyCells;

            for (int row = 1; row < 6; row++) // Start from row 1 to leave space for day names
            {
                for (int column = 0; column < 7; column++)
                {
                    if (currentDay < 1 || currentDay > daysInMonth)
                    {
                        // Empty cell for days before the first day of the month or after the last day of the month
                        var emptyBorder = new Border();
                        Grid.SetColumn(emptyBorder, column);
                        Grid.SetRow(emptyBorder, row);
                        grid.Children.Add(emptyBorder);
                    }
                    else
                    {
                        var dateTextBlock = new TextBlock
                        {
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Center,
                            Foreground = Brushes.Black,
                            FontSize = 14,
                            Text = currentDay.ToString("00")
                        };

                        var taskTextBlock = new TextBlock
                        {
                            Text = GetTasksForDate(new DateTime(date.Year, date.Month, currentDay)),
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            Foreground = Brushes.Black,
                            TextWrapping = TextWrapping.Wrap,
                            Margin = new Thickness(5, 0, 0, 0)
                        };

                        var border = new Border
                        {
                            Child = new StackPanel { Orientation = Orientation.Vertical, Children = { dateTextBlock, taskTextBlock } },
                            Style = (Style)FindResource("DayBorderStyle")
                        };

                        Grid.SetColumn(border, column);
                        Grid.SetRow(border, row);

                        grid.Children.Add(border);
                    }

                    currentDay++;
                }
            }

            // Add the grid to the StackPanel
            CalendarStackPanel.Children.Add(grid);
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

        private void Today_Click(object sender, RoutedEventArgs e)
        {
            currentDate = DateTime.Today;
            OnPropertyChanged(nameof(CurrentDate));
            OnPropertyChanged(nameof(CurrentMonthYear));
            DisplayCalendar(currentDate);
        }
    }
}
