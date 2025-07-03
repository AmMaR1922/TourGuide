using ApplicationLayer.Contracts.Services;
using ApplicationLayer.DTOs.Language;
using ApplicationLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TourGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class LanguageController : ControllerBase
    {
        private readonly ILanguageServices languageService;

        public LanguageController(ILanguageServices languageService)
        {
            this.languageService = languageService;
        }

        [HttpGet("GetAllLanguages")]
        public async Task<ActionResult<APIResponse<List<LanguageDTOResponse>>>> GetAllLanguages()
        {
            var response = await languageService.GetAll();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetLanguageById/{id}")]
        public async Task<ActionResult<APIResponse<LanguageDTOResponse>>> GetLanguageById(int id)
        {
            var response = await languageService.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AddLanguage")]
        public async Task<ActionResult<APIResponse<string>>> AddLanguage(LanguageDTORequest languageDTORequest)
        {
            var response = await languageService.Add(languageDTORequest);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("UpdateLanguage/{Id}")]
        public async Task<ActionResult<APIResponse<string>>> UpdateLanguage(int Id, LanguageDTORequest languageDTORequest)
        {
            var response = await languageService.Update(Id, languageDTORequest);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("DeleteLanguage/{Id}")]
        public async Task<ActionResult<APIResponse<string>>> DeleteLanguage(int Id)
        {
            var response = await languageService.Delete(Id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
