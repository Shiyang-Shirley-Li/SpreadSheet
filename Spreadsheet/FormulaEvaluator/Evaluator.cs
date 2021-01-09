﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
/// <summary>
/// Author: Shiyang(Shirley) Li
/// Date:01/11/2020
/// Copyright: Shiyang(Shirley) Li - This work may not be copied for use in Academic Coursework.
/// 
/// I, Shiyang(Shirley) Li, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the code are cited in my README file. 
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
        /// A helper method for checking if a string contains white space or not
        /// </summary>
        /// <param name="str"> A string needed to check if contains white space</param>
        /// <returns>true if the string has white space, otherwise false</returns>
        private static bool isEmpty(string str)
        {
            return (str.Equals(" "));
        }

        /// <summary>
        ///A helper method for "If + or - or / or * is at the top of the operator stack, 
        ///pop the value stack twice and the operator stack once." case, in order to avoid reuse of code
        /// </summary>
        /// <param name="operatorStack">stack with string operators in it</param>
        /// <param name="valueStack">stack with integers in it</param>
        private static void operatorWithPopValStackTwice(Stack<string> operatorStack, Stack<int> valueStack)
        {
            int secondVal = valueStack.Pop();
            int firstVal = valueStack.Pop();
            int currentResult = 0;
            if (operatorStack.Peek().Equals("+"))
            {
                currentResult = firstVal + secondVal;
            }
            else if (operatorStack.Peek().Equals("-"))
            {
                currentResult = firstVal - secondVal;
            }
            else if (operatorStack.Peek().Equals("*"))
            {
                currentResult = firstVal * secondVal;
            }
            else if (operatorStack.Peek().Equals("/"))
            {
                if (secondVal == 0)
                {
                    throw new ArgumentException("A division by zero occurs.");
                }
                else
                {
                    currentResult = firstVal / secondVal;
                }
            }
            valueStack.Push(currentResult);
            operatorStack.Pop();
        }

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
            Regex intNumbers = new Regex("^[0-9]+$");
            Regex variableFormat = new Regex("^[a-zA-Z]+[0-9]+$");

            foreach (string whiteSpaceToken in subStringsList)
            {
                string token = whiteSpaceToken.Trim();//delete all the possible space

                //when the token is integer or a variable
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

                    if (operatorStack.Count > 0 && (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/")))
                    {
                        if (valueStack.Count == 0)
                        {
                            throw new ArgumentException("The value stack is empty, when * or / is at the top");
                        }
                        else
                        {
                            int currentVal = valueStack.Pop();
                            string currentOperator = operatorStack.Pop();

                            if (currentOperator.Equals("*"))
                            {
                                int currentResult = currentVal * t;
                                valueStack.Push(currentResult);
                            }
                            else if (t == 0)
                            {
                                throw new ArgumentException("A division by zero occurs.");
                            }
                            else
                            {
                                int currentResult = currentVal / t;
                                valueStack.Push(currentResult);
                            }
                        }
                    }
                    else
                    {
                        valueStack.Push(t);
                    }
                }

                //when the token is + or -
                else if ((token.Equals("+") || token.Equals("-")))
                {

                    if (valueStack.Count <= 0)
                    {
                        throw new ArgumentException("Nothing to plus");
                    }
                    else if (valueStack.Count == 1)
                    {
                        operatorStack.Push(token);
                    }
                    else
                    {
                        if (operatorStack.Count < 0)
                        {
                            throw new ArgumentException("No operator for more than two numbers");
                        }
                        else
                        {
                            if (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/"))
                            {
                                operatorWithPopValStackTwice(operatorStack, valueStack);
                            }
                            else if (operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-"))
                            {
                                operatorWithPopValStackTwice(operatorStack, valueStack);
                            }
                            operatorStack.Push(token);
                        }
                    }
                }

                //when the token is * or / or (
                else if (token.Equals("*") || token.Equals("/") || token.Equals("("))
                {
                    operatorStack.Push(token);
                }

                //when the tooken is )
                else if (token.Equals(")"))
                {
                    if (valueStack.Count <= 0)
                    {
                        throw new ArgumentException("Nothing to caculate");
                    }
                    else
                    {
                        if (operatorStack.Count < 0 && valueStack.Count < 2)
                        {
                            throw new ArgumentException("No operator for more than two numbers");
                        }
                        else if (operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-"))
                        {
                            operatorWithPopValStackTwice(operatorStack, valueStack);
                        }
                    }
                    if (operatorStack.Count == 0)
                    {
                        throw new ArgumentException("A ( isn't found where expected");
                    }
                    else if (operatorStack.Count != 0)
                    {
                        if (!operatorStack.Peek().Equals("("))
                        {
                            throw new ArgumentException("A ( isn't found where expected");
                        }
                        else
                        {
                            operatorStack.Pop();
                        }
                    }

                    if (operatorStack.Count > 0 && (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/")))
                    {
                        if (valueStack.Count <= 0)
                        {
                            throw new ArgumentException("Nothing to caculate");
                        }
                        else
                        {
                            if (operatorStack.Count < 0 && valueStack.Count < 2)
                            {
                                throw new ArgumentException("No operator for more than two numbers");
                            }
                            else
                            {
                                if (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/"))
                                {
                                    operatorWithPopValStackTwice(operatorStack, valueStack);
                                }
                            }
                        }
                    }
                }
            }
            //When the last token has been processed
            if (operatorStack.Count == 0 && valueStack.Count == 1)
            {
                finalResult = valueStack.Pop();
            }
            else if (operatorStack.Count == 0 && valueStack.Count != 1)
            {
                throw new ArgumentException();
            }
            else if (operatorStack.Count != 0)
            {
                if (operatorStack.Count == 1 && (operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-")) && valueStack.Count == 2)
                {
                    int val1 = valueStack.Pop();
                    int val2 = valueStack.Pop();
                    string currentOperator = operatorStack.Pop();
                    if (currentOperator.Equals("+"))
                    {
                        finalResult = val2 + val1;
                    }
                    else if (currentOperator.Equals("-"))
                    {
                        finalResult = val2 - val1;
                    }
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
