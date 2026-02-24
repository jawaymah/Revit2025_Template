using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MechanicalToolsAddin.Core.UI;
using Serilog;
using RibbonItem = Autodesk.Revit.UI.RibbonItem;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

namespace MechanicalToolsAddin
{
    public class App : IExternalApplication
    {
        const string TabName = "Advansys";
        const string ManagerPanelName = "Mech Tools";
        private static UIControlledApplication CurrentApplication { get; set; }
        public Result OnStartup(UIControlledApplication a)
        {
            try
            {
                // Create a custom tab
                RibbonCreator.CreatebbonTab(a, TabName);
                CurrentApplication = a;
                a.ThemeChanged += A_ThemeChanged;

            }
            catch (Exception ex)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Error", ex.Message);
                Log.Error(ex, $"Exception in Startup:");
                return Result.Failed;
            }

            return Result.Succeeded;
        }

        private void A_ThemeChanged(object? sender, Autodesk.Revit.UI.Events.ThemeChangedEventArgs e)
        {
            try
            {
                //if (Globals.ActiveViews != null && Globals.ActiveViews.Any())
                //    Globals.ActiveViews.ForEach(v => v.SetBaseStyle());
            }
            catch (Exception)
            {
            }
        }
        public Result OnShutdown(UIControlledApplication a)
        {

            return Result.Succeeded;
        }
    }


    class DocumentAvailablility : IExternalCommandAvailability
    {
        public virtual bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            if (applicationData.ActiveUIDocument != null && applicationData.ActiveUIDocument.Document != null && !applicationData.ActiveUIDocument.Document.IsFamilyDocument)
                return true;
            return false;
        }
    }
    class IsDirectAvailability : DocumentAvailablility
    {
        public override bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            if (base.IsCommandAvailable(applicationData, selectedCategories))
                return true;
            return false;
        }
    }

    class DisabledAvailability : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            return false;
        }
    }
}
