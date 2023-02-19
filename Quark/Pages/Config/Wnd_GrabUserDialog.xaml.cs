using System.Collections.Generic;
using System.Windows;

namespace Quark.AppConfig.Util.Config
{
    public partial class GrabUserDialog : Window
    {
        public GrabUserDialog(List<string> users)
        {
            InitializeComponent();

            UserComboBox.ItemsSource = users;
            UserComboBox.SelectedIndex = 0;
        }

        public string SelectedUser { get; private set; }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            SelectedUser = UserComboBox.SelectedItem as string;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}