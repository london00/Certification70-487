using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certification70_487_Framework4._6.Unit_2.Entity_Framework
{
    [TestClass]
    public class LinqToEFTest
    {
        private CertificationCourseEntities dbContext;

        [TestInitialize]
        public void SetUp() {
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
    }
}
