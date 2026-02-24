using MechanicalToolsAddin.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicalToolsAddin
{
    public static class Handler
    {
        public static void Execute(Action action, string context = "Warning")
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, context);
            }
        }

        public static T Execute<T>(Func<T> func, string context = "Warning", T fallback = default)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, context);
                return fallback;
            }
        }

        public static void SilentExecute(Action action)
        {
            try
            {
                action();
            }
            catch { /* Swallow exception silently */ }
        }

        public static T SilentExecute<T>(Func<T> func, T fallback = default)
        {
            try
            {
                return func();
            }
            catch
            {
                return fallback;
            }
        }
    }
}
