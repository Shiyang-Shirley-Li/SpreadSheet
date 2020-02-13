/// <summary>
/// Author: Shiyang(Shirley) Li
/// Date:02/10/2020
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
using System.Text.RegularExpressions;

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
        public string content;
    }

    /// <summary>
    /// <para>
    /// This is a Spreadsheet class that implements the AbstractSpreadsheet project. A spreadsheet 
    /// consists of an infinite number of named cells.
    /// </para>
    /// <para>
    /// A string is a valid cell name if and only if:
    /// </para>
    /// <list type="number">
    ///      <item> it starts with one or more letters</item>
    ///      <item> it ends with one or more numbers (digits)</item>
    /// </list> 
    /// 
    /// /// <para>
    ///     Any valid incoming cell name, whether passed as a parameter or embedded in a formula,
    ///     must be normalized with the Normalize method before it is used by or saved in 
    ///     this spreadsheet.  For example, if Normalize is s => s.ToUpper(), then
    ///     the Formula "x3+a5" should be converted to "X3+A5" before use.
    /// </para>
    /// 
    /// <para>
    /// A spreadsheet contains a cell corresponding to every possible cell name. In addition to 
    /// a name, each cell has a contents and a value. 
    /// </para>
    /// 
    /// <para>
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty. 
    /// </para>
    /// 
    /// <para>
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// </para>
    /// 
    /// <para>
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.
    /// </para>
    /// 
    /// <para>
    /// For this specific spreadsheet, I will use the data structure, Dictionary, to implement
    /// the cells in the spreadsheet, the key will be the name of cell, and the value will be
    /// a Cell that contains its content. Then I can get the name of the cell by calling the 
    /// key of the dictionary, and also I cannot change a key of dictionary. In addition, I 
    /// can get the content of the cell(value in the dictionary) by its name(key).
    /// </para>
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// To check if a variable is in the format of one or more letters followed
        /// by one or more digits
        /// </summary>
        /// <param name="varialbe">The variable need to be chekced</param>
        /// <returns>true it is in the right format, otherwise, false</returns>
        private bool isVariable(string varialbe)
        {
            Regex variableFormat = new Regex("^[a-z A-Z]+[0-9]+$");
            if (variableFormat.IsMatch(varialbe))
            {
                return true;
            }
            return false;
        }

        //instance variables
        Dictionary<string, Cell> cells;//A dictionary with string of cellname as key and Cell as value
        DependencyGraph dependencyGraph;
        private bool changed;//track if the spreadsheet is changed or not

        /// <summary>
        /// Zero-argument constructor for spreadsheet that imposes no extra validity conditions, normalizes
        /// every cell name to itself, and use the name "default" as the version.
        /// </summary>
        public Spreadsheet()
            : this(s => true, s => s, "default")
        {
        }

        /// <summary>
        /// A three-argument constructor for spreadsheet
        /// </summary>
        /// <param name="isValid">a validity delegate</param>
        /// <param name="normalize">a normalization delegate</param>
        /// <param name="version">version</param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {
            cells = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();
            changed = false;
        }

        /// <summary>
        /// A four-argument constructor for spreadsheet
        /// </summary>
        /// <param name="filePath">a string representing a path to a file</param>
        /// <param name="isValid">validity delegate</param>
        /// <param name="normalize">normalization delegate</param>
        /// <param name="version">version</param>
        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version)
        : base(isValid, normalize, version)
        {
            cells = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();
            changed = false;

            //Check version exception
            if (!GetSavedVersion(filePath).Equals(version))
            {
                throw new SpreadsheetReadWriteException("The version is wrong!");
            }

            //Check open and read file exception
            
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
        /// <param name="name"> name of a cell</param>
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
        /// <param name="name">name of a cell</param>
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
        /// <returns>A list of the name and its direct and indirect dependents in order</returns>
        private IList<string> SetCellContentsHelper(string name, object newContent)
        {
            IList<string> nameAndItsDependents = new List<string>();
            //Add the name to the dictionary when it is empty at first
            if (!cells.ContainsKey(name))
            {
                Cell emptyCell = new Cell();
                emptyCell.content = "";
                cells.Add(name, emptyCell);
            }
            
            if (cells[name].content[0] == '=' )
            {
                Formula formula = new Formula(cells[name].content, Normalize, IsValid);
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

            IList<string> nameAndItsDependentsInOrder = new List<string>();//Create a list to store name and its dependents in right order
            for (int i = nameAndItsDependents.Count; i >= 0; i--)
            {
                nameAndItsDependentsInOrder.Add(nameAndItsDependents[i]);
            }

            cells[name].content = newContent.ToString();
            return nameAndItsDependentsInOrder;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell in order.
        /// </summary>
        /// <param name="name">The name of a cell</param>
        /// <param name="number">
        /// A double that we need to change the content of
        /// the named cell into
        /// </param>
        /// <returns>A list with named cells and its direct or indirect dependents in correct order</returns>
        protected override IList<string> SetCellContents(string name, double number)
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
        /// directly or indirectly, on the named cell in order.
        /// 
        /// </summary>
        /// <param name="name">The name of a cell</param>
        /// <param name="text">A text that we need to change the content of
        /// the named cell into</param>
        /// <returns>A list with named cells and its direct or indirect dependents in correct order</returns>
        protected override IList<string> SetCellContents(string name, string text)
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
        /// directly or indirectly, on the named cell in order.
        /// </summary>
        /// <param name="name">The name of a cell</param>
        /// <param name="formula">A formula that we need to change the content of
        /// the named cell into</param>
        /// <returns>A list with named cells and its direct or indirect dependents in correct order</returns>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            if (formula is null)
            {
                throw new ArgumentNullException();
            }

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
            if(name is null)
            {
                throw new ArgumentNullException();
            }

            if(name is null || !name.isVariable())
            {
                throw new InvalidNameException();
            }
            IEnumerable<string> directDependentsWitoutName = dependencyGraph.GetDependents(name);
            return directDependentsWitoutName;
        }

        /// <summary>
        ///   <para>Sets the contents of the named cell to the appropriate value. </para>
        ///   <para>
        ///       First, if the content parses as a double, the contents of the named
        ///       cell becomes that double.
        ///   </para>
        ///
        ///   <para>
        ///       Otherwise, if content begins with the character '=', an attempt is made
        ///       to parse the remainder of content into a Formula.  
        ///       There are then three possible outcomes:
        ///   </para>
        ///
        ///   <list type="number">
        ///       <item>
        ///           If the remainder of content cannot be parsed into a Formula, a 
        ///           SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       </item>
        /// 
        ///       <item>
        ///           If changing the contents of the named cell to be f
        ///           would cause a circular dependency, a CircularException is thrown,
        ///           and no change is made to the spreadsheet.
        ///       </item>
        ///
        ///       <item>
        ///           Otherwise, the contents of the named cell becomes f.
        ///       </item>
        ///   </list>
        ///
        ///   <para>
        ///       Finally, if the content is a string that is not a double and does not
        ///       begin with an "=" (equal sign), save the content as a string.
        ///   </para>
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException"> 
        ///   If the content parameter is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name parameter is null or invalid, throw an InvalidNameException
        /// </exception>
        ///
        /// <exception cref="SpreadsheetUtilities.FormulaFormatException"> 
        ///   If the content is "=XYZ" where XYZ is an invalid formula, throw a FormulaFormatException.
        /// </exception>
        /// 
        /// <exception cref="CircularException"> 
        ///   If changing the contents of the named cell to be the formula would 
        ///   cause a circular dependency, throw a CircularException.  
        ///   (NOTE: No change is made to the spreadsheet.)
        /// </exception>
        /// 
        /// <param name="name"> The cell name that is being changed</param>
        /// <param name="content"> The new content of the cell</param>
        /// 
        /// <returns>
        ///       <para>
        ///           This method returns a list consisting of the passed in cell name,
        ///           followed by the names of all other cells whose value depends, directly
        ///           or indirectly, on the named cell. The order of the list MUST BE any
        ///           order such that if cells are re-evaluated in that order, their dependencies 
        ///           are satisfied by the time they are evaluated.
        ///       </para>
        ///
        ///       <para>
        ///           For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///           list {A1, B1, C1} is returned.  If the cells are then evaluate din the order:
        ///           A1, then B1, then C1, the integrity of the Spreadsheet is maintained.
        ///       </para>
        /// </returns>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            if(Double.TryParse(content, out double val))
            {
                return SetCellContents(name, val);
            }
            else if(content[0] == '=')
            {
                object remainingContent = content.Substring(1, content.Length);
                if()//how can I check if the content can be parsed into a Formula or not?????????????
                {
                    throw new SpreadsheetUtilities.FormulaFormatException("This is not in correct formula format");
                }
                else{
                    Formula formula = (Formula)remainingContent;
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
                }
                Formula correctFormula = (Formula)remainingContent;
                return SetCellContents(name, correctFormula);
            }
            return SetCellContents(name, content);
        }

        //Do I need these methods here???????????????????????
        public override bool Changed { get; protected set; }

        public Func<string, bool> IsValid { get; protected set; }

        public Func<string, string> Normalize { get; protected set; }

        public string Version { get; protected set; }

        public override string GetSavedVersion(string filename)
        {
            throw new NotImplementedException();
        }

        public override void Save(string filename)
        {
            throw new NotImplementedException();
        }

        public override object GetCellValue(string name)
        {
            throw new NotImplementedException();
        }
    }
}
