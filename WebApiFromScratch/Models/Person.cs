using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiFromScratch.Models
{
    public class Person : IPerson
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public int GetYearOfBirth()
        {
            return DateTime.Now.Year - Age;
        }

        public List<Person> GetExamplePeople()
        {
            var people = new List<Person>();
            people.Add(new Person
            {
                Name = "Geiser",
                Age = 27
            });

            people.Add(new Person
            {
                Name = "John",
                Age = 40
            });

            people.Add(new Person
            {
                Name = "Juan",
                Age = 20
            });

            return people;
        }
    }
}