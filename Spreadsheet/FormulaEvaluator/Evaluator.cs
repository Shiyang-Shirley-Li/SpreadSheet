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
    /// 
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

            foreach (string token in list)
            {
                if (!(token.Equals("(") || token.Equals(")") || token.Equals("+") || token.Equals("-") || token.Equals("*") || token.Equals("/") || token.Equals("^[a-zA-Z]+[0-9]+$")))
                {
                    throw new ArgumentException();
                }
            }

            Stack<int> valueStack = new Stack<int>();
            Stack<string> operatorStack = new Stack<string>();

            foreach (string token in list)
            {
                if (token.Equals("[0-9]+") || token.Equals("^[a-zA-Z]+[0-9]+$"))
                {
                    int t;
                    if (token.Equals("[0-9]+"))//if token is number string, convert it to integer
                    {
                        t = Int32.Parse(token);
                    }
                    else//if token is variable string, use the looked-up value of the token
                    {
                        t = variableEvaluator(token);
                    }

                    if (valueStack.Count == 0)
                    {
                        throw new ArgumentException();
                    }

                    string opt = operatorStack.Pop();//

                    int val = valueStack.Pop();

                    if (opt.Equals("*"))
                    {

                        int result = t * val;
                        valueStack.Push(result);

                    }
                    else if (opt.Equals("/"))
                    {

                        if (t == 0)
                        {
                            throw new ArgumentException();
                        }
                        int result = val / t;
                        valueStack.Push(result);

                    }
                    valueStack.Push(t);
                }

                else if (token.Equals("+") || token.Equals("-") || token.Equals(")"))
                {
                    if (valueStack.Count < 2)
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        string oper = operatorStack.Pop();//

                        if (oper.Equals("+") || oper.Equals("-"))
                        {
                            int val1 = valueStack.Pop();
                            int val2 = valueStack.Pop();

                            if (oper.Equals("+"))
                            {
                                int result = val2 + val1;
                                valueStack.Push(result);
                            }
                            else
                            {
                                int result = val2 - val1;
                                valueStack.Push(result);
                            }
                            
                            if(token.Equals("+") || token.Equals("-"))
                            {
                                operatorStack.Push(token);
                            }
                            else
                            {
                                string leftP = operatorStack.Pop();//
                                if (!leftP.Equals("("))
                                {
                                    throw new ArgumentException();
                                }

                                string finalO = operatorStack.Pop();//
                                if (finalO.Equals("*")||finalO.Equals("/"))
                                {
                                    int valF1 = valueStack.Pop();
                                    int valF2 = valueStack.Pop();

                                    if (finalO.Equals("*"))
                                    {
                                        int result = valF2 * valF2;
                                        valueStack.Push(result);
                                    }
                                    else
                                    {
                                        if(valF1 == 0)
                                        {
                                            throw new ArgumentException();
                                        }
                                        else{
                                            int result = valF2 / valF1;
                                            valueStack.Push(result);
                                        }
                                    }
                                    
                                }
                            }
                        }
                    }
                }
                else if (token.Equals("*") || token.Equals("/") || token.Equals("("))
                {
                    operatorStack.Push(token);
                }
            }


            return valueStack.Pop();
        }
    }
}
