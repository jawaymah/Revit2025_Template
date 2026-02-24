using Autodesk.Revit.UI;
using Serilog;
using System;
using System.Windows;
using System.Windows.Interop;

namespace MechanicalToolsAddin.Core.Helpers
{
    public static class CommandManager
    {
        /// <summary>
        /// Shows or restores a singleton WPF window and assigns Revit as its owner.
        /// You can optionally pass a factory for windows that don't have a parameterless constructor.
        /// </summary>
        public static T ShowSingletonWindow<T>(
            ref T? window,
            IntPtr ownerHandle,
            Func<T>? factory = null)
            where T : Window, new()
        {
            try
            {
                Log.Information($"Opening {typeof(T).Name}");

                // Create the window if it's null or closed
                if (window == null || !window.IsVisible)
                {
                    // use factory if provided, otherwise use new()
                    window = factory != null ? factory() : new T();

                    if (ownerHandle != IntPtr.Zero)
                        new WindowInteropHelper(window).Owner = ownerHandle;
                }
                else
                {
                    // Ensure owner is correctly set
                    if (ownerHandle != IntPtr.Zero)
                    {
                        var currentOwner = new WindowInteropHelper(window).Owner;
                        if (currentOwner == IntPtr.Zero || currentOwner != ownerHandle)
                            new WindowInteropHelper(window).Owner = ownerHandle;
                    }
                }

                // Restore window if minimized or inactive
                if (window.WindowState == WindowState.Minimized || !window.IsActive || !window.ShowActivated)
                {
                    window.WindowState = WindowState.Minimized;
                    window.WindowState = WindowState.Normal;
                }

                // Show or activate the window
                if (!window.IsVisible)
                    window.Show();
                else
                    window.Activate();

                Log.Information($"End {typeof(T).Name}");
                return window;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to open {typeof(T).Name}");
                ErrorHandler.HandleException(ex, "Warning");
                throw;
            }
        }

        public static Result RunWindowCommand<T>(ref T? window, ExternalCommandData commandData, Func<T>? factory = null)
           where T : System.Windows.Window, new()
        {
            try
            {
                CommandManager.ShowSingletonWindow(ref window, commandData.Application.MainWindowHandle, factory);
                return Result.Succeeded;
            }
            catch
            {
                return Result.Failed;
            }
        }
    }
}
