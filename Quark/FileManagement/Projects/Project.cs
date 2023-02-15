using System.IO;
using Newtonsoft.Json.Linq;
using Quark.Classes.Util.Logging;

namespace Quark.Classes.FileManagement
{
    public class Project
    {
        private readonly string _Location;
        private readonly string _Name;

        private JObject _ProjectFile;

        public Project(string Name, string Directory)
        {
            _Name = Name;
            _Location = Directory;
            var shouldLoad = ShouldLoad();
            if (shouldLoad)
            {
                Logger.Instance.WithClass("Project file found, loading...");
                Load();
            }
            else
            {
                Logger.Instance.WithClass("Project file not found, creating new one...");
                Create();
            }
        }

        private bool ShouldLoad()
        {
            return File.Exists(Path.Combine(_Location, "project.json"));
        }

        private void Load()
        {
            Logger.Instance.WithClass($"Loading project file... {_Location}");
            _ProjectFile = JObject.Parse(File.ReadAllText(Path.Combine(_Location, "project.json")));
        }

        private void Create()
        {
            Logger.Instance.WithClass($"Creating project file... {_Location}");
            _ProjectFile = new JObject();
            // store Name, Location and other shit, for now though, just name and directory, not the path of the qmproject file but the directory its in.
            _ProjectFile.Add("Name", _Name);
            _ProjectFile.Add("Directory", _Location);
            Save();
        }

        public void Save()
        {
            Logger.Instance.WithClass($"Saving project file... {_Location}");
            if (!Directory.Exists(_Location)) Directory.CreateDirectory(_Location);
            File.WriteAllText(Path.Combine(_Location, "project.json"), _ProjectFile.ToString());
        }

        // make a T function to retrieve value of type from the file (internally via the JObject), return null otherwise
        public T GetValue<T>(string key)
        {
            Logger.Instance.WithClass($"Getting value from project file... {key}");
            if (_ProjectFile.ContainsKey(key)) return _ProjectFile[key].Value<T>();
            return default;
        }
    }
}