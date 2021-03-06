﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiConsulTest2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values  
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var port = Request.Host.Port;

            return new string[] { "value1", "value2", port.Value.ToString() };
        }
    }

}
