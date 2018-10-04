using System;
using Library;

namespace IntroductionToNet
{
    class Program
    {
        private const string QuestionMessage = "Pls, enter your name:";

        static void Main(string[] args)
        {
            Console.WriteLine(QuestionMessage);
            var name = Console.ReadLine();
            Console.WriteLine(Helper.GenerateMessage(name));
        }
    }
}
