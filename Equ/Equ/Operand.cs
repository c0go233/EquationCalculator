using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equ
{
    //Operand represents each number in the euqation and it has power and natural number.
    class Operand
    {
        public int Power { get; set; }
        public int NaturalNumber { get; set; }

        public Operand() { }

        public Operand(int power, int naturalNumber)
        {
            this.Power = power;
            this.NaturalNumber = naturalNumber;
        }

        public Operand(string operandToConvert)
        {
            ConverToOperand(operandToConvert);
        }

        //It convers the string into operand object based on what the string represents
        //,natural number or variable by calling ConvertToNaturalNumber() and ConvertToVariable()
        //respectively.
        private void ConverToOperand(string operandToConvert)
        {
            if (operandToConvert.ToUpper().Contains(Operator.VARIABLE))
                ConvertToVariable(operandToConvert);
            else
                ConvertToNaturalNumber(operandToConvert);
        }

        //It converts string expression into Operand object
        //It calculates power associated with the number like 2^2 will be converted into 4.
        private void ConvertToNaturalNumber(string operandToConvert)
        {
            bool isNegative = operandToConvert.ElementAt(0).Equals(Operator.MINUS);
            int numberPart = OperandConverter.GetNumberPartFromNaturalNumber(operandToConvert, (isNegative) ? 1 : 0);
            int powerPart = OperandConverter.GetPowerFrom(operandToConvert);
            this.NaturalNumber = OperandConverter.GetPoweredNumber(numberPart, powerPart, isNegative);
        }

        //It converts string expression into Operand object of variable.
        //In the case of 2x^0, it will be treated as 1.
        private void ConvertToVariable(string operandToConvert)
        {
            this.NaturalNumber = OperandConverter.GetNaturalNumberFromVariable(operandToConvert);
            this.Power = OperandConverter.GetPowerFrom(operandToConvert);
        }

        //It overloads "+" operator and plus only the natural numbers of provided operands
        public static Operand operator +(Operand firstOperand, Operand secondOperand)
        {
            int calculatedNaturalNumber;
            checked { calculatedNaturalNumber = firstOperand.NaturalNumber + secondOperand.NaturalNumber; }
            return new Operand(firstOperand.Power, calculatedNaturalNumber);
        }

        //It overloads "-" operator and minus only the natural numbers of provided operands
        public static Operand operator -(Operand firstOperand, Operand secondOperand)
        {
            int calculatedNaturalNumber;
            checked { calculatedNaturalNumber = firstOperand.NaturalNumber - secondOperand.NaturalNumber; }
            return new Operand(firstOperand.Power, calculatedNaturalNumber);
        }

        //It overloads "*" operator and plus both the natural numbers and powers of provided operands
        public static Operand operator *(Operand firstOperand, Operand secondOperand)
        {
            int naturalNumber;
            checked { naturalNumber = firstOperand.NaturalNumber * secondOperand.NaturalNumber; }
            int power = firstOperand.Power + secondOperand.Power;
            return new Operand(power, naturalNumber);
        }

        //It overloads "/" operator and minus both the natural numbers and powers of provided operands
        public static Operand operator /(Operand firstOperand, Operand secondOperand)
        {
            int naturalNumber;
            checked { naturalNumber = firstOperand.NaturalNumber / secondOperand.NaturalNumber; }
            int power = firstOperand.Power - secondOperand.Power;
            return new Operand(power, naturalNumber);
        }

        //It overloads "%" operator and modulus only the natural numbers of provided operands
        //It will throw an exeption when either of operands is variable 
        public static Operand operator %(Operand firstOperand, Operand secondOperand)
        {
            if (firstOperand.Power != 0 || secondOperand.Power != 0)
                throw new InvalidOperationModulusVariableException();
            int natrualNumber = firstOperand.NaturalNumber % secondOperand.NaturalNumber;
            return new Operand(firstOperand.Power, natrualNumber);
        }
    }
}
