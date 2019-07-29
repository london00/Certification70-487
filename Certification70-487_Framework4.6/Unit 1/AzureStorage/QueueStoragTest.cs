using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Diagnostics;

namespace Certification70_487_Framework4._6.Unit_1.AzureStorage
{
    [TestClass]
    public class QueueStorageTest
    {
        private const string QUEUE_NAME = "MyOwnQueue";
        private CloudQueue queueReference;

        [TestInitialize]
        public void SetUp()
        {
            StorageCredentials storageAccount = new StorageCredentials("geiserstoragesccount", "pmzTGAxGbQ78X5luKQpotG+bzXdnxZAOc8a5uwGQpIfbp0Aavpsi70rPvRXVSswrDmh0co57x7+JwAvIXRPQ5w==");
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageAccount, true);
            var client = cloudStorageAccount.CreateCloudQueueClient();
            this.queueReference = client.GetQueueReference(QUEUE_NAME.ToLower());
            queueReference.CreateIfNotExists();
        }

        [TestMethod]
        [DataRow("It is my message: {0}")]
        public void QueueMessage_Test(string message)
        {
            queueReference.AddMessage(new CloudQueueMessage(string.Format(message, DateTime.Now.ToString("g"))), TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(30));
        }

        [TestMethod]
        [DataRow("test.txt")]
        public void UnqueueMesagge_Test(string fileName)
        {
            var message = queueReference.GetMessage();

            Debug.WriteLine("Content: " + message.AsString);

            queueReference.DeleteMessage(message);

            Debug.WriteLine("Message removed" + message.Id);
        }
    }
}
