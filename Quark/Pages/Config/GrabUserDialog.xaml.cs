using System.Collections.Generic;
using System.Windows;

namespace Quark.Classes.Util.Config
{
    public partial class GrabUserDialog : Window
    {
        public GrabUserDialog(List<string> users)
        {
            InitializeComponent();

            userComboBox.ItemsSource = users;
            userComboBox.SelectedIndex = 0;
        }

        public string SelectedUser { get; private set; }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            SelectedUser = userComboBox.SelectedItem as string;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}