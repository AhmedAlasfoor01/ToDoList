using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls; // Add this using directive

namespace FirstProject
{
    public partial class NewTaskWindow : Window
    {
        public ObservableCollection<TaskItem> TaskList { get; set; }
        public Frame ContentFrame { get; } // Change type from object to Frame

        public NewTaskWindow()
        {
            InitializeComponent();

            // Initialize the task list
            TaskList = new ObservableCollection<TaskItem>();

            // Navigate to NewTaskPage
            NewTaskPage taskPage = new NewTaskPage(TaskList, this);
            ContentFrame.Navigate(taskPage);
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }
    }
}