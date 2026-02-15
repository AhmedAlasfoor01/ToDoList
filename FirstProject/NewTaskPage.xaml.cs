using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace FirstProject
{
    public partial class NewTaskPage :UserControl // Changed from Page to Window
    {
        private int taskCounter = 5; 
        private ObservableCollection<TaskItem> _taskList;
        public NewTaskPage() : this(null)//because I have to clarify that I want to call the constructor with the task list parameter, and I want to pass null to it so it will create a new empty list
        {

        }

        public NewTaskWindow NewTaskWindow { get; }

       

        public NewTaskPage(ObservableCollection<TaskItem> taskList = null)
        {
            InitializeComponent();
            _taskList = taskList ?? new ObservableCollection<TaskItem>();

            // Set placeholder text for initial tasks
           //SetPlaceholder(txtTask1);
            //SetPlaceholder(txtTask2);
            //SetPlaceholder(txtTask3);
            //SetPlaceholder(txtTask4);
        }

        public NewTaskPage(ObservableCollection<TaskItem> taskList = null, NewTaskWindow newTaskWindow = null) : this(taskList)
        {
            NewTaskWindow = newTaskWindow;
        }

        // Public method to get all tasks from the TaskPanel
        public List<TaskItem> GetAllTasks()
        {
            var tasks = new List<TaskItem>();

            foreach (UIElement element in TaskPanel.Children)
            {
                if (element is Border border && border.Child is Grid grid)
                {
                    TextBox textBox = null;
                    CheckBox checkBox = null;

                    // Find TextBox and CheckBox in the grid
                    foreach (UIElement child in grid.Children)
                    {
                        if (child is TextBox tb)
                            textBox = tb;
                        else if (child is CheckBox cb)
                            checkBox = cb;
                    }

                    if (textBox == null || checkBox == null)
                        continue;

                    string taskText = textBox.Text;

                    // Skip if empty or placeholder
                    if (string.IsNullOrWhiteSpace(taskText) ||
                        taskText == textBox.Tag?.ToString())
                        continue;

                    // Create task item
                    var task = new TaskItem
                    {
                        TaskName = taskText,
                        TaskText = "",
                        IsDone = checkBox.IsChecked == true
                    };
                    tasks.Add(task);
                }
            }

            return tasks;
        }

        // Placeholder text helpers
        private void SetPlaceholder(TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Tag?.ToString() ?? "Enter your task";
                textBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void txtTask1_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox?.Text == textBox?.Tag?.ToString())
            {
                textBox.Text = "";
                textBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void txtTask1_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox?.Text))
            {
                SetPlaceholder(textBox);
            }
        }

        // Add New Task
        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            // Create new task border
            Border taskBorder = new Border
            {
                BorderBrush = System.Windows.Media.Brushes.LightGray,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(5),
                Background = System.Windows.Media.Brushes.White,
                Padding = new Thickness(10),
                Margin = new Thickness(5,0,0,0)
            };

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Label
            Label label = new Label
            {
                Content = $"Task {taskCounter}:",
                Width = 60,
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.SemiBold
            };
            Grid.SetColumn(label, 0);

            // TextBox
            TextBox textBox = new TextBox
            {
                Name = $"txtTask{taskCounter}",
                MinHeight = 30,
                Margin = new Thickness(5, 0, 0, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = new Thickness(5),
                Foreground = System.Windows.Media.Brushes.Gray,
                Tag = "Enter your task"
            };
            textBox.GotFocus += txtTask1_GotFocus;
            textBox.LostFocus += txtTask1_LostFocus;
            SetPlaceholder(textBox);
            Grid.SetColumn(textBox, 1);

            // CheckBox
            CheckBox checkBox = new CheckBox
            {
                Name = $"chkTask{taskCounter}",
                Content = "Done",
                Margin = new Thickness(10, 0,0,0),
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(checkBox, 2);

            // Delete Button
            Button deleteButton = new Button
            {
                Content = "✕",
                Width = 30,
                Height = 30,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Background = System.Windows.Media.Brushes.Red,
                Foreground = System.Windows.Media.Brushes.White,
                BorderThickness = new Thickness(0)
            };
            deleteButton.Click += (s, args) => TaskPanel.Children.Remove(taskBorder);
            Grid.SetColumn(deleteButton, 3);

            // Add all to grid
            grid.Children.Add(label);
            grid.Children.Add(textBox);
            grid.Children.Add(checkBox);
            grid.Children.Add(deleteButton);

            taskBorder.Child = grid;
            TaskPanel.Children.Add(taskBorder);
            taskCounter++;

            MessageBox.Show($"Task {taskCounter - 1} added successfully!",
                          "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Delete individual tasks
        private void BtnDeleteTask1_Click(object sender, RoutedEventArgs e)
        {
            DeleteTaskAtIndex(0);
        }

        private void BtnDeleteTask2_Click(object sender, RoutedEventArgs e)
        {
            DeleteTaskAtIndex(1);
        }

        private void BtnDeleteTask3_Click(object sender, RoutedEventArgs e)
        {
            DeleteTaskAtIndex(2);
        }

        private void BtnDeleteTask4_Click(object sender, RoutedEventArgs e)
        {
            DeleteTaskAtIndex(3);
        }

        private void DeleteTaskAtIndex(int index)
        {
            if (index >= 0 && index < TaskPanel.Children.Count)
            {
                TaskPanel.Children.RemoveAt(index);
                MessageBox.Show("Task deleted successfully!",
                              "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Save All Tasks - FIXED!
        private void BtnSaveAll_Click(object sender, RoutedEventArgs e)
        {
            var allTasks = GetAllTasks();
            var completedTasks = allTasks.Where(t => t.IsDone).ToList();
            var incompleteTasks = allTasks.Where(t => !t.IsDone).ToList();

            // Add completed tasks to the shared list
            foreach (var task in completedTasks)
            {
                _taskList.Add(task);
            }

            // Show incomplete tasks warning
            if (incompleteTasks.Count > 0)
            {
                string message = "The following tasks are not done yet:\n\n" +
                               string.Join("\n", incompleteTasks.Select(t => t.TaskName));
                MessageBox.Show(message, "Incomplete Tasks",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (completedTasks.Count > 0)
            {
                MessageBox.Show($"{completedTasks.Count} task(s) saved successfully!",
                              "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Navigate back to MainWindow
                Window parentWindow = Window.GetWindow(this);
                  // Check the Window.Content (object) for a `MainWindow` instance.
                if (parentWindow?.Content is MainWindow mainWindow)
                {
                    mainWindow.ContenArea.Children.Clear();
                    
                }
            }
            else
            {
                MessageBox.Show("No completed tasks to save!", "Info",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Clear All
        private void BtnClearAll_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to clear all tasks?",
                "Confirm Clear All",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                foreach (UIElement element in TaskPanel.Children)
                {
                    if (element is Border border && border.Child is Grid grid)
                    {
                        foreach (UIElement child in grid.Children)
                        {
                            if (child is TextBox textBox)
                            {
                                textBox.Text = "";
                                SetPlaceholder(textBox);
                            }
                            else if (child is CheckBox checkBox)
                            {
                                checkBox.IsChecked = false;
                            }
                        }
                    }
                }

                MessageBox.Show("All tasks cleared!", "Success",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}