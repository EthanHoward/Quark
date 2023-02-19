using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Quark.AppConfig.Utils;
using Quark.Util.Logging;

namespace Quark.AppConfig
{
    public class QConfig
    {
        public static QConfig Instance;
        private readonly string _configFilePath;
        private readonly string _dataRoot;
        private readonly string _userName;
        private JObject _configFile;

        public QConfig()
        {
            var userName = GrabUser.ShowDialog();

            if (userName == null)
            {
                if (Logger.Instance != null)
                    Logger.Instance.Error("No user selected, exiting...");
                Environment.Exit(-235);
            }

            _userName = userName;
            _dataRoot = $@"C:\Users\{userName}\Quark";
            _configFilePath = $@"C:\Users\{userName}\Quark\Config\config.json";

            LoadConfig();
            Validate();
            Instance = this;
        }


        public QConfig GetInstance()
        {
            return Instance;
        }

        private void LoadConfig()
        {
            if (!Directory.Exists(_dataRoot))
            {
                if (Logger.Instance != null)
                    Logger.Instance.WithClass($"Config directory not found, creating new one... {_dataRoot}");
                Directory.CreateDirectory(_dataRoot);
            }

            if (!File.Exists(_configFilePath))
            {
                if (Logger.Instance != null)
                    Logger.Instance.WithClass($"Config file not found, creating new one... {_configFilePath}");
                File.Create(_configFilePath).Close();
                _configFile = new JObject();
                SaveConfig();
            }
            else
            {
                if (Logger.Instance != null)
                    Logger.Instance.WithClass($"Config file found, loading... {_configFilePath}");
                _configFile = JObject.Parse(File.ReadAllText(_configFilePath));
            }
        }

        public void SaveConfig()
        {
            //if (Logger.Instance != null) Logger.Instance.WithClass($"Saving config... {_configFilePath}");
            File.WriteAllText(_configFilePath, _configFile.ToString());
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            //if (Logger.Instance != null) Logger.Instance.WithClass($"Getting value from config... {key}");
            return _configFile.ContainsKey(key) ? _configFile[key].Value<T>() : defaultValue;
        }

        public T GetValue<T>(string key)
        {
            //if (Logger.Instance != null) Logger.Instance.WithClass($"Getting value from config... {key}");
            return _configFile.ContainsKey(key) ? _configFile[key].Value<T>() : default;
        }

        public JArray GetArray(string key)
        {
            //if (Logger.Instance != null) Logger.Instance.WithClass($"Getting array from config... {key}");
            return _configFile.ContainsKey(key) ? (JArray)_configFile[key] : new JArray();
        }

        public void SetArray(string key, JArray array)
        {
            //if (Logger.Instance != null) Logger.Instance.WithClass($"Setting array in config... {key}");
            _configFile[key] = array;
        }

        public void SetValue<T>(string key, T value)
        {
            //if (Logger.Instance != null) Logger.Instance.WithClass($"Setting value in config... {key}");
            _configFile[key] = new JValue(value);
        }

        private void Validate()
        {
            if (Logger.Instance != null) Logger.Instance.WithClass("Validating config...");
            // Check if config directory exists
            if (!Directory.Exists(_dataRoot)) Directory.CreateDirectory(_dataRoot);

            // Check if config file exists
            if (!File.Exists(_configFilePath)) File.Create(_configFilePath).Close();

            var values = new List<object[]>();
            values.Add(new object[] { "Version", "1.0-SNAPSHOT" });
            values.Add(new object[] { "DataRoot", $@"C:\Users\{_userName}\Quark\" });
            values.Add(new object[] { "ProjectsRoot", $@"C:\Users\{_userName}\Quark\Projects" });
            values.Add(new object[] { "ProjectMetas", $@"C:\Users\{_userName}\Quark\ProjectMetas" });
            values.Add(new object[] { "LastProject", "" });
            values.Add(new object[] { "UserSelected", $"{_userName}" });

            foreach (var value in values.Where(value => !_configFile.ContainsKey(value[0].ToString())))
            {
                if (Logger.Instance != null) Logger.Instance.WithClass($"Config value not found, adding... {value[0]}");
                if (value[1] is JArray) _configFile[value[0].ToString()] = new JArray();
                else _configFile[value[0].ToString()] = new JValue(value[1]);
            }

            SaveConfig();
        }
    }
}