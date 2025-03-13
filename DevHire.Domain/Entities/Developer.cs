using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Developer
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        public int? YearsOfExperience { get; set; }

        [StringLength(50)]
        public string? FavoriteLanguage { get; set; }

       

    }
}
