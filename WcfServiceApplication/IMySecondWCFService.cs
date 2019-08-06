using System.Runtime.Serialization;
using System.ServiceModel;

namespace WcfServiceApplication
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMySecondWCFService" in both code and config file together.
    [ServiceContract]
    public interface IMySecondWCFService
    {
        [OperationContract]
        [FaultContract(typeof(FaultException))]
        int GetFactorialNumber(int number);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        int ApplyOperation(int number1, int number2, EnumOperator enumOperator);
    }

    [DataContract]
    public enum EnumOperator {
        [EnumMember]
        Plus,
        [EnumMember]
        Minus,
        [EnumMember]
        Times,
        // As this enum element has not EnumMember decorator so it is not exposed in the Web Service.
        DividedBy
    }
}
