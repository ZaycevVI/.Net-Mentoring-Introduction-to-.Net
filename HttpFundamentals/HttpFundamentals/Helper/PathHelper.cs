namespace HttpFundamentals.Helper
{
    public static class PathHelper
    {
        public static string ReplaceSlashes(this string path)
        {
            return path.Replace("/", "\\");
        }
    }
}