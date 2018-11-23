namespace HttpFundamentals.Html.FileSystem
{
    public interface IFileWrapper
    {
        void CreateHtmlFile(string url, string content);
        void CreateFile(string url, string rootUrl);
    }
}