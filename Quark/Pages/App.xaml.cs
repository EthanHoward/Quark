using System.Windows;
using Quark.AppConfig;
using Quark.Util.Logging;

namespace Quark.Pages
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        // do something on quit
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            QConfig.Instance.SaveConfig();
            if (Logger.Instance != null) Logger.Instance.SaveLog();
        }
    }
}