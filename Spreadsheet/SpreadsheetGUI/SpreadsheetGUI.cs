/// <summary>
/// Author:    Shirley(Shiyang) Li
/// Partner:   Ali Kergaye
/// Date:      02/18/2020 
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Ali Kergaye and Shirley(Shiyang) Li - This work may not be copied for use in Academic Coursework. 
/// 
/// We, Ali Kergaye and Shirley(Shiyang) Li, certify that we wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// This code represents a Windows Form element for a Spreadsheet. It includes
/// a Menu Bar with five operations (close/new/open/save/save as), help menu, changes menu and three textboxes to show the name, value
/// and content of the selected celss as well as the GRID of the spreadsheet.
/// The GRID is a separate class found in SpreadsheetPanel
///   
///
/// See the SpreadsheetGUI.Designer.cs for "auto-generated" code.
///   
/// This code relies on the SpreadsheetPanel "widget"
/// 
/// </summary>
using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
/// <summary>
/// the creation of the actual spreadsheet object
/// 
/// USE DEMO AS THE FRAMEWORK FOR THIS
/// </summary>
namespace SpreadsheetGUI
{
    public partial class SpreadsheetGUIForm : Form
    {
        /// <summary>
        /// creates place holders for the previous row and column, such that it won't set the contents of the cell until the cell isn't selected
        /// </summary>
        private int prevRow = 0;
        private int prevCol = 0;
        private string fileName;
        private IList<string> changed = new List<string>();
        private AbstractSpreadsheet cells = new Spreadsheet(s => true, s => s.ToUpper(), "six");
        private List<string> changes = new List<string>();
        private int changeAmount = 0;
        /// <summary>
        /// a dictionary that stores the column name being worked on to the letter: 1 = A, 2 = B, 3 = C, etc.
        /// </summary>
        private Dictionary<int, string> column = new Dictionary<int, string>();
        //creates a reversed dictionary for the columns
        private Dictionary<string, int> revColumn = new Dictionary<string, int>();

        /// <summary>
        /// edits the dictionary column so that it contains the column name being worked on to the letter associated with it
        /// </summary>
        /// <returns></returns>
        private void columnMaker()
        {
            //stores in the dictionary each letter so that it has sets containing the column name associated with the int from 1-26 wtih A-Z
            for (int i = 0; i < 26; i++)
            {
                char col = (char)(65 + i);
                column.Add(i, col.ToString());
                revColumn.Add(col.ToString(), i);
            }
        }
        public SpreadsheetGUIForm()
        {
            //makes a dictionary<int,string> which stores the column name associated with the int from 1-26 wtih A-Z
            columnMaker();
            InitializeComponent();
            // This an example of registering a method so that it is notified when
            // an event happens.  The SelectionChanged event is declared with a
            // delegate that specifies that all methods that register with it must
            // take a SpreadsheetPanel as its parameter and return nothing.  So we
            // register the displaySelection method below.

            // This could also be done graphically in the designer, as has been
            // demonstrated in class.
            spreadsheetPanel1.SelectionChanged += displaySelection;
            spreadsheetPanel1.SetSelection(0, 0);
        }
        // Every time the selection changes, this method is called with the
        // Spreadsheet as its parameter. display the value of that cell

        private void displaySelection(SpreadsheetPanel ss)
        {
            // if still computing, nothing will happen
            if (bg_worker.IsBusy)
            {
                return;
            }
            int row, col;
            String value;
            ss.GetSelection(out col, out row);
            ss.GetValue(col, row, out value);
            // if we are on a different cell/ have left the previous cell, it will set the contents of the cell
            // else, does nothing
            if (row != prevRow || col != prevCol)
            {
                //a string containing the name of the previous cell, the cell whose value is changing
                string name = column[prevCol] + (prevRow + 1);
                //if there was no change to the value, move to new cell and nothing else
                if (cells.GetCellContents(name).ToString() == toolStripTextBox1.Text)
                {
                    // makes it so this cell is now the 'previous' cell
                    prevRow = row;
                    prevCol = col;

                    //overrides the name to be the new cell, so that it's contents can be put into the text box.
                    name = column[prevCol] + (prevRow + 1);
                    // set the text box content to what is currently stored in the newly selected cell.
                    textUpdate(name);
                    return;
                }
                // sets the value of that cell to what is currently in the text box
                //updates other cells connected to this cell
                try
                {
                    // if there is a formula format exception, catch it and create a message.
                    changed = cells.SetContentsOfCell(name, toolStripTextBox1.Text);

                }
                catch (FormulaFormatException)
                {
                    MessageBox.Show("Invalid Formula Entered By User: " + toolStripTextBox1.Text + '\n' + "Please Enter Valid Value");
                    return;
                }
                bg_worker.RunWorkerAsync();

                // displays the value of the changed cell
                ss.SetValue(prevCol, prevRow, cells.GetCellValue(name).ToString());
                //stores the change
                changes.Insert(0, name + "  :  " + cells.GetCellContents(name).ToString());
                changeAmount++;
                //if there's more than 10 changes, remove the extra ones
                if (changes.Count == 11)
                {
                    changes.RemoveAt(10);
                }
                // makes it so this cell is now the 'previous' cell
                prevRow = row;
                prevCol = col;

                //overrides the name to be the new cell, so that it's contents can be put into the text box.
                name = column[prevCol] + (prevRow + 1);
                // set the text box content to what is currently stored in the newly selected cell.
                textUpdate(name);
            }
        }

