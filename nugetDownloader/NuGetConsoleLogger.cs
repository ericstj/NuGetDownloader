using NuGet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nugetDownloader
{
    internal class NuGetConsoleLogger : ILogger
    {

        public static ILogger Instance = new NuGetConsoleLogger();

        private NuGetConsoleLogger()
        { }

        public void LogDebug(string data)
        {
            Console.WriteLine(data);
        }

        public void LogError(string data)
        {
            Console.WriteLine(data);
        }

        public void LogErrorSummary(string data)
        {
            Console.WriteLine(data);
        }

        public void LogInformation(string data)
        {
            Console.WriteLine(data);
        }

        public void LogInformationSummary(string data)
        {
            Console.WriteLine(data);
        }

        public void LogMinimal(string data)
        {
            Console.WriteLine(data);
        }

        public void LogVerbose(string data)
        {
            Console.WriteLine(data);
        }

        public void LogWarning(string data)
        {
            Console.WriteLine(data);
        }
    }
}
