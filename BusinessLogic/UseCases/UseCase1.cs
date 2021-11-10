using System;
using System.Threading.Tasks;
using BusinessLogic.Infrastructure;

namespace BusinessLogic.UseCases
{

    public class UseCase1
    {
        public class Response : IResponse { }
        public class Request : IRequestData<Response> { }
        public static Receiver<Request, Response> Receiver = request =>
        {
            return Task.FromResult(new Response());
        };
        static UseCase1() { Mediator.Register(Receiver); }

        public static void Test(object context)
        {
            context
            .Select(c => new { z = c.Data.x })
            .Select(c => new { e = c.Data.z })
            .Select(c =>
            {
                c.Select(c2 => new { });

            });
        }
    }


}