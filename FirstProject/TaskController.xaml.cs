using System;
using System.Windows;
using System.Windows.Controls;

namespace FirstProject
{
    public partial class TaskControl : UserControl
    {
        // Event that fires when delete button is clicked
        public event EventHandler DeleteRequested;

        public TaskControl()
        {
            InitializeComponent();
        }

        // Properties to access the controls
        public int TaskNumber
        {
            get => int.Parse(lblTaskNumber.Content.ToString().Replace("Task ", "").Replace(":", ""));
            set => lblTaskNumber.Content = $"Task {value}:";
        }

        public string TaskText
        {
            get => txtTask.Text;//it tagets the txt box 
            set => txtTask.Text = value;
        }

        public bool IsDone
        {
            get => chkDone.IsChecked == true;// it targets the chk box and checks if it is checked or not
            set => chkDone.IsChecked = value;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Ask for confirmation
            var result = MessageBox.Show(
                $"Are you sure you want to delete '{TaskText}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Raise the event so parent can handle deletion
                DeleteRequested?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
