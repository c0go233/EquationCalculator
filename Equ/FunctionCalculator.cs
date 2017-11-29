using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equ
{
    //This class holds the logics to calulcate various functions like linear and quadratic style functions.
    class FunctionCalculator
    {
        private const int ZERO = 0, NEGATIVE_ONE = -1, ONE = 1, TWO = 2;
        private const string RESULT_COMMA = ", ";

        //It is the central method to call all the neccesary methods to calculate 
        //provided function
        public static string GetResultFromFunction(List<Operand> function)
        {
            function.RemoveAll(o => o.NaturalNumber == ZERO);
            if (function.Count() <= 0)
                throw new InvalidFormatNoVariableException();
            List<Operand> nonDenominatorFunction = EliminateVariableDenominator(function);
            int highestPower = nonDenominatorFunction.OrderBy(o => o.Power).Last().Power;
            return CalculateFunction(nonDenominatorFunction, highestPower);
        }

        //It calls right methods to calculate provided function based on its highest power.
        //It can easily extend to handle other functions like cubic function by putting
        //case 3: CalculateCubicFunction(); in switch statement.
        private static string CalculateFunction(List<Operand> function, int highestPower)
        {
            switch (highestPower)
            {
                case ZERO: throw new InvalidFormatNoVariableException();
                case ONE: return CalculateLinearFunction(function);
                case TWO: return CalculateQuadraticFunction(function);
                default: throw new InvalidOperationFunctionException();
            }
        }

        //It checks if there is variable denominator such as (x^-1) and eliminates it by multiplying it with 
        //its counter variable so that the equation can be easily calculated.
        private static List<Operand> EliminateVariableDenominator(List<Operand> function)
        {
            for (int i = 0; i < function.Count(); i++)
            {
                Operand currentOperand = function.ElementAt(i);
                if (currentOperand.Power < ZERO)
                    function = OperandCalculator.GetCalculatedOperands(function, Operator.MULTIPLY,
                        new List<Operand>() { new Operand((currentOperand.Power * NEGATIVE_ONE), ONE) });
            }
            return function;
        }

        //It gets the number from Operand based on the power provided as parameter
        //and returns it to be further calculated in the calling method.
        private static int GetNumberFromOperandFor(List<Operand> function, int power)
        {
            int indexOfOperand = OperandCalculator.GetIndexOfSamePowerOperand(function, power);
            return (indexOfOperand == NEGATIVE_ONE) ? ZERO : function.ElementAt(indexOfOperand).NaturalNumber;
        }

        //It calculates Quadratic function by its formular.
        //PowerTwoNumber, PowerOneNumber, and PowerZeroNumber represent
        //a, b, and c in function ax^2 + bx + c (a != 0).
        private static string CalculateQuadraticFunction(List<Operand> function)
        {
            int powerZeroNumber = GetNumberFromOperandFor(function, ZERO);
            int powerOneNumber = GetNumberFromOperandFor(function, ONE);
            int powerTwoNumber = GetNumberFromOperandFor(function, TWO);
            return GetResultFromQuadraticFunction(powerZeroNumber, powerOneNumber, powerTwoNumber);
        }

        //It checks if there is actual value from the quadratic function provided
        //It will throw an exception for the case of the function that requires imaginary result.
        private static string GetResultFromQuadraticFunction(int powerZeroNumber, int powerOneNumber, int powerTwoNumber)
        {
            int determinant = ((int)Math.Pow(powerOneNumber, 2) - (4 * powerTwoNumber * powerZeroNumber));
            string result = "";
            if (determinant < ZERO)
                throw new InvalidImaginaryQuadraticFunctionException();
            result = PerformQuadraticFormula(powerOneNumber, powerTwoNumber, determinant, Operator.PLUS).ToString();
            if (determinant > ZERO)
                result += RESULT_COMMA + PerformQuadraticFormula(powerOneNumber, powerTwoNumber, determinant, Operator.MINUS).ToString();
            return result;
        }

        //It calculates quadratic formular based on the numbers as parameters.
        private static double PerformQuadraticFormula(int powerOneNumber, int powerTwoNumber, int determinant, char operatorToCalc)
        {
            double squaredDeterminant = Math.Sqrt(determinant);
            int negativePowerOne = powerOneNumber * NEGATIVE_ONE;
            int denominator = TWO * powerTwoNumber;
            double numerator = (operatorToCalc == Operator.PLUS) ?
                negativePowerOne + squaredDeterminant : negativePowerOne - squaredDeterminant;
            return numerator / denominator;
        }

        private static string CalculateLinearFunction(List<Operand> function)
        {
            int powerZeroNumber = GetNumberFromOperandFor(function, ZERO);
            int powerOneNumber = GetNumberFromOperandFor(function, ONE);
            double result = (powerZeroNumber * NEGATIVE_ONE) / (double)powerOneNumber;
            return result.ToString();
        }
    }
}
