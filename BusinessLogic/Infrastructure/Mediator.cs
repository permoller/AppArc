namespace BusinessLogic.Infrastructure
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    internal class Mediator
    {
        private static ConcurrentDictionary<Type, Func<object>> RequestHandlerFactories = new ConcurrentDictionary<Type, Func<object>>();

        public static void Register<TRequest, TResponseData>(Func<IRequestHandler<TRequest, TResponseData>> requestHandlerFactory)
            where TRequest : Request<TResponseData>
        {
            RequestHandlerFactories[typeof(TRequest)] = requestHandlerFactory;
        }

        private static Func<object> GetHandlerFactory<TRequest>(ISuccessContext<TRequest> ctx)
        {
            if (!RequestHandlerFactories.TryGetValue(typeof(TRequest), out var factory))
                throw new Exception(""); // ctx.Error(ErrorCode.NoRequestHandlerRegistered, new { RequestType = typeof(TRequest) });
            return factory;
        }

        private static IRequestHandler<TRequest, TResponseData> CreateHandler<TRequest, TResponseData>(ISuccessContext<Func<object>> ctx)
            where TRequest : Request<TResponseData>
        {
            return (IRequestHandler<TRequest, TResponseData>)ctx.Data();
        }

        public async Task<IContext<TResponseData>> Send<TRequest, TResponseData>(ISuccessContext<TRequest> ctx)
            where TRequest : Request<TResponseData>
        {
            return await ctx
                .OnSuccess(GetHandlerFactory)
                .OnSuccess(CreateHandler<TRequest, TResponseData>)
                .OnSuccess(c => c.Data.Handle(ctx));
        }


    }
}