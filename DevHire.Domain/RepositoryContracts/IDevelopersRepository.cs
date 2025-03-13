using Entities;

namespace RepositoryContracts
{
    public interface IDevelopersRepository
    {
        Task<List<Developer>> GetAllDevelopers();
        Task<Developer?> GetDeveloperById(Guid? developerID);
        Task<Developer> CreateDeveloper(Developer developer);            
        Task<Developer> UpdateDeveloper(Developer developer);
        Task<Developer> PatchDeveloper(Developer developer);
        Task<bool> DeleteDeveloper(Guid? developerID);
    }
}
