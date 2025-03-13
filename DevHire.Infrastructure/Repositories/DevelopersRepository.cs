using DBContext;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using Entities;

namespace Repositories
{
    public class DevelopersRepository : IDevelopersRepository
    {

        private readonly DevelopersDbContext _developersDbContext;

        public DevelopersRepository(DevelopersDbContext developersDbContext)
        {
            _developersDbContext = developersDbContext;
        }

        public async Task<List<Developer>> GetAllDevelopers()
        {
            //****Using Table****
            return await _developersDbContext.Developers.ToListAsync();

            //****Using SP*****
            //return _developersDbContext.sp_GetAllDevelopers();
        }

        public async Task<Developer?> GetDeveloperById(Guid? developerID)
        {
            //*****Using Table*****
            // Used .AsNoTracking() to Prevents tracking conflicts when fetching and updating with Patch
            return await _developersDbContext.Developers.AsNoTracking().FirstOrDefaultAsync(temp => temp.Id == developerID);

            //****Using SP*****
            //Developer? developer = _developersDbContext.sp_GetDeveloperById(developerID);
        }


        public async Task<Developer> CreateDeveloper(Developer developer)
        {

            //****Using Table****
            _developersDbContext.Developers.Add(developer);

            await _developersDbContext.SaveChangesAsync();

            return developer;

            // ****Using SP*****
            // _developersDbContext.sp_InsertDeveloper(developer);
        }

        public async Task<Developer> UpdateDeveloper(Developer developer)
        {
    
            _developersDbContext.Developers.Update(developer);

            await _developersDbContext.SaveChangesAsync();

            return developer;
        }

        public async Task<Developer> PatchDeveloper(Developer developer)
        {
            _developersDbContext.Developers.Update(developer);
            
            await _developersDbContext.SaveChangesAsync();

            return developer;
        }

        public async Task<bool> DeleteDeveloper(Guid? developerID)
        {
            Developer? developer = await _developersDbContext.Developers
                .FirstOrDefaultAsync(temp => temp.Id == developerID);

            if (developer == null)
            {
                return false;
            }

            _developersDbContext.Developers.Remove(developer);
            await _developersDbContext.SaveChangesAsync();
            return true;
        }

    }
}
