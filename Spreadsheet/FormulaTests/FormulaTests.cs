using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// Author: Shiyang(Shirley) Li
/// Date:01/27/2020
/// Copyright: Shiyang(Shirley) Li - This work may not be copied for use in Academic Coursework.
/// 
/// I, Shiyang(Shirley) Li, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the code are cited in my README file. 
/// 
/// This file is a test to test the Formula project
/// 
/// </summary>
namespace FormulaTests
{
    /// <summary>
    ///This is a test class for Formula and is intended
    ///to contain all Formula Unit Tests
    /// </summary>
    [TestClass]
    public class FormulaTests
    {
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void constructor_syntacticallyIncorrectExpressionTest()
        {
            Regex oneDigitOneLetter = new Regex("[0-9][A-Za-z]");
            Formula badFormula = new Formula("2x+y3", s => s.ToUpper(), oneDigitOneLetter.IsMatch);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void constructor_legalVariableTest()
        {
            Regex oneDigitOneLetter = new Regex("^[0-9][A-Za-z]$");
            Formula badFormula = new Formula("x + y3", s => s.ToUpper(), oneDigitOneLetter.IsMatch);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void constructor_EmptyFormulaTest()
        {
            Formula badFormula = new Formula("");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void constructor_WrongStartingTokenTest()
        {
            Formula badFormula = new Formula("+");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void constructor_WrongEndingTokenTest()
        {
            Formula badFormula = new Formula(" 1 + ");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void constructor_MissingParenthesisTest()
        {
            Formula badFormula = new Formula(" ( +");
        }

        public bool isValid(string str)
        {
            Regex oneDigitOneLetter = new Regex("[A-Za-z][0-9]");
            if (oneDigitOneLetter.IsMatch(str))
            {
                return true;
            }
            return false;
        }
        [TestMethod]
        public void constructorTest()
        {
            Regex oneDigitOneLetter = new Regex("[0-9][A-Za-z]");
            Formula goodFormula = new Formula("x2+y3", s => s.ToUpper(), isValid);
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
        public void test_GetDuplicatedVariables()
        {
            Formula f = new Formula("X1 + X1 + Y2 / Y2");
            IEnumerator<string> variables = f.GetVariables().GetEnumerator();
            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual("X1", variables.Current);
            Assert.IsTrue(variables.MoveNext());
            Assert.AreEqual("Y2", variables.Current);
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

            //when the pass in object is null
            Assert.IsFalse((new Formula("2 + 7").Equals(null)));
        }

        [TestMethod]
        public void test_GetHashCode()
        {
            Formula f1 = new Formula("x1+y2", s => s.ToUpper(), s => true);
            Formula f2 = new Formula("X1  +  Y2");

            Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
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

            Assert.IsFalse(nullF1 == f1);
        }

        /// <summary>
        /// A function that has a string prameter and returns a double
        /// </summary>
        /// <param name="variableName">
        /// a string 
        /// </param>
        /// <returns>If found, return a double; if not, throw an exception</returns>
        static double searchForAValue(string variableName)
        {
            if (variableName.Equals("x"))
            {
                return 2;
            }
            else if (variableName.Equals("X"))
            {
                return 4;
            }
            throw new ArgumentException("No such variable!");

        }

        [TestMethod]
        public void test_variableAddDouble()
        {
            // "x" is 2, "X" is 4, and N is a method that converts all the letters 
            // in a string to upper case
            Formula f1 = new Formula("x+7", s => s.ToUpper(), s => true);
            Assert.AreEqual((double)11, f1.Evaluate(searchForAValue));

            Formula f2 = new Formula("x+7");
            Assert.AreEqual((double)9, f2.Evaluate(searchForAValue));

            //FormulaError when the variable is not found in the lookup
            Formula f3 = new Formula("a + 7");
            Assert.AreEqual("No variables found in my library!", ((FormulaError)f3.Evaluate(searchForAValue)).Reason);

        }

        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 

        [TestMethod]
        public void twoNumsPlusTest()
        {
            Formula f = new Formula("5 + 4");
            Assert.AreEqual((double)9, f.Evaluate(null));
        }

        [TestMethod]
        public void twoNumsMinusTest()
        {
            Formula f = new Formula("5 - 4");
            Assert.AreEqual((double)1, f.Evaluate(null));
        }

        [TestMethod]
        public void twoNumsMultiplicationTest()
        {
            Formula f = new Formula("5 * 4");
            Assert.AreEqual((double)20, f.Evaluate(null));
        }

        [TestMethod]
        public void twoNumsDivisionTest()
        {
            Formula f = new Formula("6/2");
            Assert.AreEqual((double)3, f.Evaluate(null));
        }

        [TestMethod]
        public void divideByZeroTest()
        {
            Formula f = new Formula("(5 + 1) / (3 - 3)");
            Assert.AreEqual("Cannot divide by 0!", ((FormulaError)f.Evaluate(null)).Reason);

            Formula f1 = new Formula("(6+1)/0");
            Assert.AreEqual("Cannot divide by 0!", ((FormulaError)f1.Evaluate(null)).Reason);
        }

        [TestMethod]
        public void parenthesesTest()
        {
            Formula f = new Formula("6*(1+1)");
            Assert.AreEqual((double)12, f.Evaluate(null));
        }

        [TestMethod]
        public void orderOfOperationTest()
        {
            Formula f = new Formula("2 + 4 * 5");
            Assert.AreEqual((double)22, f.Evaluate(null));
        }
    }
}

