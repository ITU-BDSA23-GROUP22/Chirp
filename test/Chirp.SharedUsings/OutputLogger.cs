// ReferenceLink:
//  https://xunit.net/docs/capturing-output
//  https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger?view=dotnet-plat-ext-7.0

using Xunit.Abstractions;
using Microsoft.Extensions.Logging;

namespace Chirp.SharedUsings
{
    public class OutputLogger : ILogger, IDisposable
    {
        private readonly ITestOutputHelper output;
        private readonly string categoryName;

        public OutputLogger(ITestOutputHelper output, string categoryName)
        {
            this.output = output;
            this.categoryName = categoryName;
        }

        public void Dispose()
        {
        }

        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            return this;
        }

        bool ILogger.IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            this.output.WriteLine(formatter == null ? "" + state : formatter(state, exception));
        }
    }
}