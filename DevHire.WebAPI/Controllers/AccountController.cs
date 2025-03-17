using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DevHire.Application.DTO;
using DevHire.Application.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using DevHire.Domain.IdentityEntities;
using DTO;
using Entities;
using AutoMapper;

namespace DevHire.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public async Task<ActionResult<RegisterDTO>> Register(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
            {
                return BadRequest("RegisterDTO cannot be null");
            }

            if (string.IsNullOrEmpty(registerDTO.Password))
            {
                return BadRequest("Password cannot be null or empty");
            }

            ApplicationUser applicationUser = _mapper.Map<ApplicationUser>(registerDTO);

            IdentityResult result = await _userManager.CreateAsync(applicationUser, registerDTO.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }
    }
}
