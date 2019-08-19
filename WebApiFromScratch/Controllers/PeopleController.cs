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
        private readonly IPerson personService;

        public PeopleController(IPerson person)
        {
            this.personService = person;
        }

        // Custom Verb
        [AcceptVerbs("HOLA")]
        public List<Person> GetAllPeople()
        {
            return personService.GetExamplePeople();
        }


        [HttpGet]
        [ActionName("GetPersonByName")]
        public Person Person([FromUri] string name)
        {
            var person = personService.GetExamplePeople().FirstOrDefault(x => x.Name.ToLower() == name.ToLower());


            if (person == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }


            return person;
        }

        [HttpGet]
        public Person PersonByNameWithoutCustomActionName([FromUri] string name)
        {
            var person = personService.GetExamplePeople().FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (person == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }


            return person;
        }
    }
}
