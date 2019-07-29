using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Certification70_487_Framework4._6.Unit_1.AzureStorage
{
    [TestClass]
    public class BlobStoragTest
    {
        private const string CONTAINER_NAME = "fileContainer";
        private CloudBlobContainer containerReference;

        [TestInitialize]
        public void SetUp()
        {
            StorageCredentials storageAccount = new StorageCredentials("geiserstoragesccount", "pmzTGAxGbQ78X5luKQpotG+bzXdnxZAOc8a5uwGQpIfbp0Aavpsi70rPvRXVSswrDmh0co57x7+JwAvIXRPQ5w==");
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageAccount, true);
            CloudBlobClient client = cloudStorageAccount.CreateCloudBlobClient();
            this.containerReference = client.GetContainerReference(CONTAINER_NAME.ToLower());
        }

        [TestMethod]
        [DataRow("test.txt")]
        public void UploadFile_Test(string fileName)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
            containerReference.CreateIfNotExists();
            CloudBlockBlob cloudBlockBlob = containerReference.GetBlockBlobReference(fileName);

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                cloudBlockBlob.UploadFromStream(stream);
                Assert.AreEqual(cloudBlockBlob.Properties.Length, stream.Length);
            }
        }

        [TestMethod]
        [DataRow("test.txt")]
        public void ReadFile_Test(string fileName)
        {
            // Get container
            Assert.IsTrue(containerReference.Exists(), "Container doesn´t exist");
            CloudBlockBlob cloudBlockBlob = containerReference.GetBlockBlobReference(fileName);

            // Read blob
            var content = cloudBlockBlob.DownloadText();
            Debug.WriteLine(content);
        }

        [TestMethod]
        [DataRow("test.txt")]
        public void DeleteFile_Test(string fileName)
        {
            // Get container
            Assert.IsTrue(containerReference.Exists(), "Container doesn´t exist");
            CloudBlockBlob cloudBlockBlob = containerReference.GetBlockBlobReference(fileName);

            // Read blob
            var deleted = cloudBlockBlob.DeleteIfExists();

            Assert.IsTrue(deleted, "File doesn´t exist");
        }
    }
}
