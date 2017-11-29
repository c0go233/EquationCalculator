using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equ
{
    //This class holds the logics to get specific number part such as power, or natrual number in variable
    //and convert it into integer number.
    class OperandConverter
    {
        private const string DEFAULT_POWER = "1";
        private const string DEFAULT_NATURAL_NUMBER = "1";

        //It gets and converts to number from string natrual number 
        //It covers the case of natural number with power such as "2^2" converting to "4"
        public static int GetNumberPartFromNaturalNumber(string stringOperand, int startIndex)
        {
            string[] splitStringOperand = stringOperand.Split(Operator.POWER_SEPERATOR);
            return GetIntFrom(splitStringOperand.ElementAt(0).Substring(startIndex));
        }

        //It gets and returns the power from parenthesis such as returning 3 from 
        //equation of (2+x)^3 
        public static int GetPowerFromParenthesis(string stringOperand)
        {
            int closeParenthesisIndex = stringOperand.IndexOf(Operator.CLOSE_PARENTHESIS);
            return GetPowerFrom(stringOperand, (closeParenthesisIndex + 1));
        }

        //It gets and returns the power from the operand provided, starting at index specified
        //by the argument for the parameter of searchStartIndex.
        public static int GetPowerFrom(string stringOperand, int searchStartIndex = 0)
        {
            int powerSeparatorIndex = stringOperand.IndexOf(Operator.POWER_SEPERATOR, searchStartIndex);
            if (powerSeparatorIndex == -1) return GetIntFrom(DEFAULT_POWER);
            else return GetIntFrom(stringOperand.Substring(powerSeparatorIndex + 1));
        }

        //It accepts two numbers, one for natural number and the other for power
        //and returns powered number with those two numbers.
        //In the case of negative number to the power of n such as "-2^2", it will 
        //multiply the powered number with "-1" so that it will result in -4, not 4.
        public static int GetPoweredNumber(int number, int power, bool isNegative)
        {
            int poweredNumber = (int)Math.Pow(number, power);
            if (isNegative) poweredNumber *= -1;
            return poweredNumber;
        }

        //It extracts and returns the equation part from parenthesis
        public static string GetEquationInParenthesis(string parenthesis)
        {
            int openParenthesisIndex = parenthesis.IndexOf(Operator.OPEN_PARENTHESIS);
            int closeParenthesisIndex = parenthesis.IndexOf(Operator.CLOSE_PARENTHESIS);
            int equationStartIndex = openParenthesisIndex + 1;
            int equationCount = closeParenthesisIndex - equationStartIndex;
            string extractedEquation = parenthesis.Substring(equationStartIndex, equationCount);
            return extractedEquation;
        }

        //It gets and converts natural number part into integer number from variable of string passed 
        //as argument for the parameter of variableOperand.
        public static int GetNaturalNumberFromVariable(string variableOperand)
        {
            int variableIndex = variableOperand.ToUpper().IndexOf(Operator.VARIABLE);
            string numberPart = variableOperand.Substring(0, variableIndex);
            if (numberPart.Length == 0)
                numberPart = DEFAULT_NATURAL_NUMBER;
            else if (numberPart.Length == 1 && Operator.IsNonPrecedenceOperators(numberPart.ElementAt(0)))
                numberPart += DEFAULT_NATURAL_NUMBER;
            int number = GetIntFrom(numberPart);
            return GetIntFrom(numberPart);
        }

        //It converts and returns integer number from provided number of string.
        //It will throw an exception for invalid input such as "2a", "2.2 (float)".
        public static int GetIntFrom(string stringNumber)
        {
            int tempNumber = 0;
            if (!int.TryParse(stringNumber, out tempNumber))
                throw new InvalidFormatInNumberException();
            return tempNumber;
        }
    }
}
