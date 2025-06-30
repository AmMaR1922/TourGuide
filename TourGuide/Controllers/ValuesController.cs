﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TourGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
    public class ValuesController : ControllerBase
    {
        [HttpGet]
       
        public IActionResult Get()
        {
            return Ok("Dummy Data");
        }
    }
}
