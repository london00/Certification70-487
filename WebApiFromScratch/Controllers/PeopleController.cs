﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiFromScratch.Filters;
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
        [CustomFilter]
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

        [HttpGet]
        public async Task<Person> PersonByNameWithoutCustomActionNameAsync([FromUri] string name)
        {
            var person = personService.GetExamplePeople().FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (person == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            // Use the thread during 10 seconds. As the actions is async it is possible to make parallel requests.
            await Task.Factory.StartNew(() => {
                Thread.Sleep(10000);
            });

            return person;
        }
    }
}