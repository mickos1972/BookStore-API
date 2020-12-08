using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore_API.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore_API.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        readonly ILoggerService _logger;

        public HomeController(ILoggerService logger)
        {
            _logger = logger;
        }

        /// <returns></returns>
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogDebug("fuckwit");

            return new string[] { "value1", "value2" };
        }
        /// <summary>
        /// this is a test get 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>loads of interesting things</returns>
        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// this is a post test
        /// </summary>
        /// <param name="value"></param>
        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
