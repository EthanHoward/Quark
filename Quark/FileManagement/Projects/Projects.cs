using System.Collections.Generic;
using System.IO;
using Quark.Classes.Util.Logging;

namespace Quark.Classes.FileManagement
{
    public class Projects
    {
        private readonly List<Project> _Projects;
        public readonly Projects Instance;
        
        public Projects()
        {
            _Projects = new List<Project>();
            Instance = this;
            
            // get projects from config and load to _Projects
            //var projects = QMain.qConfig.GetValue<List<string>>("Projects");
            //foreach (var project in projects)
            //{
            //    _Projects.Add(new Project(project, Path.Combine(QMain.qConfig.GetValue<string>("ProjectsRoot"), project)));
            //}
        }

        public Project AddProject(string Name, string AbsoluteDirectory)
        {
            var pr = new Project(Name, AbsoluteDirectory);
            _Projects.Add(pr);
            return pr;
        }

        public Project AddProject(string Name)
        {
            // here we will make a project, its name being supplied, it directory being its name within the projects directory (grabbed from the config)
            var pr = new Project(Name, Path.Combine(QMain.qConfig.GetValue<string>("ProjectsRoot"), Name));
            _Projects.Add(pr);
            return pr;
        }

        public void OpenProject(string Name)
        {
            Logger.Instance.WithClass($"Opening project {Name}... Does nothing yet.");
        }

        public static void OpenProject(Project project)
        {
            Logger.Instance.WithClass($"Opening project {project.GetValue<string>("Name")}... Does nothing yet.");
        }
        
        public Project GetProject(string Name)
        {
            return _Projects.Find(x => x.GetValue<string>("Name") == Name);
        }

        public List<Project> GetProjects()
        {
            return _Projects;
        }
    }
}