using ApplicationLayer.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TourGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    
    public class ValuesController(IMailingService mailing) : ControllerBase
    {
        private readonly IMailingService mailingService = mailing;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await mailingService.SendEmailAsync("victornisem01@gmail.com","Test Mail","Vico Gad3");
            return Ok("Dummy Data");
        }
    }
}
