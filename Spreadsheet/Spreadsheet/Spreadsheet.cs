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
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;

namespace SS
{
    /// <summary>
    /// This is a Cell class which defines the cell in a spreadsheet. A cell has its content. The content
    /// can be string text, double number, and formula. We can change the content of a cell, so make it
    /// public. And the name of the Cell is defined in the Spreadsheet, thus do not need it in the class
    /// for this assignment.
    /// </summary>
    public class Cell
    {
        public object content;
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
                cells.TryGetValue(cellName, out Cell cell);
                if (!(cell.content is ""))//to check if the content of the cell is empty
                {
                    namesOfAllNonemptyCells.Add(cellName);
                }
            }
            return namesOfAllNonemptyCells;
        }

        /// <summary>
        /// A helper method for checking the validity of the name of a cell
        /// </summary>
        /// <param name="name"> name of a non-empty cell</param>
        private void exceptionHelper(string name)
        {
            if (name is null || !name.isVariable())//isVariable is an extension from Formula to check the validity of variables
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
        /// <returns>A list with named cells and its direct or indirect dependent</returns>
        public override object GetCellContents(string name)
        {
            exceptionHelper(name);

            cells.TryGetValue(name, out Cell cell);
            if (cell is null)//it means the cell is without content
            {
                return "";
            }
            return cell.content;
        }

        /// <summary>
        /// This is a helper method for the three overloaded SetCellContent
        /// 
        /// </summary>
        /// <param name="name">The name of a cell</param>
        /// <param name="newContent">the new content we need to change the named cell into</param>
        /// <returns></returns>
        private ISet<string> SetCellContentsHelper(string name, object newContent)
        {
            exceptionHelper(name);

            ISet<string> nameAndItsDependents = new HashSet<string>();
            //Add the name to the dictionary when it is empty at first
            if (!cells.ContainsKey(name))
            {
                Cell emptyCell = new Cell();
                emptyCell.content = "";
                cells.Add(name, emptyCell);
            }

            if (cells[name].content is Formula)
            {
                Formula formula = (Formula)cells[name].content;
                IEnumerable<string> variableInFormula = formula.GetVariables();//Get dependees of the named cell
                foreach (string variable in variableInFormula)
                {
                    dependencyGraph.RemoveDependency(variable, name);//change the formula to a number, we need to remove previous depdency
                }
            }

            IEnumerable<String> directNIndirectDependents = GetCellsToRecalculate(name);
            foreach (String dependent in directNIndirectDependents)
            {
                nameAndItsDependents.Add(dependent);
            }
            cells[name].content = newContent;
            return nameAndItsDependents;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// </summary>
        /// <param name="name">The name of a cell</param>
        /// <param name="number">
        /// A double that we need to change the content of
        /// the named cell into
        /// </param>
        /// <returns>A list with named cells and its direct or indirect dependents</returns>
        public override ISet<string> SetCellContents(string name, double number)
        {
            return SetCellContentsHelper(name, number);
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
        /// <param name="name">The name of a cell</param>
        /// <param name="text">A text that we need to change the content of
        /// the named cell into</param>
        /// <returns>A list with named cells and its direct or indirect dependents</returns>
        public override ISet<string> SetCellContents(string name, string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException();
            }

            return SetCellContentsHelper(name, text);

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
        /// <param name="name">The name of a cell</param>
        /// <param name="formula">A formula that we need to change the content of
        /// the named cell into</param>
        /// <returns>A list with named cells and its direct or indirect dependents</returns>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (formula is null)
            {
                throw new ArgumentNullException();
            }

            exceptionHelper(name);

            IEnumerable<string> formulaVariables = formula.GetVariables();
            IEnumerable<string> dependents = GetCellsToRecalculate(name);
            foreach (string var in formulaVariables)
            {
                foreach (string str in dependents)
                {
                    if (var.Equals(str))//To check if the new formula has the named cell's direct or indirect dependents as dependees or not
                    {
                        throw new CircularException();//When it has, it is a circular dependency
                    }
                }
            }

            IEnumerable<string> variableInFormula = formula.GetVariables();
            foreach (string variable in variableInFormula)
            {
                dependencyGraph.AddDependency(variable, name);//add dependency for the new formula
            }

            return SetCellContentsHelper(name, formula);
        }

        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.
        /// </summary>
        /// <param name="name">The name of a cell</param>
        /// <returns>A list with named cells and its direct or indirect dependents</returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            IEnumerable<string> directDependentsWitoutName = dependencyGraph.GetDependents(name);
            return directDependentsWitoutName;
        }
    }
}
