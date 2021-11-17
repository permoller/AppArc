using System.Collections;
using System.Text;

namespace BusinessLogic.Infrastructure
{
    internal interface ILogger
    {
        void Trace(string msg);
        void Info(string msg);
        void Warn(string msg);
        void Error(string msg);
        void Critical(string msg);
    }

    internal static class LogExtentions
    {
        public static ILogBuilder WithProperty(this ILogger logger, object property, object value)
        {
            return new LogBuilder(logger).WithProperty(property, value);
        }
        public static ILogBuilder OnEntity(this ILogger logger, object entityType, object entityKey)
        {
            return new LogBuilder(logger).WithProperty("EntityType", entityKey).WithProperty("EntityKey", entityKey);
        }
    }
    internal interface ILogBuilder
    {
        ILogBuilder WithProperty(object property, object value);
    }
    internal class LogBuilder : ILogBuilder
    {
        StringBuilder sb = new StringBuilder();
        ILogger logger;

        public LogBuilder(ILogger logger)
        {
            this.logger = logger;
        }

        public void Critical(string msg)
        {
            logger.Critical(sb.Append(msg).ToString());
        }

        public void Error(string msg)
        {
            logger.Error(sb.Append(msg).ToString());
        }

        public void Info(string msg)
        {
            logger.Info(sb.Append(msg).ToString());
        }

        public void Trace(string msg)
        {
            logger.Trace(sb.Append(msg).ToString());
        }

        public void Warn(string msg)
        {
            logger.Warn(sb.Append(msg).ToString());
        }

        public ILogBuilder WithProperty(object property, object value)
        {
            sb.Append(property).Append("=");
            AppendValue(sb, value);
            sb.Append(",");
            return this;
        }

        private void AppendValue<T>(StringBuilder sb, T value)
        {
            if (value is null)
                return;

            if (value is string)
            {
                sb.Append("\"").Append(value.ToString().Replace("\"", "'")).Append("\"");
            }
            else if (value.GetType().IsValueType)
            {
                sb.Append(value);
            }
            else if (value is IEnumerable enumerable)
            {
                sb.Append("[");
                var first = true;
                foreach (var element in enumerable)
                {
                    if (!first)
                        sb.Append(",");
                    first = false;
                    AppendValue(sb, element);
                }
                sb.Append("]");
            }
            AppendValue(sb, value.ToString());
        }
    }
}