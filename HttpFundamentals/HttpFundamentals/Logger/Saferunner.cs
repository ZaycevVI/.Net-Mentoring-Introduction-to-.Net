using System;

namespace HttpFundamentals.Logger
{
    public static class SafeRunner
    {
        public static void Run(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static T Run<T>(Func<T> action)
        {
            var result = default(T);

            try
            {
                result = action.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }
    }
}