        /// <summary>
        /// Additional feature: press Enter key to display the edited cell value and move to
        /// the next cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterKeys(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && prevRow < 98)
            {
                this.spreadsheetPanel1.SetSelection(prevCol, prevRow + 1);

            }
            displaySelection(this.spreadsheetPanel1);
        }

        /// <summary>
        /// updates the textbox when clicking on a cell
        /// </summary>
        /// <param name="name"></param>
        private void textUpdate(string name)
        {
            object contents = cells.GetCellContents(name);
            nameBox.Text = "Cell: " + name;
            valueBox.Text = "Value: " + cells.GetCellValue(name).ToString();
            if (contents is Formula)
            {
                toolStripTextBox1.Text = "=" + contents;
                return;
            }
            toolStripTextBox1.Text = cells.GetCellContents(name).ToString();
        }
        /// <summary>
        /// updates cells that relied on this cell
        /// </summary>
        private void changedCells(object sender, DoWorkEventArgs e)
        {
            foreach (string name in changed)
            {// gets the numbers of the row and column being updated
                string colName = name.Substring(0, 1);
                int col = revColumn[colName];
                int row = Int32.Parse(name.Substring(1)) - 1;
                //updates those spots
                this.Invoke(new MethodInvoker(() =>
                {
                    spreadsheetPanel1.SetValue(col, row, cells.GetCellValue(name).ToString());

                }));
            }

        }
        private void SpreadsheetGUIForm_Load(object sender, EventArgs e)
        {

        }
        // Deals with the New menu
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same
            // thread as the other forms.

