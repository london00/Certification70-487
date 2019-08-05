using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceApplication
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MySecondWCFService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MySecondWCFService.svc or MySecondWCFService.svc.cs at the Solution Explorer and start debugging.
    public class MySecondWCFService : IMySecondWCFService
    {
        public int GetFactorialNumber(int number)
        {
            if (number == 1) return 1;
            return number * GetFactorialNumber(--number);
        }
    }
}
