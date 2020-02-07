/// <summary>
/// Author: Shiyang(Shirley) Li
/// Date:02/03/2020
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


namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        //AbstractSpreadsheet sheet = new Spreadsheet();
        //#region Class Initialize and Cleanup
        //[ClassInitialize]
        //public static void ClassInitialize(TestContext tc)
        //{

        //}
        //[ClassCleanup]
        //public static void ClassCleanup()
        //{
        //}
        //#endregion

        [TestMethod()]
        public void SimpleEmptyConstructorTest()
        {
            AbstractSpreadsheet emptySheet = new Spreadsheet();
            Assert.IsFalse(emptySheet.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }

        [TestMethod]
        public void GetNamesOfAllNonemptyCellsTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 12);
            sheet.SetCellContents("A3", "Hello");
            Formula _cFormula = new Formula("A1 * A1");
            sheet.SetCellContents("_c", _cFormula);

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
        public void GetCellContentsNullTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents(null);
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
            sheet.SetCellContents("A1", 12);

            Assert.AreEqual((double)12, sheet.GetCellContents("A1"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsInvalidTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("&", 12.00);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsNullTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents(null, 12.00);
        }

        [TestMethod]
        public void SetCellContentsNumberTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula B1Formula = new Formula("A1 * 2");
            sheet.SetCellContents("B1", B1Formula);
            sheet.SetCellContents("C1", new Formula("B1+A1"));

            ISet<string> set = new HashSet<string> { "A1", "B1", "C1" };
            ISet<string> testSet = sheet.SetCellContents("A1", 12);

            Assert.IsTrue(set.SetEquals(testSet));
        }

        [TestMethod]
        public void SetCellContentsTextTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula B1Formula = new Formula("A1 * 2");
            sheet.SetCellContents("B1", B1Formula);
            sheet.SetCellContents("C1", new Formula("B1+A1"));

            ISet<string> set = new HashSet<string> { "A1", "B1", "C1" };//Do I need to remove the dependency?????
            ISet<string> testSet = sheet.SetCellContents("A1", "Hello");

            Assert.IsTrue(set.SetEquals(testSet));
        }

        [TestMethod]
        public void SetCellContentsFormulaTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula B1Formula = new Formula("A1 * 2");
            sheet.SetCellContents("B1", B1Formula);
            sheet.SetCellContents("C1", new Formula("B1+A1"));

            ISet<string> set = new HashSet<string> { "A1", "D1", "E1" };//Do I need to remove the dependency?????
            ISet<string> testSet = sheet.SetCellContents("A1", new Formula("D1 + E1"));

            Assert.IsTrue(set.SetEquals(testSet));
        }
    }
}


