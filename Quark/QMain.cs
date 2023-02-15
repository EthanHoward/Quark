using System;
using Quark.Classes.Config;
using Quark.Classes.FileManagement;
using Quark.Classes.Util.Logging;
using Quark.Pages;

namespace Quark
{
    
    /* TODOS
     * Make config.json save into the Config folder rather than the DataRoot... 
     * Error when bringing Projects array from the config file, its due to some casting wizardry
     * Actually give functionality to the buttons on SelectProject.xaml
     */
    public class QMain
    {
        private static Logger logger;
        public static QConfig qConfig;
        public static Projects projects;

        [STAThread]
        public static void Main()
        {
            qConfig = new QConfig();
            logger = new Logger("<QMain> Starting Quark...");

            projects = new Projects();

            projects.AddProject("Bean1");
            
            var a = new App();
            a.InitializeComponent();
            a.Run();
        }
    }
}