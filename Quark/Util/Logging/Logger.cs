using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Quark.Util.Logging
{
    public class Logger
    {
        private static string _ds;
        public static Logger Instance;
        private readonly string _currentLogFile;
        private readonly bool _isDebug;
        private readonly string _logPath;
        private readonly List<string> _logs;

        public Logger(string initMessage, bool isDebug)
        {
            Instance = this;
            _logs = new List<string>();
            _logPath = Path.Combine(QMain.qConfig.GetValue<string>("DataRoot"), "Logs");
            SetName();
            _currentLogFile = Path.Combine(_logPath, $"quark-{_ds}.log");
            PlainLog(initMessage);
            _isDebug = isDebug;
            if (!_isDebug) return;
        }

        private static void SetName()
        {
            // Extend this to include time as well
            _ds = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }

        public void PlainLog(string log)
        {
            _logs.Add(log);
        }

        public void Error(string log)
        {
            var className = new StackTrace().GetFrame(1).GetMethod().DeclaringType?.Name;
            _logs.Add(className == null ? $"<UNDEFINED> <ERROR> {log}" : $"<{className}> <ERROR> {log}");
        }

        public void Info(string log)
        {
            var className = new StackTrace().GetFrame(1).GetMethod().DeclaringType?.Name;
            _logs.Add(className == null ? $"<UNDEFINED> <INFO> {log}" : $"<{className}> <INFO> {log}");
        }

        public void Warn(string log)
        {
            var className = new StackTrace().GetFrame(1).GetMethod().DeclaringType?.Name;
            _logs.Add(className == null ? $"<UNDEFINED> <WARN> {log}" : $"<{className}> <WARN> {log}");
        }

        public void Debug(string log)
        {
            var className = new StackTrace().GetFrame(1).GetMethod().DeclaringType?.Name;
            if (_isDebug) _logs.Add(className == null ? $"<UNDEFINED> <DEBUG> {log}" : $"<{className}> <DEBUG> {log}");
        }

        public void WithClass(string log)
        {
            var className = new StackTrace().GetFrame(1).GetMethod().DeclaringType?.Name;
            _logs.Add(className == null ? $"<UNDEFINED> {log}" : $"<{className}> {log}");
        }

        public void AppendStringList(List<string> lines)
        {
            foreach (var line in lines) _logs.Add(line);
        }

        public void SaveLog()
        {
            var contents = new StringBuilder();

            contents.Append($"< {_ds} >\n");

            foreach (var line in _logs) contents.Append(line + "\n");
            if (_logs.Count == 0) contents.Append("< NO LOGS TO SHOW >\n");

            contents.Append($"< {_ds} >\n");

            if (!Directory.Exists(_logPath)) Directory.CreateDirectory(_logPath);
            var logFilePath = Path.Combine(_logPath, _currentLogFile);
            File.WriteAllText(logFilePath, contents.ToString());

            // Open in vscode
            Process.Start("code", logFilePath);
        }
    }
}