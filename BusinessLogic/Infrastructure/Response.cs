namespace BusinessLogic.Infrastructure
{
    using System;
    using System.Threading.Tasks;

    public record Response<T>(bool Success, Error Error, T Data)
    {
        public static implicit operator Response<T>(T data) => new Response<T>(true, default, data);
        public static implicit operator Response<T>(Error error) => new Response<T>(false, error, default);
    }

    internal static class ResponseExtensions
    {
        public static (Context, Response<T2>) Select<T, T2>(this (Context, Response<T>) tuple, Func<Context, T, T2> func)
        {
            if (tuple.Item2.Success)
                return (tuple.Item1, tuple.Item1.New<T, T2>("", tuple.Item2.Data, (c, d) => func(c, d)));
            return (tuple.Item1, tuple.Item2.Error);
        }
        public static (Context, Response<T2>) Select<T, T2>(this (Context, Response<T>) tuple, Func<Context, T, Response<T2>> func)
        {
            if (tuple.Item2.Success)
                return (tuple.Item1, tuple.Item1.New<T, T2>("", tuple.Item2.Data, (c, d) => func(c, d)));
            return (tuple.Item1, tuple.Item2.Error);
        }
        public static async Task<(Context, Response<T2>)> Select<T, T2>(this (Context, Response<T>) tuple, Func<Context, T, Task<T2>> func)
        {
            if (tuple.Item2.Success)
                return (tuple.Item1, await tuple.Item1.New<T, T2>("", tuple.Item2.Data, async (c, d) => await func(c, d)));
            return (tuple.Item1, tuple.Item2.Error);
        }
        public static async Task<(Context, Response<T2>)> Select<T, T2>(this (Context, Response<T>) tuple, Func<Context, T, Task<Response<T2>>> func)
        {
            if (tuple.Item2.Success)
                return (tuple.Item1, await tuple.Item1.New<T, T2>("", tuple.Item2.Data, async (c, d) => await func(c, d)));
            return (tuple.Item1, tuple.Item2.Error);
        }
    }
}