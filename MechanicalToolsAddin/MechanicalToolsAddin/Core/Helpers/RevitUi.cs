using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MechanicalToolsAddin
{
    public static class RevitUi
    {
        /// <summary>
        /// get an image as bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapSource CreateBitmapSourceFromBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            return System.Windows.Interop.Imaging.
                CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        /// <summary>
        /// Add a new tab beside the main Revit tabs
        /// </summary>
        /// <param name="application"></param>
        /// <param name="TabName"></param>
        public static void AddRibbonTab(UIControlledApplication application, string TabName)
        {
            application.CreateRibbonTab(TabName);
        }

        /// <summary>
        /// Add a new panel which contain buttons
        /// </summary>
        /// <param name="application"></param>
        /// <param name="TabName"></param>
        /// <param name="panelName"></param>
        /// <returns></returns>
        public static RibbonPanel AddRibbonPanel(UIControlledApplication application, string TabName, string panelName)
        {
            return application.CreateRibbonPanel(TabName, panelName);
        }

        /// <summary>
        /// Add a new button in a current panel
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="title"></param>
        /// <param name="targetClass"></param>
        /// <param name="largeImage"></param>
        /// <param name="smallImage"></param>
        /// <param name="available"></param>
        /// <returns></returns>
        public static PushButton AddPushButton(RibbonPanel panel, string title, Type targetClass, Bitmap largeImage, Bitmap smallImage, Type available)
        {
            string path = Assembly.GetExecutingAssembly().Location;
            var button = panel.AddItem(new PushButtonData(Guid.NewGuid().ToString(), title, path, targetClass.FullName)) as PushButton;

            if (largeImage != null)
                button.LargeImage = CreateBitmapSourceFromBitmap(largeImage);

            if (smallImage != null)
                button.Image = CreateBitmapSourceFromBitmap(smallImage);

            button.AvailabilityClassName = available.FullName;

            return button;
        }

        /// <summary>
        /// Create the info for one pushButton as PushButtonData
        /// </summary>
        /// <param name="title"></param>
        /// <param name="targetClass"></param>
        /// <returns></returns>
        public static PushButtonData AddPushButtonData(string title, Type targetClass, Bitmap largeImage, Type available)
        {
            string path = Assembly.GetExecutingAssembly().Location;
            var buttonData = new PushButtonData(Guid.NewGuid().ToString(), title, path, targetClass.FullName);
            buttonData.AvailabilityClassName = available.FullName;

            if (largeImage != null)
                buttonData.LargeImage = CreateBitmapSourceFromBitmap(largeImage);
            return buttonData;
        }


        /// <summary>
        /// Create the info for one pushButton as PushButtonData
        /// </summary>
        /// <param name="title"></param>
        /// <param name="targetClass"></param>
        /// <returns></returns>
        public static PulldownButtonData AddPullDownButtonData(string name, string title)
        {
            string path = Assembly.GetExecutingAssembly().Location;
            var buttonData = new PulldownButtonData(name, title);
            return buttonData;
        }

        /// <summary>
        /// Add a new split button which is separated 
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="ButtonsData"></param>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="largeImage"></param>
        /// <param name="smallImage"></param>
        /// <returns></returns>
        public static SplitButton AddSplitButton(RibbonPanel panel, List<PushButtonData> ButtonsData, string name, string text)
        {
            SplitButtonData spData = new SplitButtonData(name, text);
            SplitButton sb = panel.AddItem(spData) as SplitButton;
            foreach (var btn in ButtonsData)
            {
                var pushBtn = sb.AddPushButton(btn);
            }
            return sb;
        }


        public static void AddStackedButtons(RibbonPanel panel)
        {
            ComboBoxData cbData = new ComboBoxData("comboBox");

            TextBoxData textData = new TextBoxData("Text Box");
            textData.Name = "Text Box";
            textData.ToolTip = "Enter some text here";
            textData.LongDescription = "This is text that will appear next to the image"
                + "when the user hovers the mouse over the control";

            IList<RibbonItem> stackedItems = panel.AddStackedItems(textData, cbData);
            if (stackedItems.Count > 1)
            {
                TextBox tBox = stackedItems[0] as TextBox;
                if (tBox != null)
                {
                    tBox.PromptText = "Enter a comment";
                    tBox.ShowImageAsButton = true;
                    tBox.ToolTip = "Enter some text";
                    // Register event handler ProcessText
                    tBox.EnterPressed += new EventHandler<Autodesk.Revit.UI.Events.TextBoxEnterPressedEventArgs>(ProcessText);
                }

                ComboBox cBox = stackedItems[1] as ComboBox;
                if (cBox != null)
                {
                    cBox.ItemText = "ComboBox";
                    cBox.ToolTip = "Select an Option";
                    cBox.LongDescription = "Select a number or letter";

                    ComboBoxMemberData cboxMemDataA = new ComboBoxMemberData("A", "Option A");
                    cboxMemDataA.GroupName = "Letters";
                    cBox.AddItem(cboxMemDataA);

                    ComboBoxMemberData cboxMemDataB = new ComboBoxMemberData("B", "Option B");
                    cboxMemDataB.GroupName = "Letters";
                    cBox.AddItem(cboxMemDataB);

                    ComboBoxMemberData cboxMemData = new ComboBoxMemberData("One", "Option 1");
                    cboxMemData.GroupName = "Numbers";
                    cBox.AddItem(cboxMemData);
                    ComboBoxMemberData cboxMemData2 = new ComboBoxMemberData("Two", "Option 2");
                    cboxMemData2.GroupName = "Numbers";
                    cBox.AddItem(cboxMemData2);
                    ComboBoxMemberData cboxMemData3 = new ComboBoxMemberData("Three", "Option 3");
                    cboxMemData3.GroupName = "Numbers";
                    cBox.AddItem(cboxMemData3);
                }
            }
        }
        public static void AddSplitButton_Old(RibbonPanel panel)
        {
            string assembly = Assembly.GetExecutingAssembly().Location;

            // create push buttons for split button drop down
            PushButtonData bOne = new PushButtonData("ButtonNameA", "Option One", assembly, "Hello.HelloOne");

            PushButtonData bTwo = new PushButtonData("ButtonNameB", "Option Two", assembly, "Hello.HelloTwo");

            PushButtonData bThree = new PushButtonData("ButtonNameC", "Option Three", assembly, "Hello.HelloThree");

            SplitButtonData sb1 = new SplitButtonData("splitButton1", "Split");
            SplitButton sb = panel.AddItem(sb1) as SplitButton;
            sb.AddPushButton(bOne);
            sb.AddPushButton(bTwo);
            sb.AddPushButton(bThree);
        }
        public static void AddRadioGroup(RibbonPanel panel)
        {
            // add radio button group
            RadioButtonGroupData radioData = new RadioButtonGroupData("radioGroup");
            RadioButtonGroup radioButtonGroup = panel.AddItem(radioData) as RadioButtonGroup;

            // create toggle buttons and add to radio button group
            ToggleButtonData tb1 = new ToggleButtonData("toggleButton1", "Red");
            tb1.ToolTip = "Red Option";
            ToggleButtonData tb2 = new ToggleButtonData("toggleButton2", "Green");
            tb2.ToolTip = "Green Option";
            ToggleButtonData tb3 = new ToggleButtonData("toggleButton3", "Blue");
            tb3.ToolTip = "Blue Option";
            radioButtonGroup.AddItem(tb1);
            radioButtonGroup.AddItem(tb2);
            radioButtonGroup.AddItem(tb3);
        }

        #region helpers
        public static void ProcessText(object sender, Autodesk.Revit.UI.Events.TextBoxEnterPressedEventArgs args)
        {
            // cast sender as TextBox to retrieve text value
            TextBox textBox = sender as TextBox;
            string strText = textBox.Value as string;
        }

        public static void LoadLibraries()
        {
            LoadLibrary("MaterialDesignThemes.Wpf.dll");
            LoadLibrary("MaterialDesignColors.dll");
            LoadLibrary("DevExpress.Xpf.Grid.v25.1.dll");
        }

        private static void LoadLibrary(string assembName)
        {
            try
            {
                string assemblyName = String.Empty;
                assemblyName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + assembName;
                if (File.Exists(assemblyName))
                {
                    Assembly.LoadFrom(assemblyName);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        #endregion

    }
}
