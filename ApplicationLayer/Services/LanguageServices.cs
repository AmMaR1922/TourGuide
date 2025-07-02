using ApplicationLayer.Contracts.Services;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.DTOs.Language;
using ApplicationLayer.Models;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class LanguageServices : ILanguageServices
    {
        private readonly IUnitOfWork unitOfWork;

        public LanguageServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<APIResponse<List<LanguageDTOResponse>>> GetAll()
        {
            var languages = await unitOfWork.Repository<Language>().GetAll()
                .Select(l => new LanguageDTOResponse
                {
                    Id = l.Id,
                    Code = l.Code,
                    Name = l.Name
                })
                .ToListAsync();

            return APIResponse<List<LanguageDTOResponse>>.SuccessResponse(200, languages, "Languages retrieved successfully.");
        }

        public async Task<APIResponse<LanguageDTOResponse>> GetById(int Id)
        {
            var language = await unitOfWork.Repository<Language>().GetByIdAsync(Id);
            
            if (language == null)
                return APIResponse<LanguageDTOResponse>.FailureResponse(404, null, "Language not found.");

            var languageResponse = new LanguageDTOResponse
            {
                Id = language.Id,
                Code = language.Code,
                Name = language.Name
            };

            return APIResponse<LanguageDTOResponse>.SuccessResponse(200, languageResponse, "Languages retrieved successfully.");
        }

        public async Task<APIResponse<string>> Add(LanguageDTORequest languageDto)
        {
            if (string.IsNullOrWhiteSpace(languageDto.Name) || string.IsNullOrWhiteSpace(languageDto.Code))
                return APIResponse<string>.FailureResponse(400, null, "Language name and code cannot be empty.");

            var existingLanguage = await unitOfWork.Repository<Language>().GetAll().Where(l => l.Code == languageDto.Code || l.Name == languageDto.Name).FirstOrDefaultAsync();
            if (existingLanguage != null)
                return APIResponse<string>.FailureResponse(400, null, "Language with the same code or name already exists.");

            var language = new Language
            {
                Code = languageDto.Code,
                Name = languageDto.Name
            };

            await unitOfWork.Repository<Language>().AddAsync(language);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to add language. Please try again later.");
            return APIResponse<string>.SuccessResponse(200, "Language added successfully.", "Language added successfully.");
        }
    
        public async Task<APIResponse<string>> Update(int Id, LanguageDTORequest languageDto)
        {
            var language = await unitOfWork.Repository<Language>().GetByIdAsync(Id);

            if (language == null)
                return APIResponse<string>.FailureResponse(404, null, "Language not found.");

            language.Name = languageDto.Name;
            language.Code = languageDto.Code;

            unitOfWork.Repository<Language>().Update(language);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to update language. Please try again later.");
            return APIResponse<string>.SuccessResponse(200, "Language updated successfully.", "Language updated successfully.");
        }
    
        public async Task<APIResponse<string>> Delete(int Id)
        {
            var language = await unitOfWork.Repository<Language>().GetByIdAsync(Id);

            if (language == null)
                return APIResponse<string>.FailureResponse(404, null, "Language not found.");

            unitOfWork.Repository<Language>().Delete(language);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to delete language. Please try again later.");
            return APIResponse<string>.SuccessResponse(200, "Language deleted successfully.", "Language deleted successfully.");
        }

    }
}
