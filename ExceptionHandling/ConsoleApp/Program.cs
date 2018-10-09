using System;
using StringProcessing;

namespace ConsoleApp
{
    class Program
    {
        private const string EnterString = "Pls, enter custom string:";
        private const string EnterNumber = "Pls, enter custom integer number:";
        private const string ResultTemplate = "First symbols of string is: {0}";
        private const string ResultNumberTemplate = "Result number is: {0}";

        static void Main(string[] args)
        {
            bool repeat;
            Action<string>[] action = { FirstSymbolWorkflow, ParseToIntWorkflow };

            do
            {
                Console.Clear();
                Console.WriteLine("Enter one point from menu below:\n1 - First symbol;\netc - Parse to integer;");
                var menu = Console.ReadLine() == "1" ? 0 : 1;

                Console.Clear();
                Console.WriteLine(menu == 0 ? EnterString : EnterNumber);
                var input = Console.ReadLine();
                action[menu](input);
                Console.WriteLine("Would you like to repeat(y - yes, else - othes)?");
                repeat = (Console.ReadLine() == "y");
            } while (repeat);
        }

        private static void FirstSymbolWorkflow(string input)
        {
            try
            {
                var symbol = input.FirstSymbol();
                Console.WriteLine(ResultTemplate, symbol);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void ParseToIntWorkflow(string input)
        {
            try
            {
                var number = input.ToInt();
                Console.WriteLine(ResultNumberTemplate, number);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
