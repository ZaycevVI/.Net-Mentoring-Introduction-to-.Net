namespace Library.EventArgs
{
    public class StartArg : FileSystemVisitorBaseArg
    {
        public StartArg(string message)
        {
            Message = message;
        }

        public override string Message { get; }
    }
}
