using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System.Collections.Generic;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        #region Class Initialize and Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext tc)//?????????????????
        {

        }
        [ClassCleanup]
        public static void ClassCleanup()
        {
        }
        #endregion

        [TestMethod()]
        public void SimpleEmptyConstructorTest()
        {
            AbstractSpreadsheet emptySheet = new Spreadsheet();
            Assert.IsFalse(emptySheet.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());//?????
        }

        [TestMethod]
        public void GetNamesOfAllNonemptyCellsTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 12);//???????list
            sheet.SetCellContents("A3", "Hello");
            Formula _cFormula = new Formula("A1 * A1");
            sheet.SetCellContents("_c", _cFormula);

            IEnumerator<string> namesOfAllNonemptyCells = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsTrue(namesOfAllNonemptyCells.MoveNext());
            Assert.AreEqual("A1", namesOfAllNonemptyCells.Current);
            Assert.IsTrue(namesOfAllNonemptyCells.MoveNext());
            Assert.AreEqual("A3", namesOfAllNonemptyCells.Current);
            Assert.IsTrue(namesOfAllNonemptyCells.MoveNext());
            Assert.AreEqual("_c", namesOfAllNonemptyCells.Current);//will it in this order??????
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

            Assert.AreEqual(12, sheet.GetCellContents("A1"));
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
        public void SetCellContentsTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula B1Formula = new Formula("A1 * 2");
            sheet.SetCellContents("B1", B1Formula);
            sheet.SetCellContents("C1", new Formula("B1+A1"));

            IList<string> list = new List<string> { "A1", "B1", "C1" };
            Assert.AreEqual(list, sheet.SetCellContents("A1", 12));//??????? SetCellContent text/formula
        }

        [TestMethod]
        public void GetDirectDependentsTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 3);
            sheet.SetCellContents("B1", new Formula("A1*A1"));
            sheet.SetCellContents("C1", new Formula("B1+A1"));
            sheet.SetCellContents("D1", new Formula("B1 - C1"));

            IEnumerable<string> getDirectDependents = sheet.GetDirectDependents("A1");//how can I test protected test?????
        }
    }
}
