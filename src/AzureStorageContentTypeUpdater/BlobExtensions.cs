using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureStorageContentTypeUpdater
{
    public static class BlobExtensions
    {
        public static CloudBlockBlob AsBlockBlob(this IListBlobItem item)
        {
            try
            {
                return (CloudBlockBlob) item;
            }
            catch
            {
                return null;
            }
        }

        public static CloudBlobDirectory AsBlobDirectory(this IListBlobItem item)
        {
            try
            {
                return (CloudBlobDirectory)item;
            }
            catch
            {
                return null;
            }
        }
    }
}