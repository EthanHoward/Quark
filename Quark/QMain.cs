using System;
using Quark.Classes.Util.Config;
using Quark.Classes.Util.Logging;
using Quark.Pages;

namespace Quark
{
    public class QMain
    {
        private static Logger logger;
        private static Config config;
        
        [STAThread]
        public static void Main()
        {
            logger = new Logger();

            config = new Config();
            
            logger.AddLog("[MAIN] Starting Quark...");
            logger.SaveLog();
            config.SaveConfig();
            
            App a = new App();
            a.InitializeComponent();
            a.Run();
            
            
        }
    }
}