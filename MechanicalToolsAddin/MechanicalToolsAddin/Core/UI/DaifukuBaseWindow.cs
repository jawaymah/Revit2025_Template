using Autodesk.Revit.UI;
using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Theme = MaterialDesignThemes.Wpf.Theme;

namespace MechanicalToolsAddin.Core.UI
{
    public class DaifukuBaseWindow : Window , INotifyPropertyChanged, IChangeStyle
    {
        public bool ApplyControlButtonStyles { get; set; } = true;

        public event EventHandler? ClosingRequest;
        public event PropertyChangedEventHandler? PropertyChanged;
       
        protected void OnClosingRequest()
        {
            ClosingRequest?.Invoke(this, EventArgs.Empty);
        }

        public void ApplyStyles()
        {
            SetBaseStyle();
            InitializeStyle();
            SetFontSize();
        }

        public void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        public DaifukuBaseWindow()
        {
            try
            {
                Loaded += delegate { InitializeEvents(); };
                if (!Globals.ActiveViews!.Contains(this))
                {
                    Globals.ActiveViews.Add(this);
                }
                SetFontSize();
            }
            catch
            {
                return;
            }
        }

        public void SetFontSize()
        {
          Handler.SilentExecute(() => { FontSize = Properties.Settings.Default.FontSize; });
        }

        public void SetBaseStyle()
        {
            try
            {
                ResourceDictionary? themeResourceDictionary = Resources?.MergedDictionaries.SingleOrDefault(x => x is IMaterialDesignThemeDictionary);
                var userValue = Properties.Settings.Default.Theme;
                Theme? theme = themeResourceDictionary?.GetTheme();
                if(theme == null) { return; }
                if (userValue.ToLower() == "dark")
                    ApplyDark(theme);
                else if(userValue.ToLower() == "light")
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

        private void ApplyChanges(Color innerColor,Color outerColor)
        {
            var innerBorder = FindName("innerBorder") as Border;
            var outerBorder = FindName("outerBorder") as Border;
            if (innerBorder != null)
            {
                innerBorder.Background = new SolidColorBrush(innerColor);
            }
            if(outerBorder != null)
            {
                outerBorder.Background = new SolidColorBrush(outerColor);
            }
        }

        private void ApplyDark(Theme theme)
        {
            theme.SetDarkTheme();
            ApplyChanges(Color.FromArgb(255, 57, 57, 58), Color.FromArgb(255, 37, 37, 38));

            // Ensure textbox hint background uses dark color (#FF252526) for this window scope
            Resources["TextFieldBackgroundBrush"] = new SolidColorBrush(Color.FromArgb(255, 37, 37, 38));
        }
        private void ApplyLight(Theme theme)
        {
            theme.SetLightTheme();
            ApplyChanges(Color.FromArgb(255, 245, 245, 245), Color.FromArgb(255, 255, 255, 255));

            // Restore light textbox hint background for this window scope
            Resources["TextFieldBackgroundBrush"] = new SolidColorBrush(Colors.White);
        }
        public void InitializeStyle()
        {
            try
            {
                var resourceDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/DaifukuRevitAddin;component/UI/ResourceDictionray.xaml", UriKind.Absolute)
                };
                Style = resourceDict?["MaterialWindow"] as Style;
            }
            catch (Exception)
            {
                return;
            }
        }

        public void outerBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Handler.SilentExecute(() => DragMove());
        }

        private void InitializeEvents()
        {
            var resourceDict = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/DaifukuRevitAddin;component/UI/ResourceDictionray.xaml", UriKind.Absolute)
            };
            var minBtn = FindName("minBtn") as Button;
            var maxBtn = FindName("maxBtn") as Button;
            var closeBtn = FindName("closeBtn") as Button;
            Border? innerBorder = FindName("innerBorder") as Border;
            Border? outerBorder = FindName("outerBorder") as Border;
            if (minBtn != null)
            {
                if (ApplyControlButtonStyles)
                {
                    minBtn.Style = resourceDict?["MinimizeButtonStyle"] as Style;
                }
                minBtn.Click += (sender, args) => Handler.SilentExecute(() => WindowState = WindowState.Minimized);
            }

            if (closeBtn != null)
            {
                if (ApplyControlButtonStyles)
                {
                 closeBtn.Style = resourceDict?["CloseButtonStyle"] as Style;
                }
                closeBtn.Click += (sender, args) => Handler.SilentExecute(CloseWindow);
            }

            if (maxBtn != null)
            {
                if (ApplyControlButtonStyles)
                {
                    maxBtn.Style = resourceDict?["MaximizeButtonStyle"] as Style;
                }
                maxBtn.Click += (sender, args) => Handler.SilentExecute(() =>
                    {
                        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                        AdjustCornerRadius(outerBorder, innerBorder);
                    });
            }
            if (innerBorder != null)
            {
                innerBorder.MouseDown += (sender, args) => Handler.SilentExecute(DragMove);
                if(innerBorder != null)
                {
                    innerBorder.MouseLeftButtonDown += (sender, args) => Handler.SilentExecute(() =>
                    {
                        if (args.ClickCount == 2)
                            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                        AdjustCornerRadius(outerBorder,innerBorder);
                    });
                }
            }
        }

        public void CloseWindow()
        {
            if (this != null)
            {
                Handler.SilentExecute(() =>
                {
                    if (Globals.ActiveViews?.Contains(this) == true)
                        Globals.ActiveViews?.Remove(this);
                });
            }
            Close();
        }

        private void AdjustCornerRadius(Border? outerBorder,Border? innerBorder)
        {
            if (innerBorder != null && outerBorder != null)
            {
                if (WindowState == WindowState.Maximized)
                {
                    innerBorder.CornerRadius = new CornerRadius(0);
                    outerBorder.CornerRadius = new CornerRadius(0);
                }
                else
                {
                    innerBorder.CornerRadius = new CornerRadius(8,8,0,0);
                    outerBorder.CornerRadius = new CornerRadius(8);
                }
            }
        }
    }
}
