using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equ
{
    //This class hold the logics to calculate Operands with mathemitical operators.
    class OperandCalculator
    {
        //It is the central method to call all the neccesary function to calculate provided Operands 
        //based on the operator provided.
        public static List<Operand> GetCalculatedOperands(List<Operand> leftOperands, char operatorToCalc, List<Operand> rightOperands)
        {
            if (Operator.IsNonPrecedenceOperators(operatorToCalc))
                return CalculateNonPreOperators(leftOperands, operatorToCalc, rightOperands);
            else
                return CalculatePreOperators(leftOperands, operatorToCalc, rightOperands);
        }

        //It calculates operands with precedence operators such as *, /, %.
        //When the operator is /, *, then it calculates all the operands in the list of operand provided
        //such as (x+1)*(x+1) calculating to list of operands of x^2, 2x, -1
        private static List<Operand> CalculatePreOperators(List<Operand> leftOperands, char operatorToCalc, List<Operand> rightOperands)
        {
            List<Operand> calculatedOperands = new List<Operand>();
            List<Operand> finalOperands = new List<Operand>();
            for (int i = 0; i < leftOperands.Count(); i++)
            {
                Operand currentLeftOperand = leftOperands.ElementAt(i);
                for (int j = 0; j < rightOperands.Count(); j++)
                {
                    calculatedOperands.Add(CalculateOperand(currentLeftOperand, operatorToCalc,
                        rightOperands.ElementAt(j)));
                }
            }
            return CalculateNonPreOperators(finalOperands, Operator.PLUS, calculatedOperands);
        }

        //It calculates operands with non-precedence operators such as +, -.
        private static List<Operand> CalculateNonPreOperators(List<Operand> leftOperands, char operatorToCalc, List<Operand> rightOperands)
        {
            for (int i = 0; i < rightOperands.Count(); i++)
            {
                Operand currentRightOperand = rightOperands.ElementAt(i);
                int indexToPush = GetIndexOfSamePowerOperand(leftOperands, currentRightOperand.Power);
                PushToLeftOperands(leftOperands, indexToPush, operatorToCalc, currentRightOperand);
            }
            return leftOperands;
        }

        //It pushes the calculated "operand" to the list of operands based on the calculated operand's power.
        //If calculated operand is 2X and operand list has 2X, 2, 2X^2, then final result is 4X, 2, 2X^2 in the list.
        private static void PushToLeftOperands(List<Operand> leftOperands, int indexToPush, char operatorToCalc, Operand rightOperand)
        {
            if (indexToPush == -1)
                leftOperands.Add(CalculateOperand(new Operand(rightOperand.Power, 0), operatorToCalc, rightOperand));
            else
                leftOperands[indexToPush] = CalculateOperand(leftOperands.ElementAt(indexToPush), operatorToCalc, rightOperand);
        }

        //It performs actual mathmatical operation on operands provided based on the operator provided.
        private static Operand CalculateOperand(Operand leftOperand, char operatorToCalc, Operand rightOperand)
        {
            switch (operatorToCalc)
            {
                case Operator.PLUS: return leftOperand + rightOperand;
                case Operator.MINUS: return leftOperand - rightOperand;
                case Operator.DIVIDE: return leftOperand / rightOperand;
                case Operator.MULTIPLY: return leftOperand * rightOperand;
                case Operator.MODULUS: return leftOperand % rightOperand;
                default: return null;
            }
        }

        public static int GetIndexOfSamePowerOperand(List<Operand> finalOperands, int power)
        {
            return finalOperands.FindIndex(o => o.Power == power);
        }
    }
}
