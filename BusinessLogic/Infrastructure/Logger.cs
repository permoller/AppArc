namespace BusinessLogic.Infrastructure
{
    internal interface ILogger
    {
        void Trace(string msg, object obj = null);
        void Info(string msg, object obj = null);
        void Warn(string msg, object obj = null);
        void Error(string msg, object obj = null);
    }
}