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
using System.Xml;

namespace SpreadsheetTests
{
    /// <summary>
    /// This is a test class for spreadsheet. Inherit the spreadsheet, thus we can test for pivate and protected 
    /// methods.
    /// </summary>
    [TestClass]
    public class SpreadsheetTests : Spreadsheet
    {
        /// <summary>
        /// A method that match the IsValid signature telling the core spreadsheet code
        /// what valid variable names are
        /// </summary>
        /// <param name="str">A string that need to be checked</param>
        /// <returns>true if str is valid, otherwise false</returns>
        public bool isValid(string str)
        {
            Regex oneDigitOneLetter = new Regex("^[A-Za-z][0-9]$");
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
            Assert.IsFalse(sheet.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
            sheet.SetContentsOfCell("a1", "Hi!");
            IEnumerator<string> namesOfAllNonemptyCells = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsTrue(namesOfAllNonemptyCells.MoveNext());
            Assert.AreEqual("A1", namesOfAllNonemptyCells.Current);
        }

        [TestMethod()]
        public void SimpleFourArgsConstructorTest()
        {
            using (XmlWriter writer = XmlWriter.Create("save.txt"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "1.0");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A1");
                writer.WriteElementString("contents", "hello");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            AbstractSpreadsheet ss = new Spreadsheet("save.txt", s => true, s => s, "1.0");
            ss.Save("save.txt");
        }

        [TestMethod()]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SimpleFourArgsConstructorExeceptionTest()
        {
            AbstractSpreadsheet ss = new Spreadsheet("good.txt", s => true, s => s, "1.0");
        }

        [TestMethod]
        public void GetNamesOfAllNonemptyCellsTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(isValid, s => s.ToUpper(), "1");
            sheet.SetContentsOfCell("A1", "12");
            sheet.SetContentsOfCell("A3", "Hello");
            sheet.SetContentsOfCell("B1", "=A1 * A1");

            IEnumerator<string> namesOfAllNonemptyCells = sheet.GetNamesOfAllNonemptyCells().GetEnumerator();
            Assert.IsTrue(namesOfAllNonemptyCells.MoveNext());
            Assert.AreEqual("A1", namesOfAllNonemptyCells.Current);
            Assert.IsTrue(namesOfAllNonemptyCells.MoveNext());
            Assert.AreEqual("A3", namesOfAllNonemptyCells.Current);
            Assert.IsTrue(namesOfAllNonemptyCells.MoveNext());
            Assert.AreEqual("B1", namesOfAllNonemptyCells.Current);
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
            AbstractSpreadsheet sheet = new Spreadsheet(isValid, s => s.ToUpper(), "1");
            sheet.GetCellContents("A12");
        }

        [TestMethod]
        public void GetCellContentsTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(isValid, s => s.ToUpper(), "1");
            sheet.SetContentsOfCell("A1", "12");

            Assert.AreEqual((double)12, sheet.GetCellContents("A1"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellInvalidTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(isValid, s => s.ToUpper(), "1");
            sheet.SetContentsOfCell("A12", "12");
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
        public void SetCellContentsNumberTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B1", "=A1 * 2");
            sheet.SetContentsOfCell("C1", "=B1+A1");

            IList<string> list = new List<string> { "A1", "B1", "C1" };
            IList<string> testList = sheet.SetContentsOfCell("A1", "12");

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

            IList<string> list = new List<string> { "A1", "B1", "C1" };
            IList<string> testList = sheet.SetContentsOfCell("A1", "Hello");

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
            sheet.SetContentsOfCell("A1", null);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SetCellContentsFormulaFormatTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B1", "=A1 *");
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
            IEnumerable<string> directDependents = GetDirectDependents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetDirectDependentsInvalidNameTest()
        {
            IEnumerable<string> directDependents = GetDirectDependents("1a");
        }

        [TestMethod]
        public void ChangedTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.IsFalse(sheet.Changed);
            sheet.SetContentsOfCell("A1", "a");
            Assert.IsTrue(sheet.Changed);
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetSavedVersionrExeceptionTest()
        {
            GetSavedVersion("2.txt");
        }

        [TestMethod]
        public void GetSavedVersionTest()
        {
            Assert.AreEqual("1.0", GetSavedVersion("save.txt"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellValueExceptionTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(isValid, s => s.ToUpper(), "1");
            sheet.GetCellValue("a");
        }

        [TestMethod]
        public void GetStringCellValueTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s, "");
            sheet.SetContentsOfCell("A1", "Good job!");
            Assert.AreEqual("Good job!", sheet.GetCellValue("A1"));
        }

        [TestMethod]
        public void GetDoubleCellValueTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s, "");
            sheet.SetContentsOfCell("A1", "2.0");
            Assert.AreEqual(2.0, sheet.GetCellValue("A1"));
        }

        [TestMethod]
        public void GetFormulaCellValueTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s, "");
            sheet.SetContentsOfCell("A1", "= 1 + 0.5");
            Assert.AreEqual(1.5, sheet.GetCellValue("A1"));
        }

        // STRESS TESTS
        [TestMethod()]
        public void TestStress1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=B1+B2");
            s.SetContentsOfCell("B1", "=C1-C2");
            s.SetContentsOfCell("B2", "=C3*C4");
            s.SetContentsOfCell("C1", "=D1*D2");
            s.SetContentsOfCell("C2", "=D3*D4");
            s.SetContentsOfCell("C3", "=D5*D6");
            s.SetContentsOfCell("C4", "=D7*D8");
            s.SetContentsOfCell("D1", "=E1");
            s.SetContentsOfCell("D2", "=E1");
            s.SetContentsOfCell("D3", "=E1");
            s.SetContentsOfCell("D4", "=E1");
            s.SetContentsOfCell("D5", "=E1");
            s.SetContentsOfCell("D6", "=E1");
            s.SetContentsOfCell("D7", "=E1");
            s.SetContentsOfCell("D8", "=E1");
            IList<string> cells = new List<string>(s.SetContentsOfCell("E1", "0"));
            IList<string> testList = new List<string> { "E1", "D8", "D7", "C4", "D6", "D5", "C3", "B2", "D4", "D3", "C2", "D2", "D1", "C1", "B1", "A1" };
            bool equalityResult = true;
            for (int i = 0; i < cells.Count; i++)
            {
                if (!cells[i].Equals(testList[i]))
                {
                    equalityResult = false;
                }
            }
            Assert.IsTrue(equalityResult);
            
        }

        // Repeated for extra weight
        [TestMethod()]
        public void TestStress1a()
        {
            TestStress1();
        }
        [TestMethod()]
        public void TestStress1b()
        {
            TestStress1();
        }
        [TestMethod()]
        public void TestStress1c()
        {
            TestStress1();
        }

        [TestMethod()]
        public void TestStress2()
        {
            Spreadsheet s = new Spreadsheet();
            ISet<string> cells = new HashSet<string>();
            for (int i = 1; i < 200; i++)
            {
                cells.Add("A" + i);
                Assert.IsTrue(cells.SetEquals(s.SetContentsOfCell("A" + i, "=" + new Formula("A" + (i + 1)))));
            }
        }
        [TestMethod()]
        public void TestStress2a()
        {
            TestStress2();
        }
        [TestMethod()]
        public void TestStress2b()
        {
            TestStress2();
        }
        [TestMethod()]
        public void TestStress2c()
        {
            TestStress2();
        }
    }
}

