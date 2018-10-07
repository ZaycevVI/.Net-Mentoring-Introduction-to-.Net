namespace Library.EventArgs
{
    public class FinishArg : FileSystemVisitorBaseArg
    {
        public FinishArg(string message)
        {
            Message = message;
        }

        public override string Message { get; }
    }
}
