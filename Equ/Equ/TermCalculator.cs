using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equ
{
    //This class holds the logics to calculate Terms.
    class TermCalculator
    {
        public const int FIRST_OPERATOR_INDEX = 0, SECOND_OPERATOR_INDEX = 1;
        public const int LEFT_OPERANDS_INDEX = 0, RIGHT_OPERANDS_INDEX = 1;

        //It is the entry point method to get the calculated Equation
        public List<Operand> GetCalculatedEquation(string[] equations)
        {
            return CalculateEquation(equations);
        }

        //It is the overload method of the entry point method to get the calculated euqation.
        //It is called to get calculated equation for parenthesis which has only one equation.
        public List<Operand> GetCalculatedEquation(string equation)
        {
            string[] arrayEquation = { equation };
            return CalculateEquation(arrayEquation);
        }

        //It is the central method to call all the neccesary methods to return the calculated equation.
        //It calls methods to organize, calculate, eliminate parenthesis denominator, and transpose equation.
        private List<Operand> CalculateEquation(string[] equations)
        {
            List<List<Term>> terms = GenerateLists<Term>(equations.Count());
            List<List<char>> operators = GenerateLists<char>(equations.Count());
            List<List<Operand>> finalEquations = GenerateLists<Operand>(equations.Count());
            OrganizeEquation(terms, operators, equations);
            if (equations.Length > 1)
                SimplifyParenthesisDenominators(terms, operators);
            for (int i = 0; i < finalEquations.Count(); i++)
            {
                finalEquations[i] = PerformCalculation(terms[i], operators[i]);
            }
            return TransposeEquation(finalEquations);
        }

        //It is the central method to cal all the neccessary methods to calculate
        //Terms and Operators provided through parameters. 
        //The actual calculation of operands performed by OperandCalculator class.
        private List<Operand> PerformCalculation(List<Term> terms, List<char> operators)
        {
            while (terms.Count() > 1)
            {
                int operatorIndex = GetOperatorIndexToCalculate(operators);
                char operatorToCalc = operators.ElementAt(operatorIndex);
                Term leftTerm = terms.ElementAt(operatorIndex);
                Term rightTerm = terms.ElementAt((operatorIndex + 1));
                leftTerm.operands = OperandCalculator.GetCalculatedOperands(leftTerm.operands, operatorToCalc, rightTerm.operands);
                UpdateEquation(terms, operators, operatorIndex);
            }
            return terms.ElementAt(0).operands;
        }

        //It updates the Term and Operator lists by removing the calculated
        //Terms and Opertors. 
        private void UpdateEquation(List<Term> terms, List<char> operators, int operatorIndex)
        {
            operators.RemoveAt(operatorIndex);
            terms.RemoveAt((operatorIndex + 1));
        }

        //It compares the first two operators in the list passed as parameter and
        //returns index of operator that should be first calculated over the other.
        private int GetOperatorIndexToCalculate(List<char> operators)
        {
            int finalOperatorIndex = FIRST_OPERATOR_INDEX;
            if (IsPrecedenceOnSecondOperator(operators))
                finalOperatorIndex = SECOND_OPERATOR_INDEX;
            return finalOperatorIndex;
        }

        //It compares the first two operators in the list provided through paramter and
        //returns when second operator has preference over first one.
        private bool IsPrecedenceOnSecondOperator(List<char> operators)
        {
            return (SECOND_OPERATOR_INDEX < operators.Count &&
                Operator.IsNonPrecedenceOperators(operators.ElementAt(FIRST_OPERATOR_INDEX)) &&
                Operator.IsPrecedenceOperators(operators.ElementAt(SECOND_OPERATOR_INDEX)));
        }

        //It organize the the euqation provided through parameter by calling
        //EquationOrganizer class.
        private void OrganizeEquation(List<List<Term>> terms, List<List<char>> operators, string[] equations)
        {
            for (int i = 0; i < equations.Count(); i++)
            {
                new EquationOrganizer().OrganizeEquation(equations.ElementAt(i),
                    terms.ElementAt(i), operators.ElementAt(i));
            }
        }

        //It transpose the calculated euqation at one side to the other.
        //For example: x+1 = 2 goes to x+1-2 = 0 and to x-1 = 0;
        private List<Operand> TransposeEquation(List<List<Operand>> finalOperands)
        {
            if (finalOperands.Count() > 1)
                finalOperands[LEFT_OPERANDS_INDEX] = OperandCalculator.GetCalculatedOperands(
                    finalOperands.ElementAt(LEFT_OPERANDS_INDEX),
                    Operator.MINUS, finalOperands.ElementAt(RIGHT_OPERANDS_INDEX));
            return finalOperands.ElementAt(LEFT_OPERANDS_INDEX);
        }

        //It checks if there is parenthesis denominator in the equation and if there is,
        //then it calls RemoveDenominatorsByMultiply() to remove it.
        private void SimplifyParenthesisDenominators(List<List<Term>> terms, List<List<char>> operators)
        {
            List<Term> denominators = GetParenthesisDenominators(terms, operators);
            if (denominators.Count() > 0)
            {
                denominators.Reverse();
                RemoveDenominatorsByMultiply(terms, operators, denominators);
            }
        }

        //This method is actual entry point of eliminating parenthesis denominator
        //It calls all the neccessary functions to remove parenthesis denominators in the euqation.
        private void RemoveDenominatorsByMultiply(List<List<Term>> terms, List<List<char>> operators, List<Term> denominators)
        {
            for (int i = 0; i < terms.Count(); i++)
            {
                List<Term> currentTerms = terms.ElementAt(i);
                List<char> currentOperators = operators.ElementAt(i);
                MultiplyParenthesisDenominators(currentTerms, currentOperators, denominators);
                TryRemoveParenthesisDenominators(currentTerms, currentOperators);
            }
        }

        //It finds the index of multiplied parentehsis denominator 
        private int GetCounterDenominatorIndex(List<Term> terms, List<char> operators, int indexToStart)
        {
            int nearestNonPreOperator = operators.FindIndex(indexToStart, o => Operator.IsNonPrecedenceOperators(o));
            int indexToEnd = (nearestNonPreOperator == -1) ? (operators.Count() - 1) : (nearestNonPreOperator - 1);
            Term termToCompare = terms.ElementAt((indexToStart + 1));
            for (int i = indexToEnd; i > indexToStart; i--)
            {
                if (operators.ElementAt(i).Equals(Operator.MULTIPLY) && termToCompare == terms.ElementAt((i + 1)))
                    return i;
            }
            return -1;
        }

        //It multiplies the parethesis denominators found in the equation in each term separated by "+" or "-".
        //For example, "(x+1)/(x-1) = 2 + 3", it will go to "(x+1)/(x-1)*(x-1) = 2*(x-1) + 3*(x-1)". 
        private void MultiplyParenthesisDenominators(List<Term> terms, List<char> operators, List<Term> denominators)
        {
            operators.Add(Operator.PLUS);
            for (int i = (operators.Count() - 1); i >= 0; i--)
            {
                if (Operator.IsNonPrecedenceOperators(operators.ElementAt(i)))
                    InsertParenthesisDenominator(terms, operators, denominators, i);
            }
            operators.RemoveAt((operators.Count() - 1));
        }

        //It inserts multiply operator (*) into the list of operators and parentehsis denominator into the 
        //list of terms at corresponding index. 
        private void InsertParenthesisDenominator(List<Term> terms, List<char> operators, List<Term> denominators, int indexToPush)
        {
            for (int i = 0; i < denominators.Count(); i++)
            {
                operators.Insert(indexToPush, Operator.MULTIPLY);
                terms.Insert((indexToPush + 1), denominators.ElementAt(i));
            }
        }

        //It gets all the parenthesis denominators in the euqation into a list of Term
        //and returns the list to be used for the elimination of the denominators.
        private List<Term> GetParenthesisDenominators(List<List<Term>> terms, List<List<char>> operators)
        {
            List<Term> denominators = new List<Term>();
            for (int i = 0; i < terms.Count(); i++)
            {
                AddParenthesisDenominatorsTo(denominators, terms.ElementAt(i), operators.ElementAt(i));
            }
            return denominators;
        }

        //It adds all the parenthesis denominators found in the euqation to the
        //provided list as parameter.
        private void AddParenthesisDenominatorsTo(List<Term> denominators, List<Term> terms, List<char> operators)
        {
            for (int i = 0; i < operators.Count(); i++)
            {
                if (IsParenthesisDenominator(terms, operators, i))
                    denominators.Add(terms.ElementAt(i + 1));
            }
        }

        //It returns true if the operator of divide (/) is followed by parenthesis such as (x+1)
        //which means parenthesis denominator.
        private bool IsParenthesisDenominator(List<Term> terms, List<char> operators, int indexToTest)
        {
            char currentOperator = operators.ElementAt(indexToTest);
            Term currentTerm = terms.ElementAt((indexToTest + 1));
            return (currentOperator.Equals(Operator.DIVIDE) && currentTerm.GetCount() > 1);
        }

        //It finds parenthesis denominator in the equation and calls 
        //RemoveParenthesisDenominator() to actually eliminate the denominator.
        private void TryRemoveParenthesisDenominators(List<Term> terms, List<char> operators)
        {
            for (int i = (operators.Count() - 1); i >= 0; i--)
            {
                if (IsParenthesisDenominator(terms, operators, i))
                    RemoveParenthesisDenominator(terms, operators, i);
            }
        }

        //It removes parenthesis denominator at index provided through parameter as well as
        //its counter denominator multiplied. In the case of "(x+2)/(x-1)*(x-1) = 2*(x-1)",
        //"/(x-1)*(x-1)" will be removed.
        private void RemoveParenthesisDenominator(List<Term> terms, List<char> operators, int indexToRemove)
        {
            int counterDenominatorIndex = GetCounterDenominatorIndex(terms, operators, indexToRemove);
            if (counterDenominatorIndex < 0)
                throw new InvalidParenthesisDivisionException();
            RemoveTermAndOperatorAt(terms, operators, counterDenominatorIndex);
            RemoveTermAndOperatorAt(terms, operators, indexToRemove);
        }

        //It removes combination of Term and Operator at specific index.
        private void RemoveTermAndOperatorAt(List<Term> terms, List<char> operators, int indexToRemove)
        {
            int indexOfTermToRemove = indexToRemove + 1;
            operators.RemoveAt(indexToRemove);
            terms.RemoveAt(indexOfTermToRemove);
        }

        private List<List<T>> GenerateLists<T>(int numberOfList)
        {
            List<List<T>> lists = new List<List<T>>();
            for (int i = 0; i < numberOfList; i++)
            {
                lists.Add(new List<T>());
            }
            return lists;
        }
    }
}
