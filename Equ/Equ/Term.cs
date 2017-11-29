using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equ
{
    //Term is a collection of Operands. It can hold a single operand or 
    //a number of operands, which means parenthesis.
    class Term
    {
        public const int DEFAULT_OPERAND_INDEX = 0;
        public List<Operand> operands { get; set; } = new List<Operand>();

        public Term(Operand operand)
        {
            operands.Add(operand);
        }

        public Term(string termToConvert)
        {
            ConvertToTerm(termToConvert);
        }

        //It converts string expression into Term object.
        private void ConvertToTerm(string termToConvert)
        {
            if (termToConvert.Contains(Operator.OPEN_PARENTHESIS))
                ConvertFromParenthesis(termToConvert);
            else
                operands.Add(new Operand(termToConvert));
        }

        //It converts parenthesis into Term object that will holding a number of Operands.
        //In the case of (x+1)^0, it will treat it as "1".
        private void ConvertFromParenthesis(string parenthesis)
        {
            int power = OperandConverter.GetPowerFromParenthesis(parenthesis);
            bool isNegative = parenthesis.ElementAt(0) == Operator.MINUS;
            if (power == 0)
                this.operands = GetDefaultOperand(isNegative);
            else
                this.operands = GetCalculatedParenthesis(parenthesis, power, isNegative);
        }

        //It calculates the euqation in the parenthesis provided.
        //In the case of (x+2)^n, it will multiply (x+2) n times.
        private List<Operand> GetCalculatedParenthesis(string parenthesis, int power, bool isNegative)
        {
            string equationInParenthesis = OperandConverter.GetEquationInParenthesis(parenthesis);
            List<Operand> calculatedParenthesis = new TermCalculator().GetCalculatedEquation(equationInParenthesis);
            List<Operand> finalParenthesis = calculatedParenthesis;
            for (int i = 1; i < power; i++)
            {
                finalParenthesis = OperandCalculator.GetCalculatedOperands(finalParenthesis, Operator.MULTIPLY, calculatedParenthesis);
            }
            if (isNegative)
                finalParenthesis = OperandCalculator.GetCalculatedOperands(GetDefaultOperand(isNegative), Operator.MULTIPLY, finalParenthesis);
            return finalParenthesis;
        }

        //It compares all the operands held in the lists of operands provided through parameters and 
        //returns true when they are all the same.
        public static bool CompareOperands(List<Operand> leftOperands, List<Operand> rightOperands)
        {
            if (leftOperands.Count() != rightOperands.Count())
                return false;
            for (int i = 0; i < leftOperands.Count(); i++)
            {
                Operand leftOperand = leftOperands.ElementAt(i);
                Operand rightOperand = rightOperands.ElementAt(i);
                if (leftOperand.Power != rightOperand.Power)
                    return false;
                if (leftOperand.NaturalNumber != rightOperand.NaturalNumber)
                    return false;
            }
            return true;
        }

        //It returns list of operand holding only one operand to be used to
        //multiply with to make operands negative.
        private List<Operand> GetDefaultOperand(bool isNegative)
        {
            int naturalNumber = (isNegative) ? -1 : 1;
            return new List<Operand> { new Operand(0, naturalNumber) };
        }

        public int GetCount()
        {
            return this.operands.Count();
        }

        public static bool operator ==(Term leftTerm, Term rightTerm)
        {
            return CompareOperands(leftTerm.operands, rightTerm.operands);
        }

        public static bool operator !=(Term leftTerm, Term rightTerm)
        {
            return !(leftTerm == rightTerm);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


    }
}
