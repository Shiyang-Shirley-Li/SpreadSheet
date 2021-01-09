using FormulaEvaluator;
using System;
/// <summary>
/// Author: Shiyang(Shirley) Li
/// Date:01/11/2020
/// Copyright:This work may not be copied for use in Academic Coursework.
/// 
/// I, Shiyang(Shirley) Li, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the code are cited in my README file. 
/// 
/// This file is a method that tests the FormulaEvaluator project.
/// </summary>
namespace Test_The_Evaluator_Console_App
{
    /// <summary>
    /// The EvaluatorTest is a class for testing FormulaEvaluator.
    /// </summary>
    class EvaluatorTest
    {
        /// <summary>
        /// A main method that calls different tests for FormulaEvaluator
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            twoNumsPlusTest();

            twoNumsMinusTest();

            twoNumsMultiplicationTest();

            twoNumsDivisionTest();

            parenthesesTest();

            orderOfOperationTest();

            lookUpVariableTest();

            divisionByZeroExceptionTest();

            valueStackEmptyExceptionTest();

            variableHasNoValueExceptionTest();

            valueStackLessThanTwoValsExceptionTest();

            rightParenthesisNotFoundExceptionTest();

            illegalSymbolsExceptionTest();

            leftParenthesisNotFoundExceptionTest();
        }

        /// <summary>
        /// A function that has a string prameter and returns an integer
        /// </summary>
        /// <param name="variableName">
        /// a string consisting of one or more letters 
        /// followed by one or more digits
        /// </param>
        /// <returns>If found, return an integer; if not, throw an exception</returns>
        static int searchForAValue(String variableName)
        {
            if (variableName.Equals("a1"))
            {
                return 1;
            }
            else if (variableName.Equals("A1"))
            {
                return 0;
            }
            throw new ArgumentException("No such variable!");

        }
        static void twoNumsPlusTest()
        {
            if (Evaluator.Evaluate("5 + 4", null) == 9)
            {
                Console.WriteLine("5 + 4 = 9 !");
            }
        }

        public static void twoNumsMinusTest()
        {
            if (Evaluator.Evaluate("5-4", null) == 1)
            {
                Console.WriteLine("5 - 4 = 1 !");
            }
        }

        static void twoNumsMultiplicationTest()
        {
            if (Evaluator.Evaluate("5*5", null) == 25)
            {
                Console.WriteLine("5 * 5 = 25 !");
            }
        }

        static void twoNumsDivisionTest()
        {
            if (Evaluator.Evaluate("6/2", null) == 3)
            {
                Console.WriteLine("6 / 2 = 3 !");
            }
        }

        static void parenthesesTest()
        {
            if (Evaluator.Evaluate("6/(1+1)", null) == 3)
            {
                Console.WriteLine("6 / (1+1) = 3 !");
            }
        }

        static void orderOfOperationTest()
        {
            if (Evaluator.Evaluate("2 + 4 * 5", null) == 22)
            {
                Console.WriteLine("2 + 4 * 5 = 22 !");
            }
        }

        static void lookUpVariableTest()
        {
            if (Evaluator.Evaluate("x1 * 5", (x1) => 6) == 30)
            {
                Console.WriteLine("x1 * 5 = 30 !");
            }
        }

        static void divisionByZeroExceptionTest()
        {
            try
            {
                Evaluator.Evaluate("2/0", null);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Do not divide by 0!");
            }
        }

        static void valueStackEmptyExceptionTest()
        {
            try
            {
                Evaluator.Evaluate("*", null);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("No value exists in the formula!");
            }
        }

        static void variableHasNoValueExceptionTest()
        {
            try
            {
                Evaluator.Evaluate("B1", searchForAValue);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Variable does not have a value!");
            }
        }

        static void valueStackLessThanTwoValsExceptionTest()
        {
            try
            {
                Evaluator.Evaluate("1 + 1 - ", null);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("The value stack contains fewer than 2 values when trying to pop it!");
            }
        }

        static void rightParenthesisNotFoundExceptionTest()
        {
            try
            {
                Evaluator.Evaluate("(1 + 1 / 2", null);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("A ) isn't found where expected!");
            }
        }

        static void illegalSymbolsExceptionTest()
        {
            try
            {
                Evaluator.Evaluate("@", null);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Illegal symbol!");
            }
        }

        static void leftParenthesisNotFoundExceptionTest()
        {
            try
            {
                Evaluator.Evaluate("1 + 1) / 2", null);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("A ( isn't found where expected!");
            }
        }
    }
}

