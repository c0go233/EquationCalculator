using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equ
{
    class Program
    {
        private const string RESULT_PREFIX = "Result: X = ";
        private const string ERROR_PREFIX = "ERROR: ";
        private const string COMMAND_CALC = "CALC";
        private const string COMMAND_END = "END";
        private const string PROMPT = "Please enter equation or end to exit: ";
        private const string END_MESSAGE = "Thank you";
        private const int COMMAND_WORD_INDEX = 0;

        //It is starting point of this program. 
        //All the exceptions are caught in here and it will display
        //corresponding error messages onto the screen.
        static void Main(string[] args)
        {
            try
            {
                bool isStart = true, isEnd = false;
                string[] commandLine = args;
                do
                {
                    if (!isStart) commandLine = ReadCommandLine();
                    else if (isStart) isStart = false;
                    Console.WriteLine(ExecuteCommandLine(commandLine, ref isEnd));
                } while (!isEnd);
            }
            catch (OverflowException)
            {
                Console.WriteLine(ERROR_PREFIX + ExceptionMessage.INTEGER_OVERFLOW);
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine(ERROR_PREFIX + ExceptionMessage.DIVIDE_BY_ZERO);
            }
            catch (Exception e)
            {
                Console.WriteLine(ERROR_PREFIX + e.Message);
            }
        }
        
        //It execute corresponding methods to the commnad word 
        private static string ExecuteCommandLine(string[] commandLine, ref bool isEnd)
        {
            string commandWord = commandLine.ElementAt(COMMAND_WORD_INDEX).ToUpper();
            string result = END_MESSAGE;
            if (commandWord.Equals(COMMAND_END))
                isEnd = true;
            else if (commandWord.Equals(COMMAND_CALC))
                result = RESULT_PREFIX + DoCalculation(commandLine.Skip(1).ToArray());
            else
                throw new InvalidCommandException();
            return result;
        }

        //It triggers the calculation function of this program by calling
        //the SolveEquation() method of calculator object.
        private static string DoCalculation(string[] equation)
        {
            Calculator calculator = new Calculator();
            return calculator.SolveEquation(equation);
        }

        private static string[] ReadCommandLine()
        {
            Console.Write(PROMPT);
            return Console.ReadLine().Split(null);
        }

    }
}
