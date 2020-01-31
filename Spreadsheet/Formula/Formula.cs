// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// An extension class for checking formula format
    /// </summary>
    static class ExtensionsClass
    {
        /// <summary>
        /// A method to check if a stirng is in right format as defined
        /// </summary>
        /// <param name="variable">a string that need checking</param>
        /// <returns>true if the string is in right format; otherwise, return false</returns>
        public static Boolean isVariable(this string variable)
        {
            Regex variableFormat = new Regex("^([a-zA-Z]|[_])[0-9a-zA-Z]*$");
            if (variableFormat.IsMatch(variable))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// A method to check if a string is an operotor
        /// </summary>
        /// <param name="fourOperators">a string that need checking</param>
        /// <returns>true if the string is an operator; otherwise, return false</returns>
        public static Boolean isOperator(this string fourOperators)
        {
            if (fourOperators.Equals("+") || fourOperators.Equals("*")
                    || fourOperators.Equals("-") || fourOperators.Equals("/"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Boolean isDoulbe(this string token)
        {
            if (Double.TryParse(token, out double firstResult))
            {
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        //instance variables
        private List<string> normalizedFormula;


        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true. 
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect. 
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            List<string> formulaTokensList = GetTokens(formula).ToList();
            int numOfLeftParentheses = 0;
            int numOfRightParentheses = 0;

            //Syntactical correction
            if (formulaTokensList.Count() < 1)
            {
                throw new FormulaFormatException("There must be at least one token!");
            }

            for (int i = 0; i < formulaTokensList.Count(); i++)
            {

                if (formulaTokensList[i].isVariable() && !isValid(normalize(formulaTokensList[i])))
                {
                    throw new FormulaFormatException("The variable name is not valid!");
                }

                if (formulaTokensList[i].isVariable())
                {
                    formulaTokensList[i] = normalize(formulaTokensList[i]);
                }

                //Syntactical correction
                if (!(formulaTokensList[0].isDoulbe() || formulaTokensList[0].isVariable()
                || formulaTokensList[0].Equals("(")))//????
                {
                    throw new FormulaFormatException("The starting token is wrong!");
                }

                if (!(!formulaTokensList[0].isDoulbe() || !formulaTokensList[0].isVariable()
                    || !formulaTokensList[0].Equals(")")))
                {
                    throw new FormulaFormatException("The ending token is wrong!");
                }

                if (!(!formulaTokensList[0].isDoulbe() || !formulaTokensList[0].isVariable() ||
                    !formulaTokensList[i].Equals("(") || !formulaTokensList[i].Equals(")")))//check the validity of the token
                {
                    throw new FormulaFormatException("The tokens are not valid!");
                }

                if (formulaTokensList[i].Equals("("))
                {
                    numOfLeftParentheses++;
                }
                if (formulaTokensList[i].Equals(")"))
                {
                    numOfRightParentheses++;
                }
                if (numOfRightParentheses > numOfLeftParentheses)
                {
                    throw new FormulaFormatException("The number of right parentheses should not " +
                        "be greater than that of left parentheses!");
                }

                if ((formulaTokensList[i].Equals("(") || formulaTokensList[i].isOperator()) && i + 1 < formulaTokensList.Count())
                {
                    if (!(formulaTokensList[i + 1].isDoulbe() || formulaTokensList[i + 1].isVariable()
                        || formulaTokensList[i + 1].Equals("(")))//if statement?????????????
                    {
                        throw new FormulaFormatException("Wrong parenthesis/operator following!");
                    }
                }

                if ((formulaTokensList[i].isDoulbe() || formulaTokensList[i].isVariable()
                        || formulaTokensList[i].Equals(")")) && i + 1 < formulaTokensList.Count())
                {
                    if (!(formulaTokensList[i + 1].isOperator() || formulaTokensList[i + 1].Equals(")")))//if statement?????????????
                    {
                        throw new FormulaFormatException("Wrong extra following!");
                    }
                }

                if (formulaTokensList[i].isDoulbe())
                {
                    formulaTokensList[i] = Double.Parse(formulaTokensList[i]).ToString();
                }
            }

            if (numOfLeftParentheses != numOfRightParentheses)
            {
                throw new FormulaFormatException("The parentheses are not balanced!");
            }


            //create a normalized formula
            normalizedFormula = formulaTokensList;
        }

        /// <summary>
        ///A helper method for "If + or - or / or * is at the top of the operator stack, 
        ///pop the value stack twice and the operator stack once." case, in order to avoid reuse of code
        /// </summary>
        /// <param name="operatorStack">stack with string operators in it</param>
        /// <param name="valueStack">stack with integers in it</param>
        private static void operatorWithPopValStackTwice(Stack<string> operatorStack, Stack<double> valueStack)
        {
            double secondVal = valueStack.Pop();
            double firstVal = valueStack.Pop();
            double currentResult = 0;
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
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned. 
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<double> valueStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            double finalResult = 0;

            foreach (string token in normalizedFormula)
            {
                //when the token is double or a variable
                if (token.isDoulbe() || token.isVariable())
                {
                    double number;
                    if (token.isDoulbe())//if token is number string, convert it to double
                    {
                        number = double.Parse(token);
                    }
                    else//if token is variable string, use the looked-up value of the token
                    {
                        try
                        {
                            number = lookup(token);
                        }
                        catch (ArgumentException)
                        {
                            return new FormulaError("No variables found in my library!");
                        }
                    }

                    if (operatorStack.Count > 0 && (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/")))
                    {
                        //if (valueStack.Count == 0)
                        //{
                        //    throw new ArgumentException("The value stack is empty, when * or / is at the top");
                        //}
                        //else
                        //{
                            double currentVal = valueStack.Pop();
                            string currentOperator = operatorStack.Pop();

                            if (currentOperator.Equals("*"))
                            {
                                double currentResult = currentVal * number;
                                valueStack.Push(currentResult);
                            }
                            else if (number == 0)
                            {
                            return new FormulaError("Cannot divide by 0!");
                            }
                            else
                            {
                                double currentResult = currentVal / number;
                                valueStack.Push(currentResult);
                            }
                        //}
                    }
                    else
                    {
                        valueStack.Push(number);
                    }
                }

                //when the token is + or -
                else if ((token.Equals("+") || token.Equals("-")))
                {

                    //if (valueStack.Count <= 0)
                    //{
                    //    throw new ArgumentException("Nothing to plus");
                    //}
                    if (valueStack.Count == 1)
                    {
                        operatorStack.Push(token);
                    }
                    else
                    {
                        //if (operatorStack.Count < 0)
                        //{
                        //    throw new ArgumentException("No operator for more than two numbers");
                        //}
                        //else
                        //{
                            if (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/"))
                            {
                                operatorWithPopValStackTwice(operatorStack, valueStack);
                            }
                            else if (operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-"))
                            {
                                operatorWithPopValStackTwice(operatorStack, valueStack);
                            }
                            operatorStack.Push(token);
                        //}
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
                    //if (valueStack.Count <= 0)
                    //{
                    //    throw new ArgumentException("Nothing to caculate");
                    //}
                    //else
                    //{
                        //if (operatorStack.Count < 0 && valueStack.Count < 2)
                        //{
                        //    throw new ArgumentException("No operator for more than two numbers");
                        //}
                        if (operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-"))
                        {
                            operatorWithPopValStackTwice(operatorStack, valueStack);
                        }
                    //}
                    //if (operatorStack.Count == 0)
                    //{
                    //    throw new ArgumentException("A ( isn't found where expected");
                    //}
                    if (operatorStack.Count != 0)
                    {
                        //if (!operatorStack.Peek().Equals("("))
                        //{
                        //    throw new ArgumentException("A ( isn't found where expected");
                        //}
                        //else
                        //{
                            operatorStack.Pop();
                        //}
                    }

                    if (operatorStack.Count > 0 && (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/")))
                    {
                        //if (valueStack.Count <= 0)
                        //{
                        //    throw new ArgumentException("Nothing to caculate");
                        //}
                        //else
                        //{
                            //if (operatorStack.Count < 0 && valueStack.Count < 2)
                            //{
                            //    throw new ArgumentException("No operator for more than two numbers");
                            //}
                            //else
                            //{
                                if (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/"))
                                {
                                    operatorWithPopValStackTwice(operatorStack, valueStack);
                                }
                            //}
                        //}
                    }
                }
            }
            //When the last token has been processed
            if (operatorStack.Count == 0 && valueStack.Count == 1)
            {
                finalResult = valueStack.Pop();
            }
            //else if (operatorStack.Count == 0 && valueStack.Count != 1)
            //{
            //    throw new ArgumentException();
            //}
            else if (operatorStack.Count != 0)
            {
                if (operatorStack.Count == 1 && (operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-")) && valueStack.Count == 2)
                {
                    double val1 = valueStack.Pop();
                    double val2 = valueStack.Pop();
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
                //else
                //{
                //    throw new ArgumentException();
                //}
            }
            return finalResult;
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            HashSet<String> variables = new HashSet<string>();
            foreach (string token in normalizedFormula)
            {
                if (token.isVariable() && !variables.Contains(token))
                {
                    variables.Add(token);
                }
            }
            return variables;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            string nFormula = "";

            foreach (string token in normalizedFormula)
            {
                nFormula += token;
            }
            return nFormula;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Formula))
            {
                return false;
            }

            Formula objFormula = (Formula)obj;

            if (!(objFormula.ToString()).Equals(this.ToString()))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (object.ReferenceEquals(f1, null) && object.ReferenceEquals(f2, null))
            {
                return true;
            }
            else if (f1.Equals(f2))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            if (object.ReferenceEquals(f1, null) && object.ReferenceEquals(f2, null))
            {
                return false;
            }
            else if ((object.ReferenceEquals(f1, null) && !object.ReferenceEquals(f2, null)) || (!object.ReferenceEquals(f1, null) && object.ReferenceEquals(f2, null)))
            {
                return true;
            }
            else if (f1.Equals(f2))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}

