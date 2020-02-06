using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SS
{
    /// <summary>
    /// 
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
    public class Spreadsheet : AbstractSpreadsheet
    {

        //instance variables
        Dictionary<string, Cell> cells;//A dictionary with string of cellname as key and Cell as value
        DependencyGraph dependencyGraph;

        public Spreadsheet()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();//create an empty spreadsheet?????
            cells = new Dictionary<string, Cell>();
            dependencyGraph = new DependencyGraph();
        }

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            List<string> namesOfAllNonemptyCells = new List<string>();
            foreach (string cellName in cells.Keys)
            {
                namesOfAllNonemptyCells.Add(cellName);
            }
            return namesOfAllNonemptyCells;
        }
        private void exceptionHelper(string name)
        {
            if (name is null || name.isVariable())//how can I use the extension
            {
                throw new InvalidNameException();
            }
        }
        public override object GetCellContents(string name)
        {
            exceptionHelper(name);
            cells.TryGetValue(name, out Cell cell);
            object content = cell.getContents();

            return content;
        }

        public override IList<string> SetCellContents(string name, double number)
        {
            exceptionHelper(name);
            IList<string> nameNItsDependents = new List<string> { name };
            if (cells[name].getContents() is "" || cells[name].getContents() is string)//When the content of the cell is empty or a string, there is no dependents?????? Just return the list with this name?????
            {
                cells[name].setContents(number);
            }
            else//the content of the cell is a Formula
            {
                Formula preCellContent = (Formula)cells[name].getContents();
                IEnumerable<string> variablesInFormula = preCellContent.GetVariables();
                List<string> variablesList = variablesInFormula.ToList();

                foreach (string variable in variablesList)//how to get all the dependents
                {
                    IEnumerable<string> directDependents = new List<string>();
                    if (dependencyGraph.HasDependents(variable))
                    {
                        directDependents = dependencyGraph.GetDependents(variable);
                    }

                    foreach (string var in directDependents)
                    {

                    }
                }
            }

            return null;
        }

        public override IList<string> SetCellContents(string name, string text)
        {
            if(text is null)
            {
                throw new ArgumentNullException();
            }
            //the same step as above
            exceptionHelper(name);
            
            return null;
        }

        public override IList<string> SetCellContents(string name, Formula formula)
        {
            if(formula is null)
            {
                throw new ArgumentNullException();
            }
            //the same step as above 
            return null;
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            IEnumerable<string> directDependents = dependencyGraph.GetDependents(name);
            return directDependents;
        }


    }
}
