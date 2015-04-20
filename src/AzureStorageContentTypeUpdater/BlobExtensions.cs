using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureStorageContentTypeUpdater
{
    public static class BlobExtensions
    {
        public static string GetLowercasePath(this CloudBlockBlob blob)
        {
            var fileName = Path.GetFileName(blob.Name);

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return blob.Name;
            }

            var dir = blob.Name.Replace(fileName, "");
            return dir.ToLowerInvariant() + fileName;
        }
    }
}