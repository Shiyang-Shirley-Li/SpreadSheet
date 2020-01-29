using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System.Collections.Generic;
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
                Formula badFormula = new Formula("2x+y3", s => s.ToUpper(), s => oneDigitOneLetter.IsMatch(s));//2x separately
            }
            catch (FormulaFormatException)
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

        [TestMethod]
        public void constructorTest()
        {
            Regex oneDigitOneLetter = new Regex("^[0-9][A-Za-z]$");

            Formula goodFormula = new Formula("x2+y3", s => s.ToUpper(), s => oneDigitOneLetter.IsMatch(s));
        }

        
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        [TestMethod]
        public void test_GetVariables()
        {
            // new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z", 
            // when N is a method that converts all the letters in a string to upper case
            Formula testFormula = new Formula("x+y*z", s => s.ToUpper(), s => true);
            IEnumerator<string> variables = testFormula.GetVariables().GetEnumerator();
            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual("X", variables.Current);
            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual("Y", variables.Current);
            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual("Z", variables.Current);
        }
    }
}
