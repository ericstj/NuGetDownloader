using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace nugetDownloader
{
    class PackageInstaller
    {

        public static async Task InstallFromUrl(string baseUrl, PackageIdentity package, string packagesFolder, CancellationToken token)
        {

            var url = $"{baseUrl}/{package.Id}/{package.Version}/{package.Id}.{package.Version}.nupkg";

            // hack
            var client = new HttpClient();
            var stream = client.GetStreamAsync(url);

            await InstallFromStream(stream, package, packagesFolder, token);

        }

        public static async Task InstallFromStream(Task<Stream> getStream, PackageIdentity package, string packagesFolder, CancellationToken token)
        {
            bool isValid = true;
            if (OfflineFeedUtility.PackageExists(package, packagesFolder, out isValid))
            {
                return;
            }

            
            var logger = NuGetConsoleLogger.Instance;

            var versionFolderPathContext = new VersionFolderPathContext(
                package,
                packagesFolder,
                isLowercasePackagesDirectory: false,
                logger: logger,
                packageSaveMode: PackageSaveMode.Defaultv3,
                xmlDocFileSaveMode: XmlDocFileSaveMode.None);

            await PackageExtractor.InstallFromSourceAsync(
                async dest =>
                {
                    var source = await getStream;
                    await source.CopyToAsync(dest);
                },
                versionFolderPathContext,
                token);
        }


        public static async Task InstallFromFile(string file, string packagesFolder, CancellationToken token)
        {
            using (Stream stream = File.OpenRead(file))
            {
                var reader = new PackageArchiveReader(stream, leaveStreamOpen: true);
                var packageIdentity = reader.GetIdentity();
                reader.Dispose();

                stream.Seek(0, SeekOrigin.Begin);

                await InstallFromStream(Task.FromResult(stream), packageIdentity, packagesFolder, token);
            }
        }


    }
}
