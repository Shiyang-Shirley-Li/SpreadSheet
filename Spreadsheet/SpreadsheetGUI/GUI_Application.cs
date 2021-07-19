/// <summary>
///  Author:     Joe Zachary
///  Updated by: Jim de St. Germain
///  
///  Dates:      (Original) 2012-ish
///              (Updated for Core) 2020
///              
///  Target: ASP Framework (and soon ASP CORE 3.1)
///  
///  This program is an example of how to create a GUI application for 
///  a spreadsheet project.
///  
///  It relies on a working Spreadsheet Panel class, but defines other
///  GUI elements, such as the file menu (open and close operations).
///  
///  WARNING: with the current GUI designer, you will not be able to
///           use the toolbox "Drag and Drop" to update this file.
/// </summary>
using System;
using System.Windows.Forms;

namespace SpreadsheetGUI
{

    /// <summary>
    /// reworking the demo program skeleton
    /// </summary>
    class SpreadsheetApplicationContext : ApplicationContext
    {
        // Number of open forms
        private int formCount = 0;

        // Singleton ApplicationContext
        private static SpreadsheetApplicationContext appContext;

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private SpreadsheetApplicationContext()
        {

        }

        /// <summary>
        /// Returns the one DemoApplicationContext.
        /// </summary>
        public static SpreadsheetApplicationContext getAppContext()
        {
            if (appContext == null)
            {
                appContext = new SpreadsheetApplicationContext();
            }
            return appContext;
        }

        /// <summary>
        /// Runs the form
        /// </summary>
        public void RunForm(Form form)
        {
            // One more form is running
            formCount++;

            // When this form closes, we want to find out
            form.FormClosed += (o, e) => { if (--formCount <= 0) ExitThread(); };

            // Run the form
            form.Show();
        }

    }
   class A6GUI
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start an application context and run one form inside it
            SpreadsheetApplicationContext appContext = SpreadsheetApplicationContext.getAppContext();
            appContext.RunForm(new SpreadsheetGUIForm());
            Application.Run(appContext);
        }
    }
}
