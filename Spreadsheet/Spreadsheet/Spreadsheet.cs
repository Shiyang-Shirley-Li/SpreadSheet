using SpreadsheetUtilities;
using System;
using System.Collections.Generic;

namespace SS
{
    /// <summary>
    /// 
    /// </summary>
    public class  Cell
    {
        private string name;
        private object contents;
        private double value;

        public string getName()
        {
            return name;
        }

        public void setContents(object _contents)
        {
            contents = _contents;
        }

        public void setValue(double _value)
        {
            value = _value;
        }
    }
    public class Spreadsheet : AbstractSpreadsheet
    {
        
        //instance variables
        

        public Spreadsheet()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();//create an empty spreadsheet
        }

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            throw new NotImplementedException();
        }

        public override object GetCellContents(string name)
        {
            throw new NotImplementedException();
        }

        public override IList<string> SetCellContents(string name, double number)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
