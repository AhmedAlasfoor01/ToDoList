using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.ObjectModel;


namespace FirstProject
{
    public partial class NewTaskPage : Window
    {
        private int taskCounter = 5;
        private ObservableCollection<TaskItem> _taskList;
        private Window _parentWindow;

        // Constructor that receives TaskList and parent window
        public NewTaskPage(ObservableCollection<TaskItem> taskList, Window parentWindow)
        {
            InitializeComponent();
            _taskList = taskList ?? new ObservableCollection<TaskItem>();// Ensure we have a valid task list
            _parentWindow = parentWindow;
        }

        public NewTaskPage()
        {
            InitializeComponent();
            _taskList = new ObservableCollection<TaskItem>();
        }

        
        private void BtnDeleteTask1_Click(object sender, RoutedEventArgs e)
        {
            DeleteTask(txtTask1, chkTask1, sender as Button);
        }

        // Delete Task 2
        private void BtnDeleteTask2_Click(object sender, RoutedEventArgs e)
        {
            DeleteTask(txtTask2, chkTask2, sender as Button);//delete function 
        }

        private void BtnDeleteTask3_Click(object sender, RoutedEventArgs e)
        {
            DeleteTask(txtTask3, chkTask3, sender as Button);
        }

        
        private void BtnDeleteTask4_Click(object sender, RoutedEventArgs e)
        {
            DeleteTask(txtTask4, chkTask4, sender as Button);
        }

        // Helper method to delete a task
        private void DeleteTask(TextBox textBox, CheckBox checkBox, Button deleteButton)
        {
            var result = MessageBox.Show(
                $"Are you sure you want to delete '{textBox.Text}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Border parentBorder = FindParent<Border>(deleteButton);
                if (parentBorder != null)
                {
                    TaskPanel.Children.Remove(parentBorder);
                    MessageBox.Show("Task deleted successfully!", "Success",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        // Add New Task Button
        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            Border newTaskBorder = new Border//so when the user clicks new task it's gonna create  anew task with the same design as the previous ones and add it to the task panel
            {
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(5),
                Background = Brushes.White,
                Padding = new Thickness(10),
                Margin = new Thickness(0, 5, 0, 0)
            };

            Grid taskGrid = new Grid();
            taskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            taskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            taskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            taskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            Label label = new Label
            {
                Content = $"Task {taskCounter}:",
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.SemiBold,
                Width = 60
            };
            Grid.SetColumn(label, 0);

            TextBox textBox = new TextBox
            {
                Text = $"New task {taskCounter}",
                MinHeight = 30,
                Margin = new Thickness(5, 0, 5, 0),
                VerticalAlignment = VerticalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = new Thickness(5)
            };
            Grid.SetColumn(textBox, 1);

            CheckBox checkBox = new CheckBox
            {
                Content = "Done",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 10, 0)
            };
            Grid.SetColumn(checkBox, 2);

            Button deleteButton = new Button
            {
                Content = "✕",
                Width = 30,
                Height = 30,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Background = Brushes.Red,
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                Cursor = System.Windows.Input.Cursors.Hand,
                ToolTip = "Delete Task"
            };
            Grid.SetColumn(deleteButton, 3);

            deleteButton.Click += (s, args) =>
            {
                DeleteTask(textBox, checkBox, deleteButton);
            };

            taskGrid.Children.Add(label);
            taskGrid.Children.Add(textBox);
            taskGrid.Children.Add(checkBox);
            taskGrid.Children.Add(deleteButton);

            newTaskBorder.Child = taskGrid;
            TaskPanel.Children.Add(newTaskBorder);

            taskCounter++;

            MessageBox.Show($"Task {taskCounter - 1} added successfully!", "Success",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }

        
        // Save All Tasks - Transfer to MainWindow and Navigate
        private void BtnSaveAll_Click(object sender, RoutedEventArgs e)//i had an error so it gave me error in the sender but i fixed it by changing it and  worked fine
        {
            int savedCount = 0;

            // Loop through all tasks in the TaskPanel
            foreach (UIElement element in TaskPanel.Children)
            {
                if (element is Border border && border.Child is Grid grid)
                {
                    TextBox textBox = null;
                    CheckBox checkBox = null;

                    // Find the TextBox and CheckBox in the grid
                    foreach (UIElement child in grid.Children)
                    {
                        if (child is TextBox tb)
                            textBox = tb;
                        if (child is CheckBox cb)
                            checkBox = cb;
                    }

                   
                    if (textBox != null && !string.IsNullOrWhiteSpace(textBox.Text))// to warn the user if it's done or yet and complete the task without stopping the process of saving the other tasks if there are any incomplete tasks
                    {
                        
                        if (checkBox?.IsChecked != true)
                        {
                            MessageBox.Show($"Task '{textBox.Text}' is not done yet!",
                                           "Incomplete Task",
                                           MessageBoxButton.OK,
                                           MessageBoxImage.Warning);
                            continue; 
                        }

                        var newTask = new TaskItem
                        {
                            TaskName = textBox.Text,
                            TaskText = "",
                            IsDone = true 
                        };
                        _taskList.Add(newTask);
                        savedCount++;
                    }
                }
            }

            if (savedCount > 0)
            {
                MessageBox.Show($"{savedCount} task(s) saved successfully! Opening Main Window...",
                              "Success",
                              MessageBoxButton.OK,
                              MessageBoxImage.Information);

                //  show MainWindow with the TaskList in the to do list 
                MainWindow mainWindow = new MainWindow(_taskList);
                mainWindow.Show();

                // Close the current window (NewTaskWindow)
                _parentWindow.Close();
            }
            else
            {
                MessageBox.Show("No tasks to save! Please add at least one task.",
                              "Info",
                              MessageBoxButton.OK,
                              MessageBoxImage.Information);
            }
        }
        // Clear All Tasks
        private void BtnClearAll_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to clear all tasks?",
                "Confirm Clear All",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                foreach (Border border in TaskPanel.Children)
                {
                    if (border.Child is Grid grid)
                    {
                        foreach (var child in grid.Children)
                        {
                            if (child is TextBox textBox)
                            {
                                textBox.Clear();
                            }
                            if (child is CheckBox checkBox)
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

        // Helper method to find parent element
        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }
         public void OpenMainWindow()
        { //this fucntion should open the main window after the user clicks save all and it should pass the task list to the main window so it can show the tasks in the to do list
            MainWindow mainWindow = new MainWindow(_taskList);
            mainWindow.Show();
            
        }

        private void txtTask1_GotFocus(object sender, RoutedEventArgs e) { }
        private void txtTask1_LostFocus(object sender, RoutedEventArgs e) { }
    }
}