using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equ
{
    //This class holds the const variables for operators used in this calculator and 
    //operations related to them.
    class Operator
    {
        public const char VARIABLE = 'X', POWER_SEPERATOR = '^', EQUAL = '=', PLUS = '+', MINUS = '-',
            MULTIPLY = '*', DIVIDE = '/', MODULUS = '%', OPEN_PARENTHESIS = '(', CLOSE_PARENTHESIS = ')';

        //It returns true when the arguement provided through parameter is not operators and open parenthesis
        public static bool IsValidToPushMultiplyBefore(char operatorToValidate)
        {
            return (IsOperators(operatorToValidate) || operatorToValidate == OPEN_PARENTHESIS) ? false : true;
        }

        //It returns true when the arguement provided through parameter is not operators, close parenthesis,
        //and power operator (^)
        public static bool IsValidToPushMultiplyAfter(char operatorToValidate)
        {
            return (IsOperators(operatorToValidate) || operatorToValidate == CLOSE_PARENTHESIS ||
                operatorToValidate == POWER_SEPERATOR) ? false : true;
        }

        //It operates multiplication between plus and minus
        //It returns - for "+*-" and + "-*-" and "+*+"
        public static char MultiplyNonPrecedenceOperator(char firstOperator, char secondOperator)
        {
            if (firstOperator == secondOperator) return PLUS;
            else return MINUS;
        }

        public static bool IsOperators(char operatorToValidate)
        {
            return (IsNonPrecedenceOperators(operatorToValidate)
                || IsPrecedenceOperators(operatorToValidate)) ? true : false;
        }

        public static bool IsNonPrecedenceOperators(char operatorToValidate)
        {
            return (operatorToValidate == PLUS || operatorToValidate == MINUS) ? true : false;
        }

        public static bool IsPrecedenceOperators(char operatorToValidate)
        {
            return (operatorToValidate == MULTIPLY || operatorToValidate == DIVIDE || operatorToValidate == MODULUS) ? true : false;
        }
    }
}
