namespace BusinessLogic.Infrastructure
{
    public enum ErrorCode
    {
        NoError = 0,
        Exception = 1,
        NoRequestHandlerRegistered = 1,
        RequestHandlerFactoryException = 2

    }
}