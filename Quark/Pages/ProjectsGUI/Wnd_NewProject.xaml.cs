using System;
using System.IO;
using System.Windows;

namespace Quark.Pages.ProjectsGUI
{
    public partial class Wnd_NewProject : Window
    {
        public Wnd_NewProject()
        {
            InitializeComponent();
        }

        private void BtnCreateProject_OnClick(object sender, RoutedEventArgs e)
        {
            var name = TxtProjectName.Text;
            var directory = TxtProjectDirectory.Text;
            // ensure that the project name is empty
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Project name cannot be empty!");
                return;
            }

            name = name.Replace(" ", "_");
            if (name.Contains("/") || name.Contains("\\") || name.Contains(":") || name.Contains("*") ||
                name.Contains("?") || name.Contains("\"") || name.Contains("<") || name.Contains(">") ||
                name.Contains("|"))
            {
                MessageBox.Show("Project name cannot contain any of the following characters: / \\ : * ? \" < > |");
                return;
            }

            // if directory is not empty, ensure that it is a valid directory, otherwise, use the default directory
            if (!string.IsNullOrEmpty(directory))
            {
                if (!Directory.Exists(directory))
                {
                    MessageBox.Show("Directory does not exist!");
                    return;
                }
            }
            else
            {
                directory = Path.Combine(QMain.qConfig.GetInstance().GetValue<string>("ProjectsRoot"), name);
            }

            var eval = QMain.projects.Instance.NewProject(name, directory);
            if (eval == null)
            {
                MessageBox.Show("Project already exists!");
                return;
            }

            var Wnd_ChooseProject = QMain.GetWindow("Quark | Choose Project");
            if (Wnd_ChooseProject != null)
                ((Wnd_ChooseProject)Wnd_ChooseProject).LoadProjects();
            else throw new Exception("Wnd_ChooseProject is null!");
            MessageBox.Show("Project created!");
            Close();
        }
    }
}