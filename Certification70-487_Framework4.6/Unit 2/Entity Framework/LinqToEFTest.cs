using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.EntityClient;
//using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace Certification70_487_Framework4._6.Unit_2.Entity_Framework
{
    /// <summary>
    /// This test is only for Entity Framework 5 based on .Net Framework 4.6
    /// </summary>
    [TestClass]
    public class LinqToEFTest
    {
        private CertificationCourseEntities dbContext;

        [TestInitialize]
        public void SetUp()
        {
            this.dbContext = new CertificationCourseEntities();
        }

        [TestMethod]
        public void InsertTest()
        {
            var employee = new Employee
            {
                FirstName = "Geiser",
                LastName = "Aragon",
                BirtthDate = new DateTime(1992, 07, 11),
                Genre = "M"
            };

            dbContext.Employee.Add(employee);

            var numberOfChanges = dbContext.SaveChanges();

            Assert.IsTrue(numberOfChanges == 1, "Change wasn´t applied");
        }

        [TestMethod]
        public void LinqToEntityTest()
        {
            dbContext.Employee.Add(new Employee
            {
                FirstName = "Geiser",
                LastName = "Aragon",
                BirtthDate = new DateTime(1992, 07, 11),
                Genre = "M"
            });

            dbContext.Employee.Add(new Employee
            {
                FirstName = "Dayana",
                LastName = "Gaellgos",
                BirtthDate = new DateTime(1997, 07, 20),
                Genre = "M"
            });

            IQueryable<string> query = (from e in dbContext.Employee where e.BirtthDate.Year < 1995 select e.FirstName);

            #region ObjectQuery
            // For old versions using queries as SELECT VALUE product FROM AdventureWorksEntities.Products AS product
            // https://docs.microsoft.com/en-us/dotnet/api/system.data.objects.objectquery-1?view=netframework-4.8
            // ObjectQuery<string> a = new ObjectQuery<string>("", (ObjectContext) dbContext);
            #endregion

            Debug.WriteLine(query.ToString());
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TransactionTest(bool requiredRollback)
        {
            try
            {
                // Set an isolatio level 
                TransactionOptions transactionOptions = new TransactionOptions {
                    IsolationLevel =  System.Transactions.IsolationLevel.ReadCommitted,
                    Timeout = TimeSpan.FromSeconds(10)
                };

                using (var scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    var employee = new Employee
                    {
                        FirstName = "Dafne",
                        LastName = "Aragon",
                        BirtthDate = new DateTime(2012, 11, 09),
                        Genre = "F"
                    };

                    dbContext.Employee.Add(employee);
                    var numberOfChanges = dbContext.SaveChanges();

                    if (requiredRollback)
                    {
                        throw new Exception("Test rollback");
                    }

                    var exists = dbContext.Employee.Any(x => x.FirstName == "Dafne");
                    Assert.IsTrue(exists, "Dafne is NOT in the database");

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                var exists = dbContext.Employee.Any(x => x.FirstName == "Dafne");
                Assert.IsFalse(exists, "Dafne is in the database");
            }
        }

        // **** It requires ObjectContextt instead DBContext (basicly it is obsolete). 
        //[TestMethod]
        //public void CopiledQUeryTest()
        //{
        //    var compiledQuery = CompiledQuery.Compile<CertificationCourseEntities, int, Employee>(
        //        (CertificationCourseEntities dbContext, int employyeId) => {
        //            return dbContext.Employee.FirstOrDefault();
        //        });
        //}

        [TestMethod]
        public void SLQueryTest()
        {
            using (var connection = new EntityConnection("name=CertificationCourseEntities"))
            {
                connection.Open();
                EntityCommand command = new EntityCommand("SELECT VALUE p FROM CertificationCourseEntities.Employee AS p", connection);
                using (var reader = command.ExecuteReader(System.Data.CommandBehavior.SequentialAccess | System.Data.CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        string firstname = reader.GetString(1);
                        string lastname = reader.GetString(2);
                        Console.WriteLine("{0} {1}", firstname, lastname);
                    }
                }
            }
        }
    }
}
