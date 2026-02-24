using Autodesk.Revit.UI;
using MechanicalToolsAddin.Properties;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;


namespace MechanicalToolsAddin
{
    internal static class RibbonCreator
    {
        public static void CreatebbonTab(UIControlledApplication app, string tabName)
        {
            app.CreateRibbonTab(tabName);

            AddRibbonPanels(app, tabName);
        }


        static void AddRibbonPanels(UIControlledApplication application, string tabName)
        {
            CreateValidateRibbonPanel(application, tabName);
        }


        static RibbonPanel CreateRibbonPanel(UIControlledApplication application, string tabName, string panelName)
        {
            return application.CreateRibbonPanel(tabName, panelName);
        }

        static void CreateValidateRibbonPanel(UIControlledApplication application, string tabName)
        {
            RibbonPanel panel = CreateRibbonPanel(application, tabName, "Validate");

            // Single push button instead of dropdown
            panel.AddItem(RevitUi.AddPushButtonData("Hand Audit",typeof(FamilyCheckLinesDirectionsCommand), Resources.info,typeof(DocumentAvailablility)));
        }

    }
}
