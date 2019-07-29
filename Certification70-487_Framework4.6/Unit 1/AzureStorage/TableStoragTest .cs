using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using System.Diagnostics;

namespace Certification70_487_Framework4._6.Unit_1.AzureStorage
{
    [TestClass]
    public class TableStorageTest
    {
        private const string TABLE_NAME = "MyOwnTable";
        private CloudTable tableReference;

        [TestInitialize]
        public void SetUp()
        {
            StorageCredentials storageAccount = new StorageCredentials("geiserstoragesccount", "pmzTGAxGbQ78X5luKQpotG+bzXdnxZAOc8a5uwGQpIfbp0Aavpsi70rPvRXVSswrDmh0co57x7+JwAvIXRPQ5w==");
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageAccount, true);
            var client = cloudStorageAccount.CreateCloudTableClient();
            this.tableReference = client.GetTableReference(TABLE_NAME.ToLower());
            tableReference.CreateIfNotExists();
        }

        [TestMethod]
        [DataRow("Geiser", "Aragon", "Progreso", 27)]
        [DataRow("Dafne", "Aragon", "Merida", 7)]
        [DataRow("Dayana", "Gallegos", "Merida", 22)]
        public void InsertRecord_Test(string Name, string LastName, string City, int Age)
        {
            var myRecord = new MyOwnTable(Name, City)
            {
                LastName = LastName,
                Age = Age
            };

            var myInsertOperation = TableOperation.InsertOrReplace(myRecord);
            tableReference.Execute(myInsertOperation);
        }

        [TestMethod]
        [DataRow("Geiser", "Progreso")]
        public void QueryRecordByKeys_Test(string Name, string City)
        {
            var querytOperation = TableOperation.Retrieve<MyOwnTable>(Name, City);
            var result = tableReference.Execute(querytOperation);
            var myOwnEntity = result.Result as MyOwnTable;

            Assert.IsNotNull(myOwnEntity, "Record not found");
        }

        [TestMethod]
        [DataRow(20)]
        public void QueryRecordsByProperties_Test(int Age)
        {
            var result = tableReference.CreateQuery<MyOwnTable>().Where(x=>x.Age >= Age);
            Assert.IsTrue(result.Any(), "Record not found");
        }


        [TestMethod]
        [DataRow(5)]
        public void DeleteRecords_Test(int Age)
        {
            var queryResult = tableReference.CreateQuery<MyOwnTable>().Where(x => x.Age >= Age).ToList();
            Assert.IsTrue(queryResult.Any(), "Record not found");

            foreach (var result in queryResult)
            {
                var deleteOperation = TableOperation.Delete(result);
                var deleteResult = tableReference.Execute(deleteOperation);
            }
        }

        public class MyOwnTable : TableEntity
        {
            public string FirstName => this.PartitionKey;
            public string LastName { get; set; }
            public string City => this.RowKey;
            public int Age { get; set; }

            public MyOwnTable(string FirstName, string city)
            {
                this.PartitionKey = FirstName;
                this.RowKey = city;
            }

            public MyOwnTable()
            {

            }
        }
    }
}
