using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using ImageProcessor.Web.Services;
using ImageProcessor.Web.Caching;
using ImageProcessor.Web.Helpers;

namespace Doras
{
    /// <summary>
    /// An image service for retrieving images from Azure.
    /// </summary>
    public class AzureImageService : IImageService
    {
        /// <summary>
        /// Gets or sets the prefix for the given implementation.
        /// <remarks>
        /// This value is used as a prefix for any image requests that should use this service.
        /// </remarks>
        /// </summary>
        public string Prefix { get; set; } = string.Empty;

        /// <summary>
        /// Gets a value indicating whether the image service requests files from
        /// the locally based file system.
        /// </summary>
        public bool IsFileLocalService => false;

        /// <summary>
        /// Gets or sets any additional settings required by the service.
        /// </summary>
        public Dictionary<string, string> Settings { get; set; }

        /// <summary>
        /// Gets or sets the white list of <see cref="Uri" />. 
        /// </summary>
        public Uri[] WhiteList { get; set; }

        /// <summary>
        /// Gets a value indicating whether the current request passes sanitizing rules.
        /// </summary>
        /// <param name="path">The image path.</param>
        /// <returns>
        /// <c>True</c> if the request is valid; otherwise, <c>False</c>.
        /// </returns>
        public bool IsValidRequest(string path) => ImageHelpers.IsValidImageExtension(path);

        /// <summary>
        /// Gets the image using the given identifier.
        /// </summary>
        /// <param name="id">The value identifying the image to fetch.</param>
        /// <returns>
        /// The <see cref="byte" /> array containing the image data.
        /// </returns>
        public virtual async Task<byte[]> GetImage(object id)
        {
            string fileName = id.ToString();
            var blobContainer = await GetContainerAsync(Settings["Container"]);
            var blockBlob = blobContainer.GetBlockBlobReference(fileName);

            if (!await blockBlob.ExistsAsync())
            {
                return null;
            }

            using (MemoryStream memoryStream = MemoryStreamPool.Shared.GetStream())
            {
                await blockBlob.DownloadToStreamAsync(memoryStream).ConfigureAwait(false);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Initialise the service.
        /// </summary>
        protected async Task<CloudBlobContainer> GetContainerAsync(string containerName)
        {
            if (!CloudStorageAccount.TryParse(Settings["StorageAccount"], out CloudStorageAccount storageAccount))
            {
                return null;
            }

            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(containerName);

            if (!await blobContainer.ExistsAsync())
            {
                throw new StorageException();
            }

            return blobContainer;
        }
    }
}