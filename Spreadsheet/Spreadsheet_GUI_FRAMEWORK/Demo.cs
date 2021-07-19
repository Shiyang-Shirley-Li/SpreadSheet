using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SS;

namespace SS
{
    /// <summary>
    /// Example of using a SpreadsheetPanel object
    /// </summary>
    public partial class SampleSpreadsheetForm : Form
    {

        /// <summary>
        /// Constructor for the demo
        /// </summary>
        public SampleSpreadsheetForm()
        {
            InitializeComponent();

            // This an example of registering a method so that it is notified when
            // an event happens.  The SelectionChanged event is declared with a
            // delegate that specifies that all methods that register with it must
            // take a SpreadsheetPanel as its parameter and return nothing.  So we
            // register the displaySelection method below.

            // This could also be done graphically in the designer, as has been
            // demonstrated in class.
            spreadsheetPanel1.SelectionChanged += displaySelection;
            spreadsheetPanel1.SetSelection(2, 3);
        }

        // Every time the selection changes, this method is called with the
        // Spreadsheet as its parameter.  We display the current time in the cell.

        private void displaySelection(SpreadsheetPanel ss)
        {
            int row, col;
            String value;
            ss.GetSelection(out col, out row);
            ss.GetValue(col, row, out value);
            if (value == "")
            {
                ss.SetValue(col, row, DateTime.Now.ToLocalTime().ToString("T"));
                ss.GetValue(col, row, out value);
            }
        }

        // Deals with the New menu
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same
            // thread as the other forms.
            DemoApplicationContext.getAppContext().RunForm(new SampleSpreadsheetForm());
        }

        // Deals with the Close menu
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Need To save File");
            Close();
        }
        //Open file
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //Save file
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void SampleSpreadsheetForm_Load(object sender, EventArgs e)
        {

        }
        // Contains the text box that contains the contents of the cell, and where the contents are changed
        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void CellNameBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void SampleSpreadsheetForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}