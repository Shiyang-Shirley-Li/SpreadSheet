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
    /// The Evaluator is a class that evaluates arithmatic expressions
    /// using standard infix notation, which respects the usual precedence
    /// rules
    /// </summary>
    /// <remarks>
    /// This class can add, subtract, multiply and divide
    /// </remarks>
    public static class Evaluator
    {
        public delegate int Lookup(String variable_name);//a delegate used to look up the value of a variable

        /// <summary>
        /// Evaluate string expressions, do the calculation and returns the result
        /// </summary>
        /// <param name="expression"> A string of arithmetic expressions </param>
        /// <param name="variableEvaluator"> A delegate can be used to look up the value of a variable </param>
        /// <returns>the result of the arithmetic expression</returns>
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            List<string> subStringsList = new List<string>(substrings);//change the string array to list, so that we can modify

            
            static bool isEmpty(string str)
            {
                return (str.Equals(" "));
            }

            subStringsList.RemoveAll(isEmpty);//remove empty strings mixed in

            //check if all the tokens are legal
            foreach (string token in subStringsList)
            {
                if (!(!token.Equals("(") || !token.Equals(")") || !token.Equals("+") || !token.Equals("-") || !token.Equals("*") || !token.Equals("/") || !token.Equals("^[a-zA-Z]+[0-9]+$") || !token.Equals("[0-9]+")))
                {
                    throw new ArgumentException();
                }
            }

            Stack<int> valueStack = new Stack<int>();
            Stack<string> operatorStack = new Stack<string>();
            int finalResult = 0;
            Regex intNumbers = new Regex("[0-9]+");
            Regex variableFormat = new Regex("^[a-zA-Z]+[0-9]+$");

            foreach (string token in subStringsList)
            {

                if (intNumbers.IsMatch(token) || variableFormat.IsMatch(token))
                {
                    int t;
                    if (intNumbers.IsMatch(token))//if token is number string, convert it to integer
                    {
                        t = Int32.Parse(token);
                    }
                    else//if token is variable string, use the looked-up value of the token
                    {
                        t = variableEvaluator(token);
                    }

                    if (valueStack.Count == 0 && operatorStack.Count <= 1)//when nothing in both stack, just push the integer
                    {
                        valueStack.Push(t);
                    }
                    else
                    {

                        string opt = operatorStack.Pop();

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
                        else if (opt.Equals("+"))
                        {
                            int result = t + val;
                            valueStack.Push(result);
                        }
                        else if (opt.Equals("-"))
                        {
                            int result = val - t;
                            valueStack.Push(result);
                        }
                        else
                        {
                            valueStack.Push(t);
                        }
                    }
                }

                else if (token.Equals("+") || token.Equals("-") || token.Equals(")"))
                {
                    if ((valueStack.Count == 1 && !token.Equals(")")) || operatorStack.Count == 0)
                    {
                        operatorStack.Push(token);
                    }
                    else if(valueStack.Count == 1 && token.Equals(")") && operatorStack.Count == 1)
                    {
                        operatorStack.Pop();
                    }
                    else
                    { 
                    if (valueStack.Count < 2 || operatorStack.Count == 0)
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        string oper = operatorStack.Pop();

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

                            if (token.Equals("+") || token.Equals("-"))
                            {
                                operatorStack.Push(token);
                            }
                            else
                            {
                                if (operatorStack.Count == 0)
                                {
                                    throw new ArgumentException();
                                }
                                string leftP = operatorStack.Pop();
                                if (!leftP.Equals("("))
                                {
                                    throw new ArgumentException();
                                }

                                string finalO = operatorStack.Pop();
                                if (finalO.Equals("*") || finalO.Equals("/"))
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
                                        if (valF1 == 0)
                                        {
                                            throw new ArgumentException();
                                        }
                                        else
                                        {
                                            int result = valF2 / valF1;
                                            valueStack.Push(result);
                                        }
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

            if(operatorStack.Count == 0 && valueStack.Count == 1)
            {
                finalResult = valueStack.Pop();
            }
            else if(operatorStack.Count == 0 && valueStack.Count !=1)
            {
                throw new ArgumentException();
            }
            else if(operatorStack.Count != 0)
            {
                if(operatorStack.Count == 1 && operatorStack.Pop().Equals("+")&& valueStack.Count == 2)
                {
                    int val1 = valueStack.Pop();
                    int val2 = valueStack.Pop();
                    finalResult = variableEvaluator(val2.ToString()) + variableEvaluator(val1.ToString()); 
                }
                else if (operatorStack.Count == 1 && operatorStack.Pop().Equals("-") && valueStack.Count == 2)
                {
                    int val1 = valueStack.Pop();
                    int val2 = valueStack.Pop();
                    finalResult = variableEvaluator(val2.ToString()) - variableEvaluator(val1.ToString());
                }
                else
                {
                    throw new ArgumentException();
                }
            }


            return finalResult;
        }
    }
}
