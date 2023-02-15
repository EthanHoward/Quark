namespace Quark.Pages
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ChooseProject
    {
        public ChooseProject()
        {
            InitializeComponent();
            LoadProjects();
        }

        private void LoadProjects()
        {
            var projects = QMain.projects.Instance.GetProjects();

            foreach (var project in projects)
            {
                var card = new ProjectCard(project);
                AllProjects.Children.Add(card);
            }
        }
    }
}