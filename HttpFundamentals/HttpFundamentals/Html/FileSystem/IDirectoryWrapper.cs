namespace HttpFundamentals.Html.FileSystem
{
    public interface IDirectoryWrapper
    {
        /// <summary>
        /// Create root directory for site. 
        /// Naming depends on domain (ex.: www.root.ru/watch --> www.root.ru\watch)
        /// </summary>
        void Create(string urlPath);
    }
}