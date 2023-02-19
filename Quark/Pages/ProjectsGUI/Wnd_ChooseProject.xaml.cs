using System.Text;
using System.Windows;
using Quark.FileManagement.Projects;
using Quark.Pages.ProjectsGUI;
using Quark.Util.Logging;

namespace Quark.Pages
{
    /// <summary>
    ///     Interaction logic for Wnd_ChooseProject.xaml
    /// </summary>
    public partial class Wnd_ChooseProject
    {
        public Wnd_ChooseProject()
        {
            InitializeComponent();
            LoadProjects();
        }

        public void LoadProjects()
        {
            foreach (var project in QMain.projects.GetProjects())
            {
                // prevent null errors
                if (project == null) continue;
                Logger.Instance.Debug($"Loading projects... {QMain.projects.Instance.GetProjects()}");
                AllProjects.Children.Add(new ProjectCard(project));
            }

            var SB = new StringBuilder();
            foreach (var proj in QMain.projects.Instance.GetProjects()) SB.Append(proj.GetValue<string>("Name"));
            if (AllProjects.Children.Count != 0) return;
            Logger.Instance.Debug("No projects found, showing placeholder...");
            AllProjects.Children.Add(new ProjectCard(new Project("No projects found", "", false)));
        }

        public void OnCloseClick(object sender, RoutedEventArgs e)
        {
            Logger.Instance.Debug("Closing application...");
            Application.Current.Shutdown();
        }

        private void OnNewProjectClick(object sender, RoutedEventArgs e)
        {
            var newProject = new Wnd_NewProject();
            newProject.Show();
            Logger.Instance.Debug("Opening new project window... None yet");
        }
    }
}