using FormulaEvaluator;
using System;

namespace Test_The_Evaluator_Console_App
{
    class Test
    {
        static void Main(string[] args)
        {

            twoNumsPlusTest();
            twoNumsMinusTest();
            twoNumsMultiplicationTest();
            twoNumsDivisionTest();
            parenthesesTest();
            //orderOfOperationTest();
            lookUpTest();
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
            if (Evaluator.Evaluate("(2+6)/2", null) == 4)
            {
                Console.WriteLine("(2 + 6) / 2 = 4 !");
            }
        }

        static void orderOfOperationTest()
        {
            if (Evaluator.Evaluate("2 + 4 * 5", null) == 22)
            {
                Console.WriteLine("2 + 4 * 5 = 24 !");
            }
        }

        static void lookUpTest()
        {
            if (Evaluator.Evaluate("x1 * 5", (x1)=>6) == 30)
            {
                Console.WriteLine("x1 * 5 = 30 !");
            }
        }
    }
}

