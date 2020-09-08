using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MVCAngular1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
         
        [HttpPost("testdatetime")]
        public IActionResult Testdatetime([FromBody] Testdatetime value)
        {
            return Ok(value);
        }
 
    }

    public class Testdatetime
    {
        public DateTime Datetimeformat { get; set; }
        public string Stringformat { get; set; }
    }
}
