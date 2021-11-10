namespace BusinessLogic.Infrastructure
{
    using System;

    internal static class IdGenerator
    {
        public static string NewId() => Guid.NewGuid().ToString();
    }
}