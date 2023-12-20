// ReferenceLink:
//  https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger?view=dotnet-plat-ext-7.0
//  https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory?view=dotnet-plat-ext-7.0

using Xunit.Abstractions;
using Microsoft.Extensions.Logging;

namespace Chirp.SharedUsings
{
    public class OutputLoggerFactory : ILoggerFactory
    {
        private readonly ITestOutputHelper output;

        public OutputLoggerFactory(ITestOutputHelper output)
        {
            this.output = output;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new OutputLogger(output, categoryName);
        }

        public void AddProvider(ILoggerProvider provider)
        {
        }

        public void Dispose()
        {
        }
    }
}

