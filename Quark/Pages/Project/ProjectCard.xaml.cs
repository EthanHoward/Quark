using System;
using System.Runtime.Remoting.Channels;
using System.Windows.Controls;
using Quark.Classes.FileManagement;

namespace Quark.Pages
{
    public partial class ProjectCard : UserControl
    {
        private readonly Project _project;

        public ProjectCard(Project project)
        {
            InitializeComponent();
            _project = project;
            ProjectName.Text = project.GetValue<string>("Name");
            ProjectDirectory.Text = project.GetValue<string>("Directory");
            MouseLeftButtonUp += (sender, e) => OnClick();
        }
        private void OnClick()
        {
            // why is this static??
           Projects.OpenProject(_project);
        }
    }
}