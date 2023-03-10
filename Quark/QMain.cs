using System;
using System.Windows;
using Quark.AppConfig;
using Quark.FileManagement.Projects;
using Quark.Pages;
using Quark.Util.Logging;

namespace Quark
{
    // MAKE A GITIGNORE TO IGNORE LOGS AND OTHER .G.CS SHIT.

    /* TODOS
     * 1: Make config.json save into the Config folder rather than the DataRoot...
     * => Error when bringing Projects array from the config file, its due to some casting wizardry
     * 2: Actually give functionality to the buttons on SelectProject.xaml.
     * 3: Begin making main editor window, allow loading projects for now.
     * 3.2: Way to select different files in the project, include a file explorer panel, a bar to show the current file and other ones, selectable and a close button.
     * 3.3: Definitely an auto-save feature, and a way to save manually (ctrl-s).
     * 3.4: The vscode cursor thing where you press alt arrow to select multiple cursors (multi-cursor).
     * 3.5: Maybe a way to add c# plugins albeit it may be unmanageable by only one dev, me :~).
     * 4: Theming - not that important, but would be nice to have.
     * 5: Syntax Highlighting ...
     * 6: When needed, a preferences menu / window.
     * 7: When needed, a project menu / window similar to JB IDE.
     * => Can't really think of anything else right now, but I'm sure I'll think of more.
     */
    public static class QMain
    {
        private static Logger logger;
        public static QConfig qConfig;
        public static Projects projects;
        private static App app;

        [STAThread]
        public static void Main()
        {
            qConfig = new QConfig();
            logger = new Logger("<QMain> Starting Quark...", true);

            projects = new Projects();

            app = new App();
            app.InitializeComponent();
            app.Run();
        }

        public static App GetApp()
        {
            return app;
        }

        public static Window GetWindow(string title)
        {
            var windows = Application.Current.Windows;
            foreach (Window window in windows)
                if (window.Title == title)
                    return window;

            return null;
        }
    }
}