using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace MechanicalToolsAddin
{
    public class PanelRibbonExtractor
    {
        public class AddinRibbonItem
        {
            public string PanelName { get; set; }
            public string RibbonName { get; set; }
        }

        //public class AddinRibbonPanel
        //{
        //    public string PanelName { get; set; }
        //    public List<AddinRibbonItem> RibbonItems { get; set; } = new List<AddinRibbonItem>();
        //}
        public static string resultTitle { get; set; }
        public static string resultMessage { get; set; }
        public static void ExtractPanelsAndRibbons(UIControlledApplication uiApp, string outputPath = "")
        {
            outputPath = @"C:\Log";
            var panelsData = new List<AddinRibbonItem>();

            // Get all ribbon panels
            var allPanels = uiApp.GetRibbonPanels("Daifuku");

            foreach (var panel in allPanels)
            {
                // Get all items in the current panel
                foreach (var item in panel.GetItems())
                {
                    var itemData = new AddinRibbonItem
                    {
                        PanelName = panel.Name,
                        RibbonName = item.ItemText,
                    };
                    panelsData.Add(itemData);
                }
            }
        }
    }
}