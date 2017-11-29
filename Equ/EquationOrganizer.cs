using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equ
{
    //This class is responsible for organizing equation provided by users converting it 
    //into two lists of objects, Term and char. Term is for number or parenthesis expression and
    //char is for operators like (+,-,*,%,/)
    class EquationOrganizer
    {
        private const string ZERO = "0";

        //It is the central method to call all the neccesary functions to organize equation
        public void OrganizeEquation(string equationToOrganize, List<Term> terms, List<char> operators)
        {
            ValidateEachEndOperands(ref equationToOrganize);
            EliminateSuccessiveOperators(ref equationToOrganize);
            SubstituteBlankWithMultiply(ref equationToOrganize);
            SortOutEquation(equationToOrganize, terms, operators);
        }

        //It sorts out the provided euqation to a list of Term representing parenthesis or number and 
        //list of char representing operators 
        private void SortOutEquation(string equation, List<Term> terms, List<char> operators)
        {
            string tempTerm = "";
            for (int i = 0; i < equation.Length; i++)
            {
                char currentTerm = equation.ElementAt(i);
                if (currentTerm.Equals(Operator.OPEN_PARENTHESIS))
                    tempTerm += ExtractParenthesis(ref equation, i);
                else if (Operator.IsOperators(currentTerm))
                    PushTempTermToList(ref tempTerm, currentTerm, terms, operators);
                else
                    tempTerm += currentTerm;
            }
            terms.Add(new Term(tempTerm));
        }

        //It adds Term and Operator provided through parameters to their corresponding list and
        //reset the tempTerm to empty string so that next operand can be stored.
        private void PushTempTermToList(ref string tempTerm, char operatorToPush, List<Term> terms, List<char> operators)
        {
            if (Operator.IsNonPrecedenceOperators(operatorToPush) && String.IsNullOrEmpty(tempTerm))
                tempTerm = operatorToPush.ToString();
            else
            {
                terms.Add(new Term(tempTerm));
                operators.Add(operatorToPush);
                tempTerm = "";
            }
        }

        //It extracts parenthesis expression and returns it from provided equation expression.
        //It will throw an excetion for cases: (x+1 + (x+1) = , (x+1+2 =.
        private string ExtractParenthesis(ref string equation, int openParenthesisIndex)
        {
            int searchStartIndex = openParenthesisIndex + 1;
            int closeParenthesisIndex = equation.IndexOf(Operator.CLOSE_PARENTHESIS, searchStartIndex);
            if (closeParenthesisIndex == -1)
                throw new InvalidFormatInParenthesisException();
            int nextOpenParenthesisIndex = equation.IndexOf(Operator.OPEN_PARENTHESIS, searchStartIndex);
            if (nextOpenParenthesisIndex != -1 && nextOpenParenthesisIndex < closeParenthesisIndex)
                throw new InvalidFormatInParenthesisException();
            int parenthesisCount = (closeParenthesisIndex + 1) - openParenthesisIndex;
            string extractedParenthesis = equation.Substring(openParenthesisIndex, parenthesisCount);
            equation = equation.Remove(openParenthesisIndex, (parenthesisCount - 1));
            return extractedParenthesis;
        }

        //It removes successive operators by multiplying them 
        //It calls its sub function of TryEliminateSuccessiveOperator
        //only when an operator matches with + or -.
        private void EliminateSuccessiveOperators(ref string equation)
        {
            List<char> charArrayEquation = equation.ToList();
            for (int i = 0; i < charArrayEquation.Count(); i++)
            {
                char currentOperator = charArrayEquation.ElementAt(i);
                if (Operator.IsNonPrecedenceOperators(currentOperator))
                    TryEliminateSuccessiveOperator(charArrayEquation, i);
            }
            equation = String.Join(String.Empty, charArrayEquation).ToString();
        }

        //It validates if the next operator is non-precedence operators (+,-) to the provided
        //operator so that it can multiply them to eliminate successive operators.
        private void TryEliminateSuccessiveOperator(List<char> equation, int currentIndex)
        {
            int nextIndex = currentIndex + 1;
            while (Operator.IsOperators(equation.ElementAt(nextIndex)))
            {
                char nextOperator = equation.ElementAt(nextIndex);
                if (Operator.IsPrecedenceOperators(nextOperator))
                    throw new InvalidFormatInOperatorException();
                else
                {
                    equation[currentIndex]
                        = Operator.MultiplyNonPrecedenceOperator(equation.ElementAt(currentIndex), nextOperator);
                    equation.RemoveAt(nextIndex);
                }
            }
        }

        //It determines where to put multiply operator based on after or before what operand
        //the operator is pushed.
        private void SubstituteBlankWithMultiply(ref string equation)
        {
            for (int i = 0; i < equation.Count(); i++)
            {
                char currentTerm = equation.ElementAt(i);
                if (Char.ToUpper(currentTerm).Equals(Operator.VARIABLE))
                    PushMultiply(ref equation, i, false, true);
                else if (currentTerm.Equals(Operator.OPEN_PARENTHESIS))
                    PushMultiply(ref equation, i, true, false);
                else if (currentTerm.Equals(Operator.CLOSE_PARENTHESIS))
                    PushMultiply(ref equation, i, false, true);
            }
        }

        //It puts multiply (*) operators in the place where it is ommitted.
        //It covers putting the operator between number and open parenthesis "(" and
        //after variable and close parenthesis like ")2" --> ")*2" and "x2" --> "x*2"
        private void PushMultiply(ref string equation, int indexToPush, bool pushBefore, bool pushAfter)
        {
            int beforeIndex = indexToPush - 1;
            int afterIndex = indexToPush + 1;
            if (pushAfter && afterIndex < equation.Count() &&
                Operator.IsValidToPushMultiplyAfter(equation.ElementAt(afterIndex)))
                equation = equation.Insert(afterIndex, Operator.MULTIPLY.ToString());

            if (pushBefore && beforeIndex >= 0 &&
                Operator.IsValidToPushMultiplyBefore(equation.ElementAt(beforeIndex)))
                equation = equation.Insert(indexToPush, Operator.MULTIPLY.ToString());
        }

        //It add "0" when plus and minus operators are at start or end
        //It throws an exception when other operators are at start or end.
        private void ValidateEachEndOperands(ref string equation)
        {
            if (Operator.IsNonPrecedenceOperators(equation.ElementAt(0)))
                equation = equation.Insert(0, ZERO);
            else if (Operator.IsPrecedenceOperators(equation.ElementAt(0)))
                throw new InvalidFormatInOperatorException();
            int lastIndex = equation.Count() - 1;
            if (Operator.IsNonPrecedenceOperators(equation.ElementAt(lastIndex)) ||
                equation.ElementAt(lastIndex).Equals(Operator.MULTIPLY))
                equation += ZERO;
            else if (Operator.IsPrecedenceOperators(equation.ElementAt(lastIndex)))
                throw new InvalidFormatInOperatorException();
        }
    }
}
