using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DevHire.Application.ServiceContracts;
using DTO;
using Asp.Versioning;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace DevHire.WebAPI.Controllers.v2
{
    /// <summary>
    /// v2 - Developer Controller
    /// </summary>
    ///
    [Authorize] // Applies to all actions in this controller
    [ApiController]
    [Route("api/v{version:ApiVersion}/[controller]")]
    //[ApiVersion("1.0")]
    //[ApiVersion("2.0")]
    public class DevelopersController : ControllerBase
    {
        private readonly IDevelopersService? _developersService;
        private readonly ILogger<DevelopersController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="developersService"></param>
        /// <param name="logger"></param>
        public DevelopersController(IDevelopersService developersService, ILogger<DevelopersController> logger)
        {
            _developersService = developersService;
            _logger = logger;
        }

        /// <summary>
        /// Get all developers - Version 1.0
        /// </summary>
        /// <returns>List of developers</returns>
        [HttpGet]
        [ApiVersion("1.0")]
        //[MapToApiVersion("1.0")]
        public async Task<ActionResult<List<DeveloperDTO>>> GetAllDevelopersV1()
        {
            _logger.LogInformation("GetAllDevelopersV1 Action Method called");

            if (_developersService == null)
            {
                return BadRequest("Developers service is not available.");
            }

            var developerResponse = await _developersService.GetAllDevelopers();

            if (developerResponse == null || developerResponse.Count == 0)
            {
                return NotFound("No developers found.");
            }

            return Ok(developerResponse);
        }

        /// <summary>
        /// Get all developers - Version 2.0
        /// </summary>
        /// <returns>List of developers</returns>       
        [HttpGet]
        [ApiVersion("2.0")]
        //[MapToApiVersion("1.0")]
        public async Task<ActionResult<List<DeveloperDTO>>> GetAllDevelopersV2()
        {
            _logger.LogInformation("GetAllDevelopersV1 Action Method called");

            if (_developersService == null)
            {
                return BadRequest("Developers service is not available.");
            }

            var developerResponse = await _developersService.GetAllDevelopers();

            if (developerResponse == null || developerResponse.Count == 0)
            {
                return NotFound("No developers found.");
            }

            return Ok(developerResponse);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ApiVersion("1.0")]
        public async Task<ActionResult<DeveloperDTO>> GetAllDevelopersById(Guid? id)
        {
            if (_developersService == null)
            {
                return BadRequest("Developers service is not available.");
            }

            if (id == null)
            {
                return BadRequest("Developer ID is not provided.");
            }

            var developerResponse = await _developersService.GetDeveloperById(id);

            if (developerResponse == null)
            {
                return NotFound("No developers found.");
            }

            return Ok(developerResponse);
        }

      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="developerDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ApiVersion("1.0")]
        public async Task<ActionResult> UpdateDeveloper(Guid? id, DeveloperDTO developerDTO)
        {

            if (_developersService == null)
            {
                return BadRequest("Developers service is not available.");
            }

            if (id != developerDTO.Id || id == null || developerDTO == null)
            {
                return BadRequest();
            }
            
            var developerResponse = await _developersService.UpdateDeveloper(developerDTO);

            if (developerResponse == null)
            {
                return NotFound();
            }
          
            return Ok(developerResponse);
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="developerDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0")]
        public async Task<ActionResult<DeveloperDTO>> CreateDeveloper([FromBody] DeveloperDTO developerDTO)
        {

            if (_developersService == null)
            {
                return BadRequest("Developers service is not available.");
            }

            if (developerDTO == null)
            {
                return BadRequest();
            }

            var developerResponse = await _developersService.CreateDeveloper(developerDTO);

            if (developerResponse == null)
            {
                return NotFound();
            }

           return Ok(developerResponse);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="developerDTO"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [ApiVersion("1.0")]
        public async Task<ActionResult> PatchDeveloper(Guid? id, JsonPatchDocument<DeveloperDTO> developerDTO)
        {
            if (_developersService == null)
            {
                return BadRequest("Developers service is not available.");
            }

            if (id == null)
            {
                return BadRequest("Developer ID is not provided.");
            }

            if (developerDTO == null)
            {
                return BadRequest("Invalid patch document.");
            }

            var developerResponse = await _developersService.GetDeveloperById(id);

            if (developerResponse == null)
            {
                return NotFound("No developers found.");
            }

            developerDTO.ApplyTo(developerResponse);

            var developerRes = await _developersService.PatchDeveloper(id, developerResponse);

            return Ok(developerRes);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ApiVersion("1.0")]
        public async Task<ActionResult> DeleteDeveloper(Guid? id)
        {
            if (_developersService == null)
            {
                return BadRequest("Developers service is not available.");
            }

            var result = await _developersService.DeleteDeveloper(id);

            if (!result)
            {
                return NotFound("No developers found.");
            }

            return NoContent();
        }
    }
}
