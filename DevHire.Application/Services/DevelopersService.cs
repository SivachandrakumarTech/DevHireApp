using ServiceContracts;
using RepositoryContracts;
using DTO;
using Entities;
using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using Exceptions;

namespace Services
{
    public class DevelopersService : IDevelopersService
    {

        private readonly IDevelopersRepository _developersRepository;

        private readonly IMapper _mapper;

        public DevelopersService(IMapper mapper, IDevelopersRepository developersRepository)
        {
            _developersRepository = developersRepository;
              _mapper = mapper;
        }

        public async Task<List<DeveloperDTO>?> GetAllDevelopers()
        {
            List<Developer> developer = await _developersRepository.GetAllDevelopers();

            return developer.Select(item => _mapper.Map<DeveloperDTO>(item)).ToList();
        }


        public async Task<DeveloperDTO?> GetDeveloperById(Guid? developerID)
        {
            if (developerID == null || developerID == Guid.Empty)
            {
                //Using Custom Exception
                throw new InvalidDeveloperIDException("Developer ID cannot be null or empty.");
            }

            Developer? developer = await _developersRepository.GetDeveloperById(developerID);

            if (developer == null)
            {
                return null;
            }
            else
            {
                DeveloperDTO developerResponseDTO = _mapper.Map<DeveloperDTO>(developer);
                return developerResponseDTO;
            }
        }

        public async Task<DeveloperDTO> CreateDeveloper(DeveloperDTO? developerRequestDTO)
        {
            if (developerRequestDTO == null)
            {
                throw new ArgumentNullException(nameof(developerRequestDTO));
            }
            else
            {                
                Developer? developer = _mapper.Map<Developer>(developerRequestDTO);

                developer.Id = Guid.NewGuid();

                Developer? developerResponce = await _developersRepository.CreateDeveloper(developer);

                DeveloperDTO developerResponseDTO = _mapper.Map<DeveloperDTO>(developerResponce);

                return developerResponseDTO;
                                          
            }
        }                    

        public async Task<DeveloperDTO> UpdateDeveloper(DeveloperDTO developerRequestDTO)
        {
            if (developerRequestDTO == null)
            {
                throw new ArgumentNullException(nameof(developerRequestDTO));
            }
            else
            {                        
                Developer? developer = _mapper.Map<Developer>(developerRequestDTO);

                Developer? developerResponce = await _developersRepository.UpdateDeveloper(developer);              

                DeveloperDTO developerResponseDTO = _mapper.Map<DeveloperDTO>(developerResponce);

                return developerResponseDTO;
            }
        }


        public async Task<DeveloperDTO> PatchDeveloper(Guid? id, DeveloperDTO developerRequestDTO)
        {
            if (developerRequestDTO == null)
            {
                throw new ArgumentNullException(nameof(developerRequestDTO));
            }
            else
            {
                Developer? developer = _mapper.Map<Developer>(developerRequestDTO);

                Developer? developerResponce = await _developersRepository.PatchDeveloper(developer);

                DeveloperDTO developerResponseDTO = _mapper.Map<DeveloperDTO>(developerResponce);

                return developerResponseDTO;
            }
        }


        public async Task<bool> DeleteDeveloper(Guid? developerID)
        {
            if (developerID == null || developerID == Guid.Empty)
            {
                //Using Custom Exception
                throw new InvalidDeveloperIDException("Developer ID cannot be null or empty.");
            }

            bool result = await _developersRepository.DeleteDeveloper(developerID);

            return result;

        }       
    }
}
