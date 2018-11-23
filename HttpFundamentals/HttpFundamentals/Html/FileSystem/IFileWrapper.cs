namespace HttpFundamentals.Html.FileSystem
{
    public interface IFileWrapper
    {
        void CreateHtml(string url, string content);
        void CreateFile(string url, string rootUrl);
    }
}