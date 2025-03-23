using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Entities;

namespace DBContext
{
    public class DevelopersDbContext : DbContext
    {        
        public DevelopersDbContext(DbContextOptions<DevelopersDbContext> options) : base(options)
        {
        }

        //Tables
        public DbSet<Developer> Developers {  get; set; }

        //SP
        public List<Developer> sp_GetAllDevelopers()
        {
            return Developers.FromSqlRaw("EXECUTE [dbo].[GetAllDevelopers]").ToList();
        }

        public Developer? sp_GetDeveloperById(Guid? Id)
        {

            SqlParameter[] sqlParameter = new SqlParameter[]
            {
                new SqlParameter("@Id", Id)              
            };

            return Developers.FromSqlRaw("EXECUTE [dbo].[GetDeveloperById] @Id", sqlParameter).AsEnumerable().FirstOrDefault();
        }

        public int sp_InsertDeveloper(Developer developer)
        {
            SqlParameter[] sqlParameter = new SqlParameter[]
            {
                new SqlParameter("@Id", developer.Id),
                new SqlParameter("@FirstName", developer.FirstName),
                new SqlParameter("@LastName", developer.LastName),
                new SqlParameter("@YearsOfExperience", developer.YearsOfExperience),
                new SqlParameter("@FavoriteLanguage", developer.FavoriteLanguage),
            };
            
                           
            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertDeveloper] @Id, @FirstName, @LastName , @YearsOfExperience, @FavoriteLanguage", sqlParameter);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Developer>().ToTable("Developers");

            //Seeding -- Reading Data from Developer.json                  
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string seedDataPath = Path.Combine(baseDirectory, @"..\..\..\..\DevHire.Infrastructure\SeedData\Developers.json");
            string developersJSON = System.IO.File.ReadAllText(seedDataPath);

            List<Developer>? developers = JsonSerializer.Deserialize<List<Developer>>(developersJSON);

            if(developers != null && developers.Count > 0)
            {
                foreach (Developer developer in developers)
                {
                    //Seed Data
                    modelBuilder.Entity<Developer>().HasData(developer);
                }
            }                       
        }
    }
}
