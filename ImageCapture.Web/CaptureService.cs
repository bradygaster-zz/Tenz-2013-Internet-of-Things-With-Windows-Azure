using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Configuration;
using System.Web;

namespace ImageCapture.Web
{
    public class CaptureService : IHttpHandler
    {
        private CloudStorageAccount _cloudStorageAccount;
        private Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient _blobClient;
        private Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer _blobContainer;

        public CaptureService()
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["STORAGE_ACCOUNT"]
                );

            _blobClient = _cloudStorageAccount.CreateCloudBlobClient();
            _blobContainer = _blobClient.GetContainerReference("imagecapturearchive");
            _blobContainer.CreateIfNotExists();
            _blobContainer.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
        }

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.HttpMethod == "GET")
            {
                context.Response.Write("Post images to this URL for them to be persisted.");
                return;
            }

            if (context.Request.HttpMethod == "POST")
            {
                var files = context.Request.Files;
                var file = files.Count > 0 ? files[0] : null;

                if (file != null)
                {
                    var blob = _blobContainer.GetBlockBlobReference(file.FileName);
                    blob.UploadFromStream(file.InputStream);
                    context.Response.Write(blob.Uri.AbsoluteUri);
                    return;
                }

                if (context.Request.InputStream.Length > 50)
                {
                    if (context.Request.Headers["filename"] != null)
                    {
                        var blob = _blobContainer.GetBlockBlobReference(
                            context.Request.Headers["filename"]
                            );

                        blob.UploadFromStream(context.Request.InputStream);
                        context.Response.Write(blob.Uri.AbsoluteUri);
                        return;
                    }
                }
            }
        }

        #endregion
    }
}
