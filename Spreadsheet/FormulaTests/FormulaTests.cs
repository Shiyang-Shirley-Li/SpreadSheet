using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;

namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void constructor_syntacticallyIncorrectExpressionTest()
        {
            Regex oneDigitOneLetter = new Regex("[0-9][A-Za-z]");
            
            try
            {
                Formula badFormula = new Formula("2x+y3", s => s.ToUpper(), s => oneDigitOneLetter.IsMatch(s));
            }
            catch(FormulaFormatException)
            {
                return;
            }
            Assert.Fail("Syntactically Correct Expression.");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void constructor_legalVariableTest()
        {
            Regex oneDigitOneLetter = new Regex("^[0-9][A-Za-z]$");

            try
            {
                Formula badFormula = new Formula("x + y3", s => s.ToUpper(), s => oneDigitOneLetter.IsMatch(s));
            }
            catch (FormulaFormatException)
            {
                return;
            }
            Assert.Fail("Legal Variable.");
        }
    }
}
