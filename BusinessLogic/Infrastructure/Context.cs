namespace BusinessLogic.Infrastructure
{
    using System;
    using System.Threading.Tasks;


    internal interface IContext<T>
    {
        string User { get; }
        IContextLogger Log { get; }
        IContext<T2> OnSuccess<T2>(Func<ISuccessContext<T>, T2> selector);
        Task<IContext<T2>> OnSuccess<T2>(Func<ISuccessContext<T>, Task<T2>> selector);
        IContext<T> OnError(Func<IErrorContext<T>, T> selector);
        Task<IContext<T>> OnError(Func<IErrorContext<T>, Task<T>> selector);
    }
    internal interface ISuccessContext<T> : IContext<T>
    {
        T Data { get; }

    }
    internal interface IErrorContext<T> : IContext<T>
    {
        Error Error { get; }
    }

    internal interface IContextLogger
    {
        void Trace(string msg);
        void Info(string msg);
        void Warn(string msg);
        void Error(string msg);
    }

    internal class Context<T> : IContext<T>, ISuccessContext<T>, IErrorContext<T>
    {
        public T Data { get; }
        public Error Error { get; }

        public Context(T data)
        {
            Data = data;
        }
        public Context(Error error)
        {
            Error = error;
        }

        public string User { get; }

        public IContextLogger Log { get; }
        private Error ExceptionToError(Exception e) => new Error(default, default, default, default);
        private IContext<T2> New<T2>(T2 data) => new Context<T2>(data);
        private IContext<T2> New<T2>(Error error) => new Context<T2>(error);
        private IContext<T2> New<T2>(Exception exception) => new Context<T2>(ExceptionToError(exception));
        public IContext<T> OnError(Func<IErrorContext<T>, T> selector)
        {
            if (Error is not null)
            {
                try
                {
                    return New(selector(this));
                }
                catch (Exception e)
                {
                    return New<T>(e);
                }
            }
            return New<T>(Data);
        }

        public async Task<IContext<T>> OnError(Func<IErrorContext<T>, Task<T>> selector)
        {
            if (Error is not null)
            {
                try
                {
                    return New(await selector(this));
                }
                catch (Exception e)
                {
                    return New<T>(e);
                }
            }
            return New<T>(Data);
        }

        public IContext<T2> OnSuccess<T2>(Func<ISuccessContext<T>, T2> selector)
        {
            if (Error is null)
            {
                try
                {
                    return New(selector(this));
                }
                catch (Exception e)
                {
                    return New<T2>(e);
                }
            }
            return New<T2>(Error);
        }

        public async Task<IContext<T2>> OnSuccess<T2>(Func<ISuccessContext<T>, Task<T2>> selector)
        {
            if (Error is null)
            {
                try
                {
                    return New(await selector(this));
                }
                catch (Exception e)
                {
                    return New<T2>(e);
                }
            }
            return New<T2>(Error);
        }
    }

}
// internal record ContextStackFrame(string Name, ContextStackFrame Parent) { }
// internal record Context(string Id, ContextStackFrame ContextStackTrace)
//     {
//         public async Task<Response<TResponseData>> New<TRequest, TResponseData>(string name, TRequest request, Func<Context, TRequest, Task<Response<TResponseData>>> func)
//         {
//             var ctx = new Context(this, NewId(), CorrelationId, name, User, Log);
//             try
//             {
//                 ctx.Log.Trace("Entering context", request);
//                 var response = await func(ctx, request);
//                 ctx.Log.Trace("Exiting context", response);
//                 return response;
//             }
//             catch (Exception exception)
//             {
//                 var error = Error(ErrorCode.Exception);
//                 ctx.Log.Error($"Exception in context [ErrorId={error.Id}]", exception);
//                 return error;
//             }
//         }

//         public Response<TResponseData> New<TRequest, TResponseData>(string name, TRequest request, Func<Context, TRequest, Response<TResponseData>> func)
//         {
//             var ctx = new Context(this, NewId(), CorrelationId, name, User, Log);
//             try
//             {
//                 ctx.Log.Trace("Entering context", request);
//                 var response = func(ctx, request);
//                 ctx.Log.Trace("Exiting context", response);
//                 return response;
//             }
//             catch (Exception exception)
//             {
//                 var error = Error(ErrorCode.Exception);
//                 ctx.Log.Error($"Exception in context [ErrorId={error.Id}]", exception);
//                 return error;
//             }
//         }

//         public Error Error(ErrorCode errorCode, object data = null) => new Error(NewId(), errorCode, CorrelationId, data);
//     }
// }

// internal record RequestContext<TRequest>(TRequest Request) : Context
// {

// }