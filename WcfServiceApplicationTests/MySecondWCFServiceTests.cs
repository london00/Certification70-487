using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using WcfServiceApplicationTests.MySecondWCFServiceReference;

namespace WcfServiceApplication.Tests
{
    [TestClass]
    public class MySecondWCFServiceTests
    {
        [TestMethod]
        public void GetFactorialNumberTest()
        {
            IMySecondWCFService mySecondWCFService = new MySecondWCFServiceClient();
            var result = mySecondWCFService.GetFactorialNumber(5);
            Assert.AreEqual(result, 120);
        }

        [TestMethod]
        public void GetFactorialNumber_IsEqualsOrLessThanZero_ThrowsException()
        {
            IMySecondWCFService mySecondWCFService = new MySecondWCFServiceClient();
            try
            {
                var result = mySecondWCFService.GetFactorialNumber(0);
            }
            catch (FaultException ex)
            {

            }
            catch
            {
                Assert.Fail("Web service didn´t throw ArgumentOutOfRangeException");
            }
        }

        [TestMethod]
        public void ApplyOperationTest()
        {
            IMySecondWCFService mySecondWCFService = new MySecondWCFServiceClient();
            var result = mySecondWCFService.ApplyOperation(3, 2, EnumOperator.Minus);
            Assert.AreEqual(result, 1);
        }
    }
}