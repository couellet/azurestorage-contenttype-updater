using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Fclp;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureStorageContentTypeUpdater
{
    class Program
    {
        public static string AccountName { get; set; }
        public static string Key { get; set; }
        public static string Container { get; set; }
        public static bool UseHttps { get; set; }

        public static IDictionary<string, string> ExtraMimeMaps = new Dictionary<string, string>
        {
            {".woff", "application/font-woff"},
            {".coffee", "application/coffee"},
            {".eot", "application/vnd.ms-fontobject"},
            {".otf", " application/font-sfnt"},
            {".svg", "image/svg+xml"},
            {".ttf", "application/font-sfnt"}
        };

        static void Main(string[] args)
        {
            ParseArguments(args);

            var acccount = GetStorageAccount();
            var client = acccount.CreateCloudBlobClient();

            if (string.IsNullOrWhiteSpace(Container))
            {
                var containers = client.ListContainers();

                foreach (var c in containers)
                {
                    MapContainerFilesContentType(c);
                }
            }
            else
            {
                var container = client.GetContainerReference(Container);

                MapContainerFilesContentType(container);
            }
        }

        private static void MapContainerFilesContentType(CloudBlobContainer container)
        {
            var blobs = container.ListBlobs()
                .Select(x => x.AsBlockBlob())
                .Where(x => x != null);

            foreach (var blob in blobs)
            {
                MapBlobContentType(blob);
            }

            var folders = container.ListBlobs()
                .Select(x => x.AsBlobDirectory())
                .Where(x => x != null);

            ProcessBlobDirectory(folders);
        }

        private static void MapBlobContentType(CloudBlockBlob blob)
        {
            try
            {
                var ext = Path.GetExtension(blob.Name);

                if (ExtraMimeMaps.ContainsKey(ext))
                {
                    blob.Properties.ContentType = ExtraMimeMaps[ext];
                }
                else
                {
                    blob.Properties.ContentType = MimeMapping.GetMimeMapping(blob.Name); ;
                }

                blob.SetProperties();

                Console.WriteLine("{0} -> {1}", blob.Name, blob.Properties.ContentType);
            }
            catch
            {
                Console.WriteLine("Failed to find mime types for file " + blob.Name);
            }
        }

        private static void ProcessBlobDirectory(IEnumerable<CloudBlobDirectory> folders)
        {
            foreach (var folder in folders)
            {
                var blobs = folder.ListBlobs()
                    .Select(x => x.AsBlockBlob())
                    .Where(x => x != null);

                foreach (var b in blobs)
                {
                    MapBlobContentType(b);
                }

                var directories = folder.ListBlobs()
                    .Select(x => x.AsBlobDirectory())
                    .Where(x => x != null);

                ProcessBlobDirectory(directories);
            }
        }

        private static CloudStorageAccount GetStorageAccount()
        {
            return new CloudStorageAccount(
                new StorageCredentials(AccountName, Key),
                UseHttps);
        }

        private static void ParseArguments(string[] args)
        {
            var p = new FluentCommandLineParser();

            p.Setup<string>('a', "account")
                .Callback(x => AccountName = x)
                .Required();

            p.Setup<string>('k', "key")
                .Callback(x => Key = x)
                .Required();

            p.Setup<string>('c', "container")
                .Callback(x => Container = x);

            p.Setup<bool>("https")
                .Callback(x => UseHttps = x);

            p.Parse(args);
        }
    }
}
