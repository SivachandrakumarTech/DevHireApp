using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DevHire.Application.DTO;
using DevHire.Domain.IdentityEntities;
using DTO;
using Entities;

namespace DevHire.Application.Mapping
{
    public class RegisterProfile : Profile
    {
       public RegisterProfile() {
            CreateMap<User, RegisterDTO>().ReverseMap(); //Reverse Mapping (Two way Mapping)

        }

    }
}
