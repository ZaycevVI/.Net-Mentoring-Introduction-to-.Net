using System;
using System.Linq;

namespace StringProcessing
{
    public static class StringExtensions
    {
        private const int StartAsciCode = 48;
        private const int EndAsciCode = 57;

        public static char FirstSymbol(this string input)
        {
            if(string.IsNullOrEmpty(input))
                throw new ArgumentException(
                    "Input string is null or empty.", nameof(input));

            return input[0];
        }

        public static int ToInt(this string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException(
                    "Input string is null or empty.", nameof(input));

            if(input.Any(s => s < StartAsciCode || s > EndAsciCode))
                throw new InvalidCastException("String contains invalid symbols.");

            checked
            {
                var number = 0;

                foreach (var symbol in input)
                {
                    number *= 10;
                    number += symbol - StartAsciCode;
                }

                return number;
            }
        }
    }
}
