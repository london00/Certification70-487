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
    public class MySecondWCFServiceTests
    {
        [TestMethod()]
        public void GetFactorialNumberTest()
        {
            MySecondWCFService mySecondWCFService = new MySecondWCFService();
            var result = mySecondWCFService.GetFactorialNumber(5);
            Assert.AreEqual(result, 120);
        }
    }
}