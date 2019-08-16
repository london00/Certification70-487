using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiFromScratch.Models;

namespace WebApiFromScratch.Controllers
{
    public class PeopleController : ApiController
    {
        private List<Person> GetExamplePeople() {
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

        // Custom Verb
        [AcceptVerbs("HOLA")]
        public List<Person> GetAllPeople()
        {
            return GetExamplePeople();
        }

        [HttpGet]
        [ActionName("GetPersonByName")]
        public Person Person([FromUri] string name)
        {
            var person = GetExamplePeople().FirstOrDefault(x => x.Name.ToLower() == name.ToLower());


            if (person == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }


            return person;
        }
    }
}
