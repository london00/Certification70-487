using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Azure.Cosmos;
using System.Diagnostics;
using UnitTestProjectCore.Model.CosmosGettingStartedTutorial;
using System.Net;
using System;
using System.Linq;
using System.Collections.Generic;

namespace UnitTestProjectCore
{
    [TestClass]
    public class CosmosDBTest
    {
        // The Cosmos client instance
        private CosmosClient cosmosClient;
        // The database we will create
        private Database database;
        // The container we will create.
        private Container container;

        [TestInitialize]
        public void SetUp()
        {
            // The Azure Cosmos DB endpoint for running this sample.
            string EndpointUri = "https://cosmosdb-example.documents.azure.com:443/";
            // The primary key for the Azure Cosmos account.
            string PrimaryKey = "qjaSIQrvPM45pC3uCkrxNDURY1diLzZLID93nJdPkAHuZqHSEd4BGEAnEfZZQIfJqn6W7eADnIfttS3aWHAz5w==";

            cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);

            string databaseId = "FamilyDatabase";
            this.database = this.cosmosClient.CreateDatabaseIfNotExistsAsync(id: databaseId).Result.Database;

            Debug.WriteLine($"Database Id: {database.Id}");

            string containerId = "FamilyContainer";
            this.container = this.database.CreateContainerIfNotExistsAsync(id: containerId, partitionKeyPath: "/LastName").Result.Container;
            Debug.WriteLine($"Container Id: {container.Id}");
        }

        [TestMethod]
        public void InsertTest()
        {
            #region Create a family object for the Andersen family
            Family andersenFamily = new Family
            {
                Id = "Andersen.1",
                LastName = "Andersen",
                Parents = new Parent[]
                {
                    new Parent { FirstName = "Thomas" },
                    new Parent { FirstName = "Mary Kay" }
                },
                Children = new Child[]
                {
                    new Child
                    {
                        FirstName = "Henriette Thaulow",
                        Gender = "female",
                        Grade = 5,
                        Pets = new Pet[]
                        {
                            new Pet { GivenName = "Fluffy" }
                        }
                    }
                },
                Address = new Address { State = "WA", County = "King", City = "Seattle" },
                IsRegistered = false
            };

            try
            {
                // Read the item to see if it exists. Note ReadItemAsync will not throw an exception if an item does not exist. Instead, we check the StatusCode property off the response object. 
                ItemResponse<Family> andersenFamilyResponse = this.container.ReadItemAsync<Family>(andersenFamily.Id, new PartitionKey(andersenFamily.LastName)).Result;
                Debug.WriteLine("Item in database with id: {0} already exists\n", andersenFamilyResponse.Resource.Id);
            }
            catch (Exception ex) when (((CosmosException)ex.InnerException).StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
                ItemResponse<Family> andersenFamilyResponse = this.container.CreateItemAsync<Family>(andersenFamily, new PartitionKey(andersenFamily.LastName)).Result;

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Debug.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", andersenFamilyResponse.Resource.Id, andersenFamilyResponse.RequestCharge);
            }

            #endregion

            #region Create a family object for the Wakefield family
            Family wakefieldFamily = new Family
            {
                Id = "Wakefield.7",
                LastName = "Wakefield",
                Parents = new Parent[]
                {
                    new Parent { FamilyName = "Wakefield", FirstName = "Robin" },
                    new Parent { FamilyName = "Miller", FirstName = "Ben" }
                },
                Children = new Child[]
                {
                    new Child
                    {
                        FamilyName = "Merriam",
                        FirstName = "Jesse",
                        Gender = "female",
                        Grade = 8,
                        Pets = new Pet[]
                        {
                            new Pet { GivenName = "Goofy" },
                            new Pet { GivenName = "Shadow" }
                        }
                    },
                    new Child
                    {
                        FamilyName = "Miller",
                        FirstName = "Lisa",
                        Gender = "female",
                        Grade = 1
                    }
                },
                Address = new Address { State = "NY", County = "Manhattan", City = "NY" },
                IsRegistered = true
            };

            try
            {
                // Read the item to see if it exists
                ItemResponse<Family> wakefieldFamilyResponse = this.container.ReadItemAsync<Family>(wakefieldFamily.Id, new PartitionKey(wakefieldFamily.LastName)).Result;
                Debug.WriteLine("Item in database with id: {0} already exists\n", wakefieldFamilyResponse.Resource.Id);
            }
            catch (Exception ex) when (((CosmosException)ex.InnerException).StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container representing the Wakefield family. Note we provide the value of the partition key for this item, which is "Wakefield"
                ItemResponse<Family> wakefieldFamilyResponse = this.container.CreateItemAsync<Family>(wakefieldFamily, new PartitionKey(wakefieldFamily.LastName)).Result;

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Debug.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", wakefieldFamilyResponse.Resource.Id, wakefieldFamilyResponse.RequestCharge);
            }

            #endregion
        }

        [TestMethod]
        public void QueryTest()
        {
            string sqlQueryText = "SELECT * FROM c WHERE c.LastName = 'Andersen'";
            Debug.WriteLine($"Running query: {sqlQueryText}");

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Family> queryResultSetIterator = this.container.GetItemQueryIterator<Family>(queryDefinition);

            List<Family> families = new List<Family>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Family> currentResultSet = queryResultSetIterator.ReadNextAsync().Result;
                foreach (Family family in currentResultSet)
                {
                    families.Add(family);
                    Debug.WriteLine("\tRead {0}\n", family);
                }
            }
        }

        [TestMethod]
        public void QueryLinqTest()
        {
            var querable = this.container.GetItemLinqQueryable<Family>(true);
            var family = querable.Where(x => x.IsRegistered).ToList();
        }

        [TestMethod]
        public void UpdateTest()
        {
            var basicInfo = new
            {
                Id = "Andersen.1",
                LastName = "Andersen"
            };

            // Read the item to see if it exists. Note ReadItemAsync will not throw an exception if an item does not exist. Instead, we check the StatusCode property off the response object. 
            ItemResponse<Family> andersenFamilyResponse = this.container.ReadItemAsync<Family>(basicInfo.Id, new PartitionKey(basicInfo.LastName)).Result;
            Debug.WriteLine("Is registered : ", andersenFamilyResponse.Resource.IsRegistered);

            var family = andersenFamilyResponse.Resource;
            family.IsRegistered = true;

            andersenFamilyResponse = this.container.UpsertItemAsync(family, new PartitionKey(family.LastName)).Result;
            Debug.WriteLine("Is registered : ", andersenFamilyResponse.Resource.IsRegistered);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var basicInfo = new
            {
                Id = "Andersen.1",
                LastName = "Andersen"
            };

            // Read the item to see if it exists. Note ReadItemAsync will not throw an exception if an item does not exist. Instead, we check the StatusCode property off the response object. 
            ItemResponse<Family> response = this.container.DeleteItemAsync<Family>(basicInfo.Id, new PartitionKey(basicInfo.LastName)).Result;
            Debug.WriteLine("Is deleted : ", basicInfo.Id);
        }
    }
}
