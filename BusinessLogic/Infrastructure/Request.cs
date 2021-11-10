namespace BusinessLogic.Infrastructure
{
    public abstract record Request<TResponseData>(string Id) { }
}