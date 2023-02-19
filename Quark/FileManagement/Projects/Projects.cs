using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Quark.Util.Logging;

namespace Quark.FileManagement.Projects
{
    public class Projects
    {
        private readonly List<Project> _projects;
        public readonly Projects Instance;

        public Projects()
        {
            _projects = new List<Project>();
            Instance = this;
            LoadFromMetas();
        }

        private void LoadFromMetas()
        {
            var projectMetafiles = Directory.GetFiles(QMain.qConfig.GetValue<string>("ProjectMetas"), "*.json");
            foreach (var metafile in projectMetafiles)
            {
                var data = JObject.Parse(File.ReadAllText(metafile));
                Logger.Instance.WithClass($"Found a metafile, {data.GetValue("Name")} at {data.GetValue("Directory")}");
                if (data.GetValue("Name") == null || data.GetValue("Directory") == null)
                {
                    Logger.Instance.Debug("Metafile is invalid, has no directory and/or name");
                    continue;
                }

                var project = new Project(data.GetValue("Name")?.ToString(), data.GetValue("Directory")?.ToString(),
                    true);
                Append(project);
            }

            Logger.Instance.WithClass(_projects.ToString());
        }

        public Project NewProject(string Name, string AbsoluteDirectory)
        {
            var pr = new Project(Name, AbsoluteDirectory, true);
            // check if it exists in array
            if (_projects.All(x => x.GetValue<string>("Name") != Name))
            {
                _projects.Add(pr);
                return pr;
            }

            Logger.Instance.WithClass(
                $"Project {Name} already exists in config, not adding it again. (This is not a bug, you cannot have two projects with the same name)");
            return null;
        }

        public static void OpenProject(Project project)
        {
            Logger.Instance.Debug($"Opening project {project.GetValue<string>("Name")}... Does nothing yet.");
        }

        public Project GetProject(string Name)
        {
            return _projects.Find(x => x.GetValue<string>("Name") == Name);
        }

        public List<Project> GetProjects()
        {
            return _projects;
        }

        private void Append(Project project)
        {
            Logger.Instance.Debug(
                $"Appending project {project.GetValue<string>("Name")} to projects array Projects.cs:12");
            _projects.Add(project);
        }

        private bool Validate(Project project)
        {
            // here we must validate the project against a "WAGOLL"
            var WAGOLL = new JObject();
            WAGOLL.Add(new object[] { "Name", "dddddddddddddddddddd" });
            WAGOLL.Add(new object[] { "Directory", "dddddddddddddddddddd" });
            //WAGOLL.Add("Files", new JArray("array")); not adding yet, will be added later
            //WAGOLL.Add("OpenFiles", new JArray("array")); not adding yet, will be added later
            // now check
            foreach (var token in WAGOLL)
                // ensure it is a JValue or a JArray as they are different and may throw errors if incorrectly checked
                if (token.Value is JValue)
                {
                    if (project.GetValue<string>(token.Key) == null)
                    {
                        Logger.Instance.Debug(
                            $"Project {project.GetValue<string>("Name")} is invalid, missing {token.Key}");
                        return false;
                    }
                }
                else if (token.Value is JArray)
                {
                    if (project.GetValue<JArray>(token.Key) == null)
                    {
                        Logger.Instance.Debug(
                            $"Project {project.GetValue<string>("Name")} is invalid, missing {token.Key}");
                        return false;
                    }
                }

            return true;
        }
    }
}