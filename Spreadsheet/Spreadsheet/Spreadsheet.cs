using SpreadsheetUtilities;
using System;
using System.Collections.Generic;

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
        Dictionary<string, Cell> cells;
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
            cells.TryGetValue(name, out Cell content);

            return content;
        }

        public override IList<string> SetCellContents(string name, double number)
        {
            exceptionHelper(name);
            cells[name].setContents(number);

            return null;
        }

        public override IList<string> SetCellContents(string name, string text)
        {
            throw new NotImplementedException();
        }

        public override IList<string> SetCellContents(string name, Formula formula)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            IEnumerable<string> directDependents = dependencyGraph.GetDependents(name);
            return directDependents;
        }


    }
}
