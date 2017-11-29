using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equ
{
    class Calculator
    {
        //It is the central method to call all the neccessary functions to solve equation.
        //It returns calculated result as string to be displayed onto screen
        public string SolveEquation(string[] equation)
        {
            string stringEquation = String.Join(String.Empty, equation).ToString();
            ValidateThereIsVariableIn(stringEquation);
            string[] separatedEquations = GetSeparatedEquationsBy(Operator.EQUAL, stringEquation);
            List<Operand> calculatedEquation = new TermCalculator().GetCalculatedEquation(separatedEquations);
            return FunctionCalculator.GetResultFromFunction(calculatedEquation);
        }

        //It validates if there is at least one variable in the equation provided
        //If not, then it will throw an exception 
        private void ValidateThereIsVariableIn(string equation)
        {
            if (!equation.ToUpper().Contains(Operator.VARIABLE))
                throw new InvalidFormatNoVariableException();
        }

        //It separates equations by specific operator passed through the parameter
        //of operatorToSeparate and returns separated equation as string array
        private string[] GetSeparatedEquationsBy(char operatorToSeparate, string equation)
        {
            string[] separatedEquations = equation.Split(Operator.EQUAL);
            ValidateSeparatedEquations(separatedEquations);
            return separatedEquations;
        }

        //It validates if there are two equations seprated by (=)operator from original user input equation.
        //It throws an exception for error cases: "=x+2", "x+2=", "x+2=2+2=", "x+2".
        private void ValidateSeparatedEquations(string[] equation)
        {
            if (equation.Count() == 1 || equation.Count() > 2
                || String.IsNullOrEmpty(equation.ElementAt(0)) || String.IsNullOrEmpty(equation.ElementAt(1)))
                throw new InvalidFormatInEqualSignException();
        }
    }
}
