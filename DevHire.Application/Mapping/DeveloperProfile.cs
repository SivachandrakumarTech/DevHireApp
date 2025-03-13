using System.Threading.Tasks;
using AutoMapper;
using DTO;
using Entities;

namespace Mapping
{
    public class DeveloperProfile : Profile
    {
        public DeveloperProfile() {      
            CreateMap<Developer, DeveloperDTO>().ReverseMap(); //Reverse Mapping (Two way Mapping)
            
            /* (One Way Mapping)
            CreateMap<Developer, DeveloperDTO>();
            CreateMap<DeveloperDTO, Developer>();
            */
        }
    }
}
