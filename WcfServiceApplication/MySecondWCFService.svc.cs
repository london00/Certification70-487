using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WcfServiceApplication
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MySecondWCFService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MySecondWCFService.svc or MySecondWCFService.svc.cs at the Solution Explorer and start debugging.
    public class MySecondWCFService : IMySecondWCFService
    {
        public int ApplyOperation(int number1, int number2, EnumOperator enumOperator)
        {
            switch (enumOperator)
            {
                case EnumOperator.Plus:
                    return number1 + number2;
                case EnumOperator.Minus:
                    return number1 - number2;
                case EnumOperator.Times:
                    return number1 * number2;
                default:
                    throw new FaultException("Unknown operator");
            }
        }

        public int GetFactorialNumber(int number)
        {
            if (number <= 0)
            {
                throw new FaultException(
                    MessageFault.CreateFault(
                        FaultCode.CreateSenderFaultCode(
                            new FaultCode(
                                "E01", 
                                "www.google.com"
                                )
                            ), 
                        new FaultReason(
                            new FaultReasonText(
                                "Number should be greater than zero", 
                                CultureInfo.CurrentUICulture
                                )
                            )
                        )
                    , "Check the fault reason");
            }

            if (number == 1) return 1;
            return number * GetFactorialNumber(--number);
        }
    }
}
