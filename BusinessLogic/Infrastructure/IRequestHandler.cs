namespace BusinessLogic.Infrastructure
{
    using System.Threading.Tasks;

    internal interface IRequestHandler<TRequest, TResponseData>
        where TRequest : Request<TResponseData>
    {
        Task<TResponseData> Handle(ISuccessContext<TRequest> ctx);
    }
}