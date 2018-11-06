namespace FileLibrary.Logger
{
    public delegate void SendMessageHandler(string msg);
    public interface ILogNotifier
    {
        event SendMessageHandler SendMessage;
    }
}