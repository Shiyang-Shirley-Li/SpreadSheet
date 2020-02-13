/// <summary>
/// Author: Shiyang(Shirley) Li
/// Date:02/10/2020
/// Course: CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Shiyang(Shirley) Li - This work may not be copied for use in Academic Coursework.
/// 
/// I, Shiyang(Shirley) Li, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// This is a test class for SpreadsheetTest and is intended
/// to contain all SpreadsheetTest Unit Tests 
/// 
/// </summary>
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SpreadsheetTests
{
    /// <summary>
    /// This is test class for spreadsheet. Inherit the spreadsheet, thus we can test for pivate and protected 
    /// methods.
    /// </summary>
    [TestClass]
    public class SpreadsheetTests:Spreadsheet
    {
        /// <summary>
        /// A method that match the IsValid signature telling the core spreadsheet code
        /// what valid variable names are
        /// </summary>
        /// <param name="str">A string that need to be checked</param>
        /// <returns>true if str is valid, otherwise false</returns>
        public bool isValid(string str)
        {
            Regex oneDigitOneLetter = new Regex("[A-Za-z][0-9]");
            if (oneDigitOneLetter.IsMatch(str))
            {
                return true;
            }
            return false;
        }

        [TestMethod()]
        public void SimpleEmptyConstructorTest()
        {
            AbstractSpreadsheet emptySheet = new Spreadsheet();
            Assert.IsFalse(emptySheet.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }

        [TestMethod()]
        public void SimpleThreeArgsConstructorTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(isValid, s => s.ToUpper(), "1");
            //How to test the constructor and do I need to test?????????
        }

        [TestMethod()]
        public void SimpleFourArgsConstructorTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet("", isValid, s => s.ToUpper(), "1");//what is the filePath?????????
        }

        [TestMethod]
        public void GetNamesOfAllNonemptyCellsTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "12");
            sheet.SetContentsOfCell("A3", "Hello");
            sheet.SetContentsOfCell("_c", "=A1 * A1");

            IEnumerator<string> namesOfAllNonemptyCells = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsTrue(namesOfAllNonemptyCells.MoveNext());
            Assert.AreEqual("A1", namesOfAllNonemptyCells.Current);
            Assert.IsTrue(namesOfAllNonemptyCells.MoveNext());
            Assert.AreEqual("A3", namesOfAllNonemptyCells.Current);
            Assert.IsTrue(namesOfAllNonemptyCells.MoveNext());
            Assert.AreEqual("_c", namesOfAllNonemptyCells.Current);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsNullNameTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents(null);
        }

        [TestMethod]
        public void GetCellContentsNullTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.AreEqual("", sheet.GetCellContents("A1"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsInvalidTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("&");
        }

        [TestMethod]
        public void GetCellContentsTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "12");

            Assert.AreEqual((double)12, sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Test setCellContents when the previous content is a formula
        /// </summary>
        [TestMethod]
        public void SetCellContentsGeneralTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("C1", "=B1+A1");

            IList<string> list = new List<string> { "C1" };
            IList<string> testList = sheet.SetContentsOfCell("C1", "Hello");

            //Is there a good way to test???????????
            bool equalityResult = true;
            for(int i = 0; i < list.Count; i++)
            {
                if (!list[i].Equals(testList[i]))
                {
                    equalityResult = false;
                }
            }
            Assert.IsTrue(equalityResult);
        }

        [TestMethod]
        public void SetCellContentsNumberTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B1", "=A1 * 2");
            sheet.SetContentsOfCell("C1", "=B1+A1");

            IList<string> list = new List<string> { "A1", "B1", "C1" };
            IList<string> testList = sheet.SetContentsOfCell("A1", "12");

            //Is there a good way to test???????????
            bool equalityResult = true;
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].Equals(testList[i]))
                {
                    equalityResult = false;
                }
            }
            Assert.IsTrue(equalityResult);
        }

        [TestMethod]
        public void SetCellContentsTextTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B1", "=A1 * 2");
            sheet.SetContentsOfCell("C1", "=B1+A1");

            IList<string> list= new List<string> { "A1", "B1", "C1" };
            IList<string> testList = sheet.SetContentsOfCell("A1", "Hello");

            //Is there a good way to test???????????
            bool equalityResult = true;
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].Equals(testList[i]))
                {
                    equalityResult = false;
                }
            }
            Assert.IsTrue(equalityResult);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void SetCellContentsNullTextTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            string str = null;
            sheet.SetContentsOfCell("A1", str);
        }

        [TestMethod]
        public void SetCellContentsFormulaTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B1", "=A1 * 2");
            sheet.SetContentsOfCell("C1", "=B1+A1");

            IList<string> list = new List<string> { "A1", "B1", "C1" };
            IList<string> testList = sheet.SetContentsOfCell("A1", "=D1 + E1");

            //Is there a good way to test???????????
            bool equalityResult = true;
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].Equals(testList[i]))
                {
                    equalityResult = false;
                }
            }
            Assert.IsTrue(equalityResult);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void SetCellContentsNullFormulaTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=null");
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCellContentsFormulaCircularTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B1", "=A1 * 2");
            sheet.SetContentsOfCell("C1", "=B1+A1");
            sheet.SetContentsOfCell("A1", "=B1 + C1");
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void GetDirectDependentsNullNameTest()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetDirectDependentsNullOrInvalidNameTest()
        {

        }

        [TestMethod]
        public void GetDirectDependentsTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "3");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "=B1+A1");
            sheet.SetContentsOfCell("D1", "=B1-C1");

            IEnumerator<string> directDependentsOfA1 = GetDirectDependents("A1").GetEnumerator();//why i cannot use sheet.Get.....???????????????
            Assert.IsTrue(directDependentsOfA1.MoveNext());
            Assert.AreEqual("A1", directDependentsOfA1.Current);
            Assert.IsTrue(directDependentsOfA1.MoveNext());
            Assert.AreEqual("A3", directDependentsOfA1.Current);
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void GetCellsToRecalculateCircularExceptionTest()
        {
            //how to test since the exception will be thrown during set
        }

        [TestMethod]
        public void GetCellsToRecalculateTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            sheet.SetContentsOfCell("B1", "7");
            sheet.SetContentsOfCell("C1", "=A1+B1");
            sheet.SetContentsOfCell("D1", "=A1*C1");
            sheet.SetContentsOfCell("E1", "15");
            sheet.SetContentsOfCell("A1", "2");
            sheet.SetContentsOfCell("B1", "3");//how to test???????????
            IEnumerator<string> directDependentsOfA1 = GetDirectDependents("A1").GetEnumerator();//why i cannot use sheet.Get.....???????????????
            Assert.IsTrue(directDependentsOfA1.MoveNext());
            Assert.AreEqual("A1", directDependentsOfA1.Current);
            Assert.IsTrue(directDependentsOfA1.MoveNext());
            Assert.AreEqual("A3", directDependentsOfA1.Current);
        }

        [TestMethod]
        public void GetCellsToRecalculateStringTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            sheet.SetContentsOfCell("B1", "7");
            sheet.SetContentsOfCell("C1", "=A1+B1");
            sheet.SetContentsOfCell("D1", "=A1*C1");
            sheet.SetContentsOfCell("E1", "15");
            sheet.SetContentsOfCell("A1", "2");
            sheet.SetContentsOfCell("B1", "3");//how to test???????????
            IEnumerator<string> directDependentsOfA1 = GetDirectDependents("A1").GetEnumerator();//why i cannot use sheet.Get.....???????????????
            Assert.IsTrue(directDependentsOfA1.MoveNext());
            Assert.AreEqual("A1", directDependentsOfA1.Current);
            Assert.IsTrue(directDependentsOfA1.MoveNext());
            Assert.AreEqual("A3", directDependentsOfA1.Current);
        }


    }
}


