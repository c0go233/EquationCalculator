using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equ
{
    //This class holds all the custom exceptions that might be thrown in the operation
    //of this calculator.
    class ExceptionMessage
    {
        public const string DIVIDE_BY_ZERO = "Division by zero is invalid";
        public const string INTEGER_OVERFLOW = "Out of integer range";
    }

    class InvalidFormatInEqualSignException : Exception
    {
        public InvalidFormatInEqualSignException()
            : base("Invalid format in equal sign is found") { }
    }

    class InvalidFormatNoVariableException : Exception
    {
        public InvalidFormatNoVariableException()
            : base("There should be at least one variable to solve on") { }
    }

    class InvalidFormatInOperatorException : Exception
    {
        public InvalidFormatInOperatorException()
            : base("Invalid format in operators is found") { }
    }

    class InvalidFormatInParenthesisException : Exception
    {
        public InvalidFormatInParenthesisException()
            : base("Invalid format in parenthesis is found") { }
    }

    class InvalidFormatInNumberException : Exception
    {
        public InvalidFormatInNumberException()
            : base("Invalid format in number is found") { }
    }

    class InvalidOperationModulusVariableException : Exception
    {
        public InvalidOperationModulusVariableException()
            : base("Invalid operation of modulus with variable is found") { }
    }

    class InvalidOperationFunctionException : Exception
    {
        public InvalidOperationFunctionException()
            : base("Invalid function with power of more than 2 is found") { }
    }

    class InvalidImaginaryQuadraticFunctionException : Exception
    {
        public InvalidImaginaryQuadraticFunctionException()
            : base("No support for imaginary value from quadratic function") { }
    }

    class InvalidParenthesisDivisionException : Exception
    {
        public InvalidParenthesisDivisionException()
            : base("Invalid parenthesis division is found") { }
    }

    class InvalidCommandException : Exception
    {
        public InvalidCommandException()
            : base("Invalid command is found") { }
    }
}
