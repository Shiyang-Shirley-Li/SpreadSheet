using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Author: Shiyang(Shirley) Li
/// Date:02/03/2020
/// Course: CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Shiyang(Shirley) Li - This work may not be copied for use in Academic Coursework.
/// 
/// I, Shiyang(Shirley) Li, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// This file is a method that build the "internals" of the spreadsheet
/// 
/// </summary>
/// 
namespace SS
{
    /// <summary>
    /// This is a Cell class which defines the cell in a spreadsheet. A cell has a name and a content. The content
    /// can be string text, double number, and formula. We cannot change the name of a cell, but can change its
    /// content.
    /// </summary>
    public class Cell
    {
        private string name;
        private object contents;

        public string getName()
        {
            return name;
        }

        public void setContents(object _contents)
        {
            contents = _contents;
        }

        public object getContents()
        {
            return contents;
        }
    }
    /// <summary>
    /// This is a Spreadsheet class that implements the AbstractSpreadsheet project. A spreadsheet 
    /// consists of an infinite number of named cells.
    /// 
    /// A string is a valid cell name if and only if:
    ///   (1) its first character is an underscore or a letter
    ///   (2) its remaining characters (if any) are underscores and/or letters and/or digits
    ///   
    /// A spreadsheet contains a cell corresponding to every possible cell name. In addition to 
    /// a name, each cell has a contents and a value. 
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty. 
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.
    /// 
    /// For this specific spreadsheet, I will use the data structure, Dictionary, to implement
    /// the cells in the spreadsheet, the key will be the name of cell, and the value will be
    /// a Cell that contains its content. Then I can get the name of the cell by calling the 
    /// key of the dictionary, and also I cannot change a key of dictionary. In addition, I 
    /// can get the content of the cell(value in the dictionary) by its name(key).
    /// 
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {

        //instance variables
        Dictionary<string, Cell> cells;//A dictionary with string of cellname as key and Cell as value
        DependencyGraph dependencyGraph;

        //Constructor for spreadsheet
        public Spreadsheet()
        {
            cells = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet
        /// </summary>
        /// <returns> A list of names of all the non-empty cells</returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            List<string> namesOfAllNonemptyCells = new List<string>();
            foreach (string cellName in cells.Keys)
            {
                namesOfAllNonemptyCells.Add(cellName);
            }
            return namesOfAllNonemptyCells;
        }

        /// <summary>
        /// A helper method for checking the validity of the name of a cell
        /// </summary>
        /// <param name="name"> name of a non-empty cell</param>
        private void exceptionHelper(string name)
        {
            if (name is null || name.isVariable())//isVariable is an extension from Formula to check the validity of variables
            {
                throw new InvalidNameException();
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        /// <param name="name">name of a non-empty cell</param>
        /// <returns></returns>
        public override object GetCellContents(string name)
        {
            exceptionHelper(name);
            cells.TryGetValue(name, out Cell cell);
            object content = cell.getContents();

            return content;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// </summary>
        /// <param name="name">The name of a non-empty cell</param>
        /// <param name="number">
        /// A double that we need to change the content of
        /// the named cell into
        /// </param>
        /// <returns>A list with named cells and its direct or indirect dependents</returns>
        public override IList<string> SetCellContents(string name, double number)
        {
            exceptionHelper(name);
            IList<string> nameNItsDependents = new List<string> { name };
            if (cells[name].getContents() is "" || cells[name].getContents() is string)//When the content of the cell is empty or a string, there is no dependents?????? Just return the list with this name?????
            {
                cells[name].setContents(number);
                //should I add it to the dg, since it doesn't have dependents?????????????? 
            }
            else//the content of the cell is a Formula
            {
                Formula preCellContent = (Formula)cells[name].getContents();
                IEnumerable<string> variablesInFormula = preCellContent.GetVariables();
                List<string> variablesList = variablesInFormula.ToList();
                //Do I need to add dependency here??? How to get its dependents????? 
            }

            return null;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// </summary>
        /// <param name="name">The name of a non-empty cell</param>
        /// <param name="text">A text that we need to change the content of
        /// the named cell into</param>
        /// <returns>A list with named cells and its direct or indirect dependents</returns>
        public override IList<string> SetCellContents(string name, string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException();
            }
            //the same step as above
            exceptionHelper(name);
            //remove dependency?? Will it cause error, since there are dependents? replace to blank // use recalculate here
            //
            return null;
        }

        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException, and no change is made to the spreadsheet.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// </summary>
        /// <param name="name">The name of a non-empty cell</param>
        /// <param name="formula">A formula that we need to change the content of
        /// the named cell into</param>
        /// <returns>A list with named cells and its direct or indirect dependents</returns>
        public override IList<string> SetCellContents(string name, Formula formula)
        {
            if (formula is null)
            {
                throw new ArgumentNullException();
            }
            //the same step as above 
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            IEnumerable<string> directDependentsWitoutName = dependencyGraph.GetDependents(name);
            LinkedList<string> directDependents = directDependentsWitoutName.ToList();
            directDependents.AddFirst(name);//????????
            return directDependents;
        }
    }
}
