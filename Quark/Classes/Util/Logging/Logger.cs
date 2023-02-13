using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Quark.Classes.Util.Logging
{
    public class Logger
    {
        private readonly string CurDir;
        private readonly string LogPath;
        private readonly List<string> Logs;
        private readonly string CurrentLogFile;
        private static string DS;
        public static Logger INSTANCE;
        
        public Logger()
        {
            INSTANCE = this;
            Logs = new List<string>();
            CurDir = Directory.GetCurrentDirectory();
            LogPath = Path.Combine(CurDir, "logs");
            SetName();
            CurrentLogFile = Path.Combine(LogPath, $"quark-{DS}.log");
        }

        private static void SetName()
        {
            // Extend this to include time as well
            DS = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }
        
        public void AddLog(string log)
        {
            Logs.Add(log);
        }

        public void SaveLog()
        {
            var contents = new StringBuilder();
            
            // DATE IS 19 chars
            // 
            
            contents.Append($"===================================== {DS} =====================================\n");

            foreach (var line in Logs) contents.Append(line + "\n");
            if (Logs.Count == 0)
            {
                contents.Append("|                                                                                             |\n");
                contents.Append("======================================= NO LOGS TO SHOW =======================================\n");
                contents.Append("|                                                                                             |\n");
            }

            contents.Append($"===================================== {DS} =====================================\n");
            
            var LogFilePath = Path.Combine(LogPath, CurrentLogFile);
            File.WriteAllText(LogFilePath, contents.ToString());
            
            // Open in vscode
            System.Diagnostics.Process.Start("code", LogFilePath);
        }
    }
}