using Newtonsoft.Json;
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
using System.IO;

namespace TeacherStudentPlatform
{
    /// <summary>
    /// Interaction logic for TeacherCalendar.xaml
    /// </summary>
    /// 
    public class TaskAddedEventArgs : EventArgs
    {
        public DateTime Date { get; }
        public string Task { get; }

        public TaskAddedEventArgs(DateTime date, string task)
        {
            Date = date;
            Task = task;
        }
    }

    public static class TaskManager
    {
        public static List<string> Tasks { get; private set; } = new List<string>();

        public static void AddTask(DateTime date, string task)
        {
            string taskDescription = $"{date.ToShortDateString()}: {task}";
            Tasks.Add(taskDescription);
        }

        public static void SaveTasksToJson(string filePath)
        {
            string json = JsonConvert.SerializeObject(Tasks);
            File.WriteAllText(filePath, json);
        }

        public static void LoadTasksFromJson(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                Tasks = JsonConvert.DeserializeObject<List<string>>(json);
            }
        }
    }
    public partial class TeacherCalendar : Page
    {
        private const string FilePath = "CalendarTasks.json";

        private DateTime currentDate;
        private Dictionary<DateTime, List<string>> tasksByDate;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<TaskAddedEventArgs> TaskAdded;

        public static List<string> Tasks { get; private set; } = new List<string>();

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

        public TeacherCalendar()
        {
            InitializeComponent();
            currentDate = DateTime.Today;
            tasksByDate = new Dictionary<DateTime, List<string>>();
            DisplayCalendar(currentDate);

            // Load tasks from JSON file when the application starts
            LoadTasks();

            // Attach event handler for window closing event
            Loaded += TeacherCalendar_Loaded;
        }

        private void TeacherCalendar_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Closing += ParentWindow_Closing;
            }
        }

        private void ParentWindow_Closing(object sender, CancelEventArgs e)
        {
            // Save tasks to JSON file when the application closes
            SaveTasks();
        }

        private void LoadTasks()
        {
            TaskManager.LoadTasksFromJson(FilePath);
            // Reload the calendar to display the loaded tasks
            DisplayCalendar(currentDate);
        }

        private void SaveTasks()
        {
            TaskManager.SaveTasksToJson(FilePath);
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
            string[] dayNames = { "MÅNDAG", "TISDAG", "ONSDAG", "TORSDAG", "FREDAG", "LÖRDAG", "SÖNDAG" };
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
                            FontSize = 13,
                            Text = currentDay.ToString("00")
                        };

                        var taskTextBlock = new TextBlock
                        {
                            Text = GetTasksForDate(new DateTime(date.Year, date.Month, currentDay)),
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            Foreground = Brushes.Black,
                            FontSize = 11,
                            TextWrapping = TextWrapping.Wrap,
                            Margin = new Thickness(1, 0, 0, 0)
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
            string formattedDate = date.ToShortDateString();
            List<string> tasks = TaskManager.Tasks.Where(t => t.StartsWith(formattedDate)).ToList();

            // Remove the formatted date from each task description
            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i] = tasks[i].Replace(formattedDate + ": ", ""); // Replace the formatted date with an empty string
            }

            return string.Join("\n", tasks);
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

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = TaskDatePicker.SelectedDate ?? DateTime.Today;
            string task = TaskDescriptionTextBox.Text;

            TaskManager.AddTask(date, task);
            DisplayCalendar(currentDate);
        }
        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Today_Click(object sender, RoutedEventArgs e)
        {
            currentDate = DateTime.Today;
            OnPropertyChanged(nameof(CurrentDate));
            OnPropertyChanged(nameof(CurrentMonthYear));
            DisplayCalendar(currentDate);
        }

        private void AddTaskWindow_Click(object sender, RoutedEventArgs e)
        {
            AddTaskWindowStackPanel.Visibility = Visibility.Visible;
        }
    }
}