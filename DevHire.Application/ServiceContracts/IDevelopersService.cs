using Entities;
using DTO;

namespace DevHire.Application.ServiceContracts
{
    public interface IDevelopersService
    {
        Task<DeveloperDTO> CreateDeveloper(DeveloperDTO? developerRequestDTO);
        Task<DeveloperDTO?> GetDeveloperById(Guid? developerID);
        Task<List<DeveloperDTO>?> GetAllDevelopers();
        Task<DeveloperDTO> UpdateDeveloper(DeveloperDTO developerRequestDTO);
        Task<DeveloperDTO> PatchDeveloper(Guid? developerID, DeveloperDTO developerRequestDTO);
        Task<bool> DeleteDeveloper(Guid? developerID);
    }
}

