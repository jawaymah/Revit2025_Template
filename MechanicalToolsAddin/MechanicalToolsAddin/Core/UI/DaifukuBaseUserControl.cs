using Autodesk.Revit.UI;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using Theme = MaterialDesignThemes.Wpf.Theme;

namespace MechanicalToolsAddin.Core.UI
{
    public class DaifukuBaseUserControl : UserControl , IChangeStyle
    {
        public DaifukuBaseUserControl()
        {
            try
            {
                if (!Globals.ActiveViews!.Contains(this))
                {
                    Globals.ActiveViews.Add(this);
                }
                Loaded += DaifukuBaseUserControl_Loaded;
            }
            catch
            {
                return;
            }
        }

        private void DaifukuBaseUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetBaseStyle();
            SetFontSize();
        }

        public void Control_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        public void SetBaseStyle()
        {
            try
            {
                ResourceDictionary? themeResourceDictionary = Resources?.MergedDictionaries.SingleOrDefault(x => x is IMaterialDesignThemeDictionary);
                var userValue = Properties.Settings.Default.Theme;
                Theme? theme = themeResourceDictionary?.GetTheme();
                if (theme == null) { return; }
                if (userValue.ToLower() == "dark")
                    ApplyDark(theme);
                else if (userValue.ToLower() == "light")
                    ApplyLight(theme);
                else
                {
                    var currentRevitTheme = UIThemeManager.CurrentTheme;
                    if (currentRevitTheme == UITheme.Dark)
                        ApplyDark(theme);
                    else
                        ApplyLight(theme);
                }
                themeResourceDictionary?.SetTheme(theme);
            }
            catch (Exception)
            {
                return;
            }
        }
        private void ApplyDark(Theme theme)
        {
            theme.SetDarkTheme();
            ApplyChanges(Color.FromArgb(255, 57, 57, 58));

            // Ensure textbox hint background uses dark color (#FF252526) for this control scope
            Resources["TextFieldBackgroundBrush"] = new SolidColorBrush(Color.FromArgb(255, 37, 37, 38));
        }
        private void ApplyLight(Theme theme)
        {
            theme.SetLightTheme();
            ApplyChanges(Color.FromArgb(255, 190, 190, 190));

            // Restore light textbox hint background for this control scope
            Resources["TextFieldBackgroundBrush"] = new SolidColorBrush(Colors.White);
        }

        private void ApplyChanges(Color innerColor)
        {
            var innerBorder = FindName("innerBorder") as Border;
            if (innerBorder != null)
            {
                innerBorder.Background = new SolidColorBrush(innerColor);
            }
        }
        public void SetFontSize()
        {
            Handler.SilentExecute(() => { FontSize = Properties.Settings.Default.FontSize; });
        }
    }
}
