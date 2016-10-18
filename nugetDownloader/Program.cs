using NuGet.Commands;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace nugetDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            // scraped from https://dotnet.myget.org/F/dotnet-core/api/v3/index.json
            string baseUrl = "https://dotnetmyget.blob.core.windows.net/artifacts/dotnet-core/nuget/v3/flatcontainer/";

            // copied from https://raw.githubusercontent.com/dotnet/versions/master/build-info/dotnet/corefx/master/Latest_Packages.txt
            var packages = File.ReadAllLines("Latest_Packages.txt").Select(pl => pl.Split(' ')).Select(p => new PackageIdentity(p[0], NuGetVersion.Parse(p[1])));

            var options = new ParallelOptions() { MaxDegreeOfParallelism = 32 };

            var packagesFolder = Path.GetFullPath(".\\packages");

            if (Directory.Exists(packagesFolder))
            {
                Directory.Delete(packagesFolder, true);
            }
            Directory.CreateDirectory(packagesFolder);

            var sw = Stopwatch.StartNew();
            Parallel.ForEach(packages, options, p => PackageInstaller.InstallFromUrl(baseUrl, p, packagesFolder, CancellationToken.None));
            Console.WriteLine($"Total: {sw.Elapsed}");

        }
    }
}
