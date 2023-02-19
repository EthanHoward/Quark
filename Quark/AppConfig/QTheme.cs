using System.Windows;

namespace Quark.AppConfig
{
    public static class QTheme
    {
        public static void SetTheme(AppTheme t)
        {
            switch (t)
            {
                case AppTheme.DefaultDark:
                    Application.Current.Resources["AppStyle"] = Application.Current.Resources["ThemeDarkDefault"];
                    break;
                case AppTheme.DefaultLight:
                    Application.Current.Resources["AppStyle"] = Application.Current.Resources["ThemeLightDefault"];
                    break;
                default:
                    Application.Current.Resources["AppStyle"] = Application.Current.Resources["ThemeLightDefault"];
                    break;
            }
        }


        public static AppTheme GetTheme()
        {
            switch (Application.Current.Resources["AppStyle"])
            {
                case "ThemeDarkDefault":
                    return AppTheme.DefaultDark;
                case "ThemeLightDefault":
                    return AppTheme.DefaultLight;
                default:
                    return AppTheme.DefaultLight;
            }
        }
    }

    public enum AppTheme
    {
        DefaultLight,
        DefaultDark
    }
}