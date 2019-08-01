using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certification70_487_Framework4._6.Unit_1.XML
{
    internal sealed class ExamplePOCOClass
    {
        public ExamplePOCOClass() { }
        public ExamplePOCOClass(string firstName, string middleInitial, string lastName)
        {
            FirstName = firstName;
            MiddleInitial = middleInitial;
            LastName = lastName;
        }

        public string FirstName { get; }
        public string MiddleInitial { get; }
        public string LastName { get; }
    }
}
