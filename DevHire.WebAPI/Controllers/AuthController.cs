using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DevHire.Application.DTO;
using DevHire.Application.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using DevHire.Domain.IdentityEntities;
using DTO;
using Entities;
using AutoMapper;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using DevHire.Application.Enum;
using System.Data;
using Asp.Versioning;
using System.Security.Claims;

namespace DevHire.WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;       
        private readonly SignInManager<User> _signInManager;
       
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="mapper"></param>
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IMapper mapper, IJwtService jwtService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registerDTO"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [ApiVersion("1.0")]
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

            if (!ModelState.IsValid)
            {
                string errorMessage = string.Join("|", ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessage);
            }

            User user = _mapper.Map<User>(registerDTO);

            // Create User
            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                if (registerDTO.UserType == UserType.Developer)
                {
                    //Create 'Developer' role if it never got added
                    if (await _roleManager.FindByNameAsync(UserType.Developer.ToString()) is null)
                    {
                        Role role = new Role() { Name = UserType.Developer.ToString() };
                        await _roleManager.CreateAsync(role);
                    }

                    //Add the new user into 'Developer' role
                    await _userManager.AddToRoleAsync(user, UserType.Developer.ToString());
                }
                else if (registerDTO.UserType == UserType.Employer)
                {
                    //Create 'Employee' if it never got added
                    if (await _roleManager.FindByNameAsync(UserType.Employer.ToString()) is null)
                    {
                        Role role = new Role() { Name = UserType.Employer.ToString() };
                        await _roleManager.CreateAsync(role);
                    }

                    //Add the new user into 'Employee' role in UserRole Table
                    await _userManager.AddToRoleAsync(user, UserType.Employer.ToString());
                }
                //Sign-In
                await _signInManager.SignInAsync(user, false);

                //Get the user Role details           
                var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

                if (userRole == null)
                {
                    return Problem("User role not found");
                }

                //Create a JWTToken and Refresh Token using Jwt Service
                var authenticationResponse = _jwtService.GenerateJwtToken(user, userRole);

                if (authenticationResponse != null) { 

                    user.RefreshToken = authenticationResponse.RefreshToken;
                    user.RefreshTokenExpiration = authenticationResponse.RefreshTokenExpiration;
                    await _userManager.UpdateAsync(user);                
                }

                return Ok(authenticationResponse);
            }
            else
            {
                string errorMessage = string.Join("|", result.Errors.Select(e => e.Description));
                return Problem(errorMessage);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<ActionResult> IsEmailAlreadyRegistered(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email cannot be null or empty");
            }

            User? user = await _userManager.FindByEmailAsync(email!);

            if (user == null)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ApiVersion("1.0")]
        public async Task<ActionResult> Login(LoginDTO loginDTO)
        {

            if (!ModelState.IsValid)
            {
                string errorMessage = string.Join("|", ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessage);
            }

            if (loginDTO == null)
            {
                return BadRequest("LoginDTO cannot be null");
            }

            if (string.IsNullOrEmpty(loginDTO.Email))
            {
                return BadRequest("Email cannot be null or empty");
            }

            if (string.IsNullOrEmpty(loginDTO.Password))
            {
                return BadRequest("Password cannot be null or empty");
            }

           var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, false, false);

            if (result.Succeeded)
            {
                //Get the User details
                User? user = await _userManager.FindByEmailAsync(loginDTO.Email);
                     
                if (user == null)
                {
                    return Problem("User is not found");
                }
             
                //Sign-In
                await _signInManager.SignInAsync(user, false);

                //Get the user Role details           
                var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();         

                if (userRole == null)
                {
                    return Problem("User role not found");
                }

                // Create a JWTToken and Refresh Token using Jwt Service
                var authenticationResponse = _jwtService.GenerateJwtToken(user, userRole);              

                if (authenticationResponse != null)
                {
                    user.RefreshToken = authenticationResponse.RefreshToken;
                    user.RefreshTokenExpiration = authenticationResponse.RefreshTokenExpiration;
                    await _userManager.UpdateAsync(user);
                }

                return Ok(authenticationResponse);              

            }
            else
            {
                return Problem();             
            }
                
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenDTO"></param>
        /// <returns></returns>
        [HttpPost("refresh")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> RefreshToken(TokenDTO tokenDTO)
        {
            if (tokenDTO == null)
                return BadRequest("Invalid token");
        
            var principal = _jwtService.GetClaimsPrincipalFromAccessToken(tokenDTO.AccessToken);

            if (principal == null)
                return BadRequest("Invalid Access token");

            string? email = principal.FindFirstValue(ClaimTypes.Email);

            if (email == null)
                return BadRequest("Invalid Access token");

            //Get the user  details   
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Problem("User is not found");
            }

            //Get the user Role details           
            var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            if (userRole == null)
            {
                return Problem("User role not found");
            }

            if (user == null || user.RefreshToken != tokenDTO.RefreshToken || user.RefreshTokenExpiration <= DateTime.UtcNow)
                return BadRequest("Invalid Refresh token.");

            // Create a JWTToken and Refresh Token using Jwt Service
            var authenticationResponse = _jwtService.GenerateJwtToken(user, userRole);

            if (authenticationResponse != null)
            {
                user.RefreshToken = authenticationResponse.RefreshToken;
                user.RefreshTokenExpiration = authenticationResponse.RefreshTokenExpiration;
                await _userManager.UpdateAsync(user);
            }

            return Ok(authenticationResponse);                
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        [ApiVersion("1.0")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();

        }
    }
}
