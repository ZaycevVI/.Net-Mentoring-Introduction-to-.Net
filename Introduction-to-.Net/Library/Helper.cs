using System;

namespace Library
{
    public static class Helper
    {
        public static string GenerateMessage(string name)
        {
            return $"{DateTime.Now} \t Hello, {name}!";
        }
    }
}
