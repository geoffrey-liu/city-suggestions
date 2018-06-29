using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LocationSuggestion.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

namespace LocationSuggestion.Controllers
{
    [Produces("application/json")]
    [Route("suggestions")]
    public class SuggestionsController : Controller
    {
        private readonly LocationPersistence locationPersistence = LocationPersistence.Instance;

        // GET: api/suggestions/5
        [Route("")]
        [HttpGet]
        public IEnumerable<Location> Get(string q, float? latitude, float? longitude)
        {
            return locationPersistence.GetLocations(q, latitude, longitude);
        }

        // POST: unused
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: unused
        [HttpPut("id")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: unused
        [HttpDelete("id")]
        public void Delete(int id)
        {
        }
    }
}
