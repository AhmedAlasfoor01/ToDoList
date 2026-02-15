using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace FirstProject
{
    public partial class MainWindow : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<TaskItem> _taskList;

        public ObservableCollection<TaskItem> TaskList
        {
            get { return _taskList; }
            set
            {
                _taskList = value;
                OnPropertyChanged(nameof(TaskList));
                UpdateNoTasksMessage();
            }
        }

        public Panel ContenArea { get; private set; }

        public void InitializeComponent()
        {
        }

        public MainWindow()
        {
            InitializeComponent();
            // Assign ContenArea to the named panel from XAML
            ContenArea = this.FindName("ContenArea") as Panel;//this function gonna let us access the content area from the code behind
            InitializeTasks();
            DataContext = this;
           



        }

        // Add this constructor to accept an ObservableCollection<TaskItem>
        public MainWindow(ObservableCollection<TaskItem> taskList)
        {
            InitializeComponent();
            _taskList = taskList;
            TaskList = taskList;
            // Optionally, update UI or perform other initialization as needed
        }

        private void InitializeTasks()
        {
            TaskList = new ObservableCollection<TaskItem>
            {
                new TaskItem { TaskName = "Sample Task", TaskText = "This is a sample task", IsDone = false }
            };

            // Listen for collection changes
            TaskList.CollectionChanged += (s, e) => UpdateNoTasksMessage();
        }

        // Add New Task Button - Opens the Add Task Page
        // Add New Task Button - Opens the Add Task Page
        private void BtnAddNewTask_Click(object sender, RoutedEventArgs e)
        {
            // Clear the content area first
            ContenArea.Children.Clear();

            // Add the NewTaskPage to the content area
            NewTaskPage addTaskPage = new NewTaskPage(_taskList);
            ContenArea.Children.Add(addTaskPage); // ✅ No .Show() needed
        }

        // Delete Individual Task (X button)
        private void BtnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.Tag is TaskItem taskToDelete)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete '{taskToDelete.TaskName}'?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    TaskList.Remove(taskToDelete);
                    MessageBox.Show("Task deleted successfully!", "Success",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        // Clear All Tasks
        private void BtnClearAll_Click(object sender, RoutedEventArgs e)
        {
            if (TaskList.Count == 0)
            {
                MessageBox.Show("No tasks to clear!", "Info",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete all {TaskList.Count} tasks?",
                "Confirm Clear All",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                TaskList.Clear();
                MessageBox.Show("All tasks cleared!", "Success",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Update "No tasks" message visibility
        private void UpdateNoTasksMessage()
        {
            //if (txtNoTasks != null)
            //{
            //    txtNoTasks.Visibility = TaskList.Count == 0 ?
            //        Visibility.Visible : Visibility.Collapsed;
            //}
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