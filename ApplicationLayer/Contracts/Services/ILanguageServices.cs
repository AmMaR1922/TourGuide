using ApplicationLayer.DTOs.Language;
using ApplicationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Services
{
    public interface ILanguageServices
    {
        Task<APIResponse<List<LanguageDTOResponse>>> GetAll();
        Task<APIResponse<LanguageDTOResponse>> GetById(int Id);
        Task<APIResponse<string>> Add(LanguageDTORequest languageDto);
        Task<APIResponse<string>> Update(int Id, LanguageDTORequest languageDto);
        Task<APIResponse<string>> Delete(int Id);
    }
}
