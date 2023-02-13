using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Quark.Classes.Util.Logging;

namespace Quark.Classes.Util.Config
{
    public class Config
    {
        // Crap really, but it just does not want to work. ever.
        private const string PredefinedConfigDir = @"C:\Users\Ethan\Quark\Config\";
        private static string PredefinedConfigFile = PredefinedConfigDir + "quark.config.json";

        private JObject ConfigFile;

        public Config()
        {
            LoadConfig();
            Validate();
        }

        private void LoadConfig()
        {
            if (!Directory.Exists(PredefinedConfigDir))
            {
                Logger.INSTANCE.AddLog($"[CONFIG] Config directory not found, creating new one... {PredefinedConfigDir}");
                Directory.CreateDirectory(PredefinedConfigDir);
            }
            if (!File.Exists(PredefinedConfigFile))
            {
                Logger.INSTANCE.AddLog($"[CONFIG] Config file not found, creating new one... {PredefinedConfigFile}");
                File.Create(PredefinedConfigFile).Close();
                ConfigFile = new JObject();
                SaveConfig();
            }
            else
            {
                Logger.INSTANCE.AddLog($"[CONFIG] Config file found, loading... {PredefinedConfigFile}");
                ConfigFile = JObject.Parse(File.ReadAllText(PredefinedConfigFile));
            }
        }

        public void SaveConfig()
        {
            Logger.INSTANCE.AddLog($"[CONFIG] Saving config... {PredefinedConfigFile}");
            File.WriteAllText(PredefinedConfigFile, ConfigFile.ToString());
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            if (ConfigFile.ContainsKey(key))
            {
                return ConfigFile[key].Value<T>();
            }

            return defaultValue;
        }

        public void SetValue<T>(string key, T value)
        {
            ConfigFile[key] = new JValue(value);
        }

        private void Validate()
        {
            // Check if config directory exists
            if (!Directory.Exists(PredefinedConfigDir))
            {
                Directory.CreateDirectory(PredefinedConfigDir);
            }
            
            // Check if config file exists
            if (!File.Exists(PredefinedConfigFile))
            {
                File.Create(PredefinedConfigFile).Close();
            }
            
            List<Object[]> values = new List<object[]>();
            values.Add(new object[] {"version", "1.0.0"});
            values.Add(new object[] {"projectDirectory", @"C:\Users\Ethan\Quark\Projects"});
            values.Add(new object[] {"lastProject", ""});
            values.Add(new object[] {"projects", new JArray()});
            
            foreach (var value in values)
            {
                if (!ConfigFile.ContainsKey(value[0].ToString()))
                {
                    if (value[1] is JArray) ConfigFile[value[0].ToString()] = new JArray();
                    else ConfigFile[value[0].ToString()] = new JValue(value[1]);
                }
            }
        }

        public JObject NewProject(string Name, string ProjectDir)
        {
            JObject project = new JObject();
            project["name"] = Name;
            project["directory"] = ProjectDir;
            project["qmproj"] = ProjectDir + @"\" + Name + ".qmproj";
            project["lastOpened"] = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            return project;
        }
        
        public void AddToProjects(JObject project)
        {
            JArray projects = ConfigFile["projects"].Value<JArray>();
            projects.Add(project);
            ConfigFile["projects"] = projects;
        }
        
    }
}