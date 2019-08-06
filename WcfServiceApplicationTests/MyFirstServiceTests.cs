using Microsoft.VisualStudio.TestTools.UnitTesting;
using WcfServiceApplicationTests.MyFisrServiceWCFReference;

namespace WcfServiceApplication.Tests
{
    [TestClass]
    public class Service1Tests
    {
        [TestMethod]
        [DataRow(99)]
        public void GetDataTest(int testData)
        {
            IMyFirstService myFirstService = new MyFirstServiceClient();
            var response = myFirstService.GetData(testData);
            
            Assert.AreEqual(string.Format("You entered: {0}", testData), response);
        }
    }
}