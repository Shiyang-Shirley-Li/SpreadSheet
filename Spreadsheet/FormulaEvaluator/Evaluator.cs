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
                            int result = Int32.Parse(token) * Int32.Parse(val);
                            valueStack.Push(result.ToString());
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
                            if (token.Equals("0"))
                            {
                                throw new ArgumentException();
                            }
                            int result = Int32.Parse(val) / Int32.Parse(token);
                            valueStack.Push(result.ToString());
                        }
                    }
                    valueStack.Push(token);
                }
                else if (token.Equals("^[a-zA-Z]+[0-9]+$"))
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
                            int intTokenM = variableEvaluator(token);//use the looked-up value of t
                            int result = intTokenM * Int32.Parse(val);
                            valueStack.Push(result.ToString());
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
                            int intTokenD = variableEvaluator(token);//use the looked-up value of t
                            if (intTokenD == 0)
                            {
                                throw new ArgumentException();
                            }
                            int result = Int32.Parse(val) / intTokenD;
                            valueStack.Push(result.ToString());
                        }
                    }
                    int intToken = variableEvaluator(token);//use the looked-up value of t
                    valueStack.Push(intToken.ToString());
                }
                else if(token.Equals("+")|| token.Equals("-"))
                {
                    if(valueStack.Count < 2)
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        string val1 = valueStack.Pop();
                        string val2 = valueStack.Pop();
                        string oper = operatorStack.Pop();

                    }
                }
            }


            return 0;
        }
    }
}
