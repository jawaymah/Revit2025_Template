using MechanicalToolsAddin.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicalToolsAddin.Core.Helpers
{
    public class ErrorHandler
    {
        public static void HandleException(Exception ex, string title = "Error")
        {
            var errorMessage = $"Exception: {ex.Message}";
            //view messageBox
        }
    }
}
