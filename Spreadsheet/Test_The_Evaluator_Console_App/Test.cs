using FormulaEvaluator;
using System;

namespace Test_The_Evaluator_Console_App
{
    class Test
    {
        static void Main(string[] args)
        {
            
            if (Evaluator.Evaluate("5+5", null) == 10)
            {
                Console.WriteLine("Hello World!");
            }

            
        
        }
    }
}

