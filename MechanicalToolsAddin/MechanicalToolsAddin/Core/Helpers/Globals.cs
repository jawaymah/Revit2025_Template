using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using MechanicalToolsAddin.Core.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechanicalToolsAddin
{
    public static class Globals
    {
        public static Document Doc { get; set; }
        public static UIDocument UIDoc { get; set; }

        public static readonly string TempFolder = Environment.GetEnvironmentVariable("TMP", EnvironmentVariableTarget.User);
        public static string APOCFolderPath = Path.Combine(TempFolder, "APOC");
        public static string SettingsPath = Path.Combine(APOCFolderPath, "Settings");
        public static string ViewportsDataPath = Path.Combine(APOCFolderPath, "Viewports");

        public static string ObseleteFamiliesError = string.Empty;
        public static string? SelectedSubsystem = string.Empty;
        public static double? InfeedElevation { get; set; }
        public static string? ConveyorNumber { get; set; }
        public static string? ProductLine { get; set; }
        public static double? OutfeedElevation { get; set; }
        public static List<FamilySymbol> ConveyerTypes => new FilteredElementCollector(Doc).OfCategory(BuiltInCategory.OST_SpecialityEquipment)
            .OfClass(typeof(FamilySymbol)).Cast<FamilySymbol>().ToList();

        public static List<IChangeStyle>? ActiveViews { get;  set; } = [];
        public static void SetupCurrentDocument(Document doc)
        {
            Doc = doc;
        }

    }
}
