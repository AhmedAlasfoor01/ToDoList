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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace FirstProject
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<TaskItem> _taskList;

        public ObservableCollection<TaskItem> TaskList
        {
            get { return _taskList; }
            set
            {
                _taskList = value;
                OnPropertyChanged(nameof(TaskList));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeTasks();
            DataContext = this; // Set DataContext for binding
        }

    

        private void InitializeTasks()
        {
            TaskList = new ObservableCollection<TaskItem>
            {
                new TaskItem { TaskName = "Task1", TaskText = "Enter task 1", IsDone = false },
                new TaskItem { TaskName = "Task2", TaskText = "Enter task 2", IsDone = false },
                new TaskItem { TaskName = "Task3", TaskText = "Enter task 3", IsDone = false },
                new TaskItem { TaskName = "Task4", TaskText = "Enter task 4", IsDone = false }
            };

            this.TaskItemsControl.ItemsSource = TaskList;
        }

        // EDIT Button
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Edit mode enabled! Modify your tasks.", "Edit");
        }

        // ADD Button - Add new task
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            int nextTaskNumber = TaskList.Count + 1;

            TaskList.Add(new TaskItem
            {
                TaskName = $"Task{nextTaskNumber}",
                TaskText = $"Enter task {nextTaskNumber}",
                IsDone = false
            });

            MessageBox.Show($"Task{nextTaskNumber} added successfully!", "Success");
        }

        // DELETE Button - Clear all tasks
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to clear all tasks?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                foreach (var task in TaskList)
                {
                    task.TaskText = "";
                    task.IsDone = false;
                }
                MessageBox.Show("All tasks cleared!", "Success");
            }
        }

        // SAVE Button - Show current tasks
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string taskSummary = "Current Tasks:\n\n";

            foreach (var task in TaskList)
            {
                string status = task.IsDone ? "✓ Done" : "○ Pending";
                taskSummary += $"{task.TaskName}: {task.TaskText} [{status}]\n";
            }

            MessageBox.Show(taskSummary, "Task Summary");
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Task Model Class
    public class TaskItem : INotifyPropertyChanged
    {
        private string _taskName;
        private string _taskText;
        private bool _isDone;

        public string TaskName
        {
            get { return _taskName; }
            set
            {
                _taskName = value;
                OnPropertyChanged(nameof(TaskName));
            }
        }

        public string TaskText
        {
            get { return _taskText; }
            set
            {
                _taskText = value;
                OnPropertyChanged(nameof(TaskText));
            }
        }

        public bool IsDone
        {
            get { return _isDone; }
            set
            {
                _isDone = value;
                OnPropertyChanged(nameof(IsDone));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
