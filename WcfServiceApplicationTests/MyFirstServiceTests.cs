using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfServiceApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceApplication.Tests
{
    [TestClass()]
    public class Service1Tests
    {
        [TestMethod]
        [DataRow(99)]
        public void GetDataTest(int testData)
        {
            MyFirstService myFirstService = new MyFirstService();
            var response = myFirstService.GetData(testData);
            
            Assert.AreEqual(string.Format("You entered: {0}", testData), response);
        }
    }
}