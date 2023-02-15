using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Quark.Classes.Util.Logging
{
    public class Logger
    {
        private static string DS;
        public static Logger Instance;
        private readonly string CurrentLogFile;
        private readonly string LogPath;
        private readonly List<string> Logs;

        public Logger(string InitMessage)
        {
            Instance = this;
            Logs = new List<string>();
            LogPath = Path.Combine(QMain.qConfig.GetValue<string>("DataRoot"), "Logs");
            SetName();
            CurrentLogFile = Path.Combine(LogPath, $"quark-{DS}.log");
            PlainLog(InitMessage);
        }

        private static void SetName()
        {
            // Extend this to include time as well
            DS = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }

        public void PlainLog(string log)
        {
            Logs.Add(log);
        }

        public void Error(string log)
        {
            Logs.Add($"<ERROR> {log}");
        }

        public void Info(string log)
        {
            Logs.Add($"<INFO> {log}");
        }

        public void Warn(string log)
        {
            Logs.Add($"<WARN> {log}");
        }

        public void Debug(string log)
        {
            Logs.Add($"<DEBUG> {log}");
        }

        public void WithClass(string log)
        {
            var className = new StackTrace().GetFrame(1).GetMethod().DeclaringType?.Name;
            Logs.Add(className == null ? $"<UNDEFINED> {log}" : $"<{className}> {log}");
        }

        public void AppendStringList(List<string> lines)
        {
            foreach (var line in lines) Logs.Add(line);
        }

        public void SaveLog()
        {
            var contents = new StringBuilder();

            contents.Append($"< {DS} >\n");

            foreach (var line in Logs) contents.Append(line + "\n");
            if (Logs.Count == 0) contents.Append("< NO LOGS TO SHOW >\n");

            contents.Append($"< {DS} >\n");

            if (!Directory.Exists(LogPath)) Directory.CreateDirectory(LogPath);
            var logFilePath = Path.Combine(LogPath, CurrentLogFile);
            File.WriteAllText(logFilePath, contents.ToString());

            // Open in vscode
            Process.Start("code", logFilePath);
        }
    }
}