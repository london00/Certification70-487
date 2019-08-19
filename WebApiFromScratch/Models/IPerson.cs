using System.Collections.Generic;

namespace WebApiFromScratch.Models
{
    public interface IPerson
    {
        int GetYearOfBirth();
        List<Person> GetExamplePeople();
    }
}