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
                Formula badFormula = new Formula("2x+y3", s => s.ToUpper(), oneDigitOneLetter.IsMatch);
            }
            catch (FormulaFormatException)
            {
                return;//why not right?
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
                Formula badFormula = new Formula("x + y3", s => s.ToUpper(), oneDigitOneLetter.IsMatch);
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

            Formula goodFormula = new Formula("x2+y3", s => s.ToUpper(), oneDigitOneLetter.IsMatch);//validate
        }

        
        
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

            // new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
            testFormula = new Formula("x+X*z");
            variables = testFormula.GetVariables().GetEnumerator();
            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual("x", variables.Current);
            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual("X", variables.Current);
            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual("z", variables.Current);
        }

        [TestMethod]
        public void test_toString()
        {
            // new Formula("x + y", N, s => true).ToString() should return "X+Y" 
            // when N is a method that converts all the letters in a string to upper case
            Assert.AreEqual("X+Y", new Formula("x + y", s => s.ToUpper(), s => true).ToString());

            // new Formula("x + Y").ToString() should return "x+Y"
            Assert.AreEqual("x+Y", new Formula("x + Y").ToString());
        }

        
        
        [TestMethod]
        public void test_Equals()
        {
            // new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
            // when N is a method that converts all the letters in a string to upper case
            Assert.IsTrue(new Formula("x1+y2", s => s.ToUpper(), s => true).Equals(new Formula("X1  +  Y2")));

            // new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
            // when N is a method that converts all the letters in a string to upper case
            Assert.IsFalse(new Formula("x1+y2").Equals(new Formula("X1+Y2")));

            // new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
            Assert.IsFalse(new Formula("x1+y2").Equals(new Formula("y2+x1")));

            // new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
            Assert.IsTrue(new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")));
        }

        [TestMethod]
        public void test_GetHashCode()
        {
            Formula f1 = new Formula("x1+y2", s => s.ToUpper(), s => true);
            Formula f2 = new Formula("X1  +  Y2");

            Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode()); //is it a good way to test?????
        }

        [TestMethod]
        public void test_OperatorEquals()
        {
            Formula f1 = new Formula("x1+y2", s => s.ToUpper(), s => true);
            Formula f2 = new Formula("X1  +  Y2");

            Assert.IsTrue(f1 == f2);

            //when two formulas are null
            Formula nullF1 = null;
            Formula nullF2 = null;

            Assert.IsTrue(nullF1 == nullF2);

            Assert.IsFalse(f1 == nullF2);
        }

        [TestMethod]
        public void test_OperatorNotEquals()
        {
            Formula f1 = new Formula("x1+y2", s => s.ToUpper(), s => true);
            Formula f2 = new Formula("X1  +  Y2");

            Assert.IsFalse(f1 != f2);

            //when two formulas are null
            Formula nullF1 = null;
            Formula nullF2 = null;

            Assert.IsFalse(nullF1 != nullF2);

            Assert.IsTrue(f1 != nullF2);
        }
    }
}
