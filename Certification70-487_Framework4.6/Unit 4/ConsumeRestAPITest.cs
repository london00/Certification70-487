using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace Certification70_487_Framework4._6.Unit_4
{
    /// <summary>
    /// Summary description for ConsumeRestAPITest
    /// </summary>
    [TestClass]
    public class ConsumeRestAPITest
    {
        private HttpClient httpClient;

        #region Additional test attributes
        [TestInitialize]
        public void MyTestInitialize()
        {
            this.httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost/WebApiFromScratch/");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion

        [TestMethod]
        public void HttpGet_Test()
        {
            var response = this.httpClient.GetAsync("api/People/GetPersonByName" + "?name=geiser").Result;
            Assert.IsTrue(response.IsSuccessStatusCode, "Error code");
            var person = response.Content.ReadAsAsync<Person>().Result;
        }

        [TestMethod]
        public void HttpPost_Test()
        {
            var response = httpClient.PostAsJsonAsync("api/People", new Person { Name = "Geiser", Age = 27 } ).Result;
            Assert.IsTrue(response.IsSuccessStatusCode, "Error code");
        }

        [TestMethod]
        public void HttpPut_Test()
        {
            var response = httpClient.PutAsJsonAsync("api/People", new Person { Name = "Geiser", Age = 27 }).Result;
            Assert.IsTrue(response.IsSuccessStatusCode, "Error code");
        }

        [TestMethod]
        public void HttpDelete_Test()
        {
            var response = httpClient.DeleteAsync("api/People" + "?name=geiser").Result;
            Assert.IsTrue(response.IsSuccessStatusCode, "Error code");
        }

        [TestMethod]
        public void HttpExceptionHandler_Test()
        {
            try
            {
                var response = httpClient.GetAsync("api/BadUri").Result;
                response.EnsureSuccessStatusCode();

                Assert.Fail("Never should arrive here");
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }
    }

    internal sealed class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