            SpreadsheetApplicationContext.getAppContext().RunForm(new SpreadsheetGUIForm());
        }

        private void SpreadsheetGUIForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cells.Changed)
            {
                //save it
                saveToolStripMenuItem_Click(sender, e);
            }
            else
            {
                return;
            }

            if (cells.Changed)
            {
                DialogResult result = MessageBox.Show("You have unsaved changes in this spreedsheet " +
                   "Would you like to close without saving?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                // if 'No' then we don't want to open the file so return
                if (result == DialogResult.Yes)
                    return;

            }
            e.Cancel = (true);

        }
        // Deals with the Close menu
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Close();
        }
        //Open file
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //makes the spreadsheet have the values of the saved file.
            //have to figure out how to let the user choose a file.
            // Referenced MSDN library for Filter syntax
            openFileDialog1.Filter = "Spreadsheet Files (*.sprd)|*.sprd|All Files (*.*)|*.*";
            openFileDialog1.DefaultExt = ".sprd";
            openFileDialog1.FileName = "";
            openFileDialog1.Title = "Open";
            openFileDialog1.ShowDialog();

            // get the name of the selected file
            string nameOfFile = openFileDialog1.FileName;
            //if the filename is empty, do nothing
            if (nameOfFile == "")
            {
                return;
            }

            // if i can't open that file type, explain why
            if (openFileDialog1.DefaultExt != "sprd")
            {
                MessageBox.Show("Unable to open file of Type " + openFileDialog1.DefaultExt);
                return;
            }
            //creates error message if trying to leave when there's a change.
            if (cells.Changed)
            {
                DialogResult result = MessageBox.Show("Your spreadsheet has unsaved changed. Opening " +
                    "this file will overwrite any changes that have not been saved. Open anyways?",
                    "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // if 'No' then we don't want to open the file so return
                if (result == DialogResult.No)
                    return;
            }
            spreadsheetPanel1.Clear();
            cells = new Spreadsheet(nameOfFile, cells.IsValid, cells.Normalize, cells.Version);
            // updates the visual information/graphics to display the cell info.
            foreach (string name in cells.GetNamesOfAllNonemptyCells())
            {
                // gets the column and row of the cell
                int col = revColumn[name.Substring(0, 1)];
                int row = Int32.Parse(name.Substring(1)) - 1;
                // sets the value of the cell
                spreadsheetPanel1.SetValue(col, row, cells.GetCellValue(name).ToString());
            }
            //sets the textbox value as the selected cell
            string name2 = column[prevCol] + (prevRow + 1);
            toolStripTextBox1.Text = cells.GetCellContents(name2).ToString();
        }

        //Save file
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // if there is no filename, use the Save As method
            if (ReferenceEquals(fileName, null))
                saveAsToolStripMenuItem_Click(sender, e);

            //if the Save As was cancelled, don't save.
            if (ReferenceEquals(fileName, null))
            {
                return;
            }
            //since the save wasn't canceled and we have a location, save the spreadsheet
            cells.Save(fileName);
        }


        // Contains the text box that contains the contents of the cell, and where the contents are changed
        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// when enter is clicked from the text-box, moves down a row and calculates values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (prevRow < 99)
                {
                    this.spreadsheetPanel1.SetSelection(prevCol, prevRow + 1);
                    // change the display to accomodate.
                    displaySelection(this.spreadsheetPanel1);
                }
            }
        }
        // when return is done in the text box

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make the user choose a file name
            saveFileDialog1.Filter = "Spreadsheet Files (*.sprd)|*.sprd|All Files (*.*)|*.*";
            saveFileDialog1.DefaultExt = ".sprd";
            saveFileDialog1.Title = "Save";
            saveFileDialog1.ShowDialog();

            // get the name of the selected file
            string savename = saveFileDialog1.FileName;

            // if the file name is empty, return
            if (savename == "")
                return;

            // do a check here to make sure the user wants to overwrite an existing file
            DialogResult result = MessageBox.Show("Saving this spreadsheet to " + savename +
                    " will overwrite any data contained in that file. Save anyways?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            // if 'No' then we don't want to open the file so return
            if (result == DialogResult.No)
                return;

            this.Text = savename; // set the name of the document to the saved name

            // if the user selected option 1 (needs to be .sprd) then if the selected
            // filename does not end with .sprd, append it to the end
            if (saveFileDialog1.FilterIndex == 1)
                saveFileDialog1.AddExtension = true;

            // once we have a proper file name, save the spreadsheet to that file
            cells.Save(savename);

            // once we save, set fileName to where we saved the spreadsheet
            fileName = savename;
        }

        /// <summary>
        /// the button with the help menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Here are some helpers to make you use spreadsheet easier:" +
                            "\n" +
                            "\n- File menu: " +
                            "\n-             new: create a new empty spreasheet" +
                            "\n-             close: remind you to save if have changes and close the window" +
                            "\n-             open: open a spreadsheet from a file replacing the current spreadsheet" +
                            "\n-             save: save the current spreadsheet to file" +
                            "\n-             save as: save the current spreadsheet as the choosen extension" +
                            "\n- Change selections: mouse click on the cell " +
                            "\n- Input data: select a cell, enter content only through the green box and then" +
                            "\n              press Enter to show the value and move selection to the next cell" +
                            "\n- Select data:select a cell and the cell name, value and content will show above" +
                            "\n- Changes:    show the changes made in the spreadsheet" +
                            "\n- Formulas recalculate automagically." +
                            "\n- Formulas must begin with an equals sign." +
                            "\n- The only valid operators are + - * / and ()"
                        );
        }

        /// <summary>
        /// Additinal features: the button with the changes menu
        /// Give the info on number of number of changes that has made in 
        /// the current spreadsheet and also list the changed cells as well as its content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changedList_Click(object sender, EventArgs e)
        {
            //puts the amount of changes currently made
            string total = "Total Changes: " + changeAmount + '\n';
            // creates a string of lines representing the last ten changes
            foreach (string change in changes)
            {
                total += change + '\n';
            }
            //create a message with the change info.
            MessageBox.Show(total);
        }
    }
}
