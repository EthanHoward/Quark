using System.IO;
using Newtonsoft.Json.Linq;
using Quark.Util.Logging;

namespace Quark.FileManagement.Projects
{
    public class Project
    {
        private readonly bool _shouldLog = false;
        private readonly bool _shouldSave;
        public readonly string Location;
        public readonly string MetaPath;
        public readonly string Name;

        private JObject _projectFile;


        public Project(string Name, string Directory, bool shouldSave)
        {
            this.Name = Name;
            Location = Directory;
            MetaPath = Path.Combine(QMain.qConfig.GetValue<string>("ProjectMetas"), $"{this.Name}.json");
            _shouldSave = shouldSave;
            var shouldLoad = ShouldLoad();
            if (shouldLoad)
            {
                Logger.Instance.Info("Project file found, loading...");
                Load();
            }
            else
            {
                Logger.Instance.Info("Project file not found, creating new one...");
                Create();
            }
        }

        private bool ShouldLoad()
        {
            return File.Exists(MetaPath);
        }

        private void Load()
        {
            if (_shouldLog) Logger.Instance.Debug($"Loading project file... {MetaPath}");
            _projectFile = JObject.Parse(File.ReadAllText(MetaPath));
        }

        private void Create()
        {
            if (_shouldLog) Logger.Instance.Debug($"Creating project file... {MetaPath}");
            _projectFile = new JObject();
            // store Name, Location and other shit, for now though, just name and directory, not the path of the qmproject file but the directory its in.
            _projectFile.Add("Name", Name);
            _projectFile.Add("Directory", Location);
            if (_shouldSave) Save();
        }

        public void Save()
        {
            if (_shouldLog) Logger.Instance.Debug($"Saving project file... {MetaPath}");

            if (!Directory.Exists(Location)) Directory.CreateDirectory(Location);

            var projectsMetadataDirectory = QMain.qConfig.GetInstance().GetValue<string>("ProjectMetas");
            if (!Directory.Exists(projectsMetadataDirectory)) Directory.CreateDirectory(projectsMetadataDirectory);
            File.WriteAllText(MetaPath, _projectFile.ToString());
        }

        public T GetValue<T>(string key)
        {
            Logger.Instance.Debug($"Getting value from project file... {key}");
            return _projectFile.ContainsKey(key) ? (_projectFile[key] ?? "").Value<T>() : default;
        }
    }
}