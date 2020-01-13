using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
/// <summary>
/// Author: Shiyang(Shirley) Li
/// Date:01/11/2020
/// Course: CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Shiyang(Shirley) Li - This work may not be copied for use in Academic Coursework.
/// 
/// I, Shiyang(Shirley) Li, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// This file is a method that evaluates arithmatic expressions using standard infix notation, which respects the
/// usual precedence rules.
/// 
/// </summary>
namespace FormulaEvaluator
{
    /// <summary>
    /// bob
    /// </summary>
    public static class Evaluator
    {
        public delegate int Lookup(String variable_name);

        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            List<string> list = new List<string>(substrings);//change the string array to list, so that we can modify

            //function which checks whether a string is empty or not
            static bool isEmpty(string str)
            {
                return (str.Equals(" "));
            }

            list.RemoveAll(isEmpty);//remove empty strings mixed in

            foreach(string token in list)
            {
                if(!(token.Equals("(") || token.Equals(")") || token.Equals("+") || token.Equals("-") || token.Equals("*") || token.Equals("/") || token.Equals("^[a-zA-Z]+[0-9]+$"))){
                    throw new ArgumentException();
                }
            }

            Stack<string> valueStack = new Stack<string>();
            Stack<string> operatorStack = new Stack<string>();

            foreach(string token in list)
            {
                if (token.Equals("[0-9]+"))
                {
                    string opt = operatorStack.Pop();
                    if (opt.Equals("*"))
                    {
                        if(valueStack.Count == 0)
                        {
                            throw new ArgumentException();
                        }
                        else
                        {
                            string val = valueStack.Pop();
                            int result = Int32.Parse(token) * Int32.Parse(val); //integer? delegate?
                        }
                    }
                    else if (opt.Equals("/"))
                    {
                        if (valueStack.Count == 0)
                        {
                            throw new ArgumentException();
                        }
                        else
                        {
                            string val = valueStack.Pop();
                            if (val.Equals("0"))
                            {
                                throw new ArgumentException();
                            }
                            int result = Int32.Parse(val) / Int32.Parse(token); //which divides by which?
                        }
                    }
                    valueStack.Push(token);
                }
            }


            return 0;
        }
    }
}
