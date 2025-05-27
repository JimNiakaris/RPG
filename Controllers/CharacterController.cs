using System.Security.Claims;
using DotNet_RPG.DTO.Character;
using DotNet_RPG.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNet_RPG.Controllers
{
    [Authorize] //must login to use the http requests
    [Route("api/[controller]")] // enables us to access the controller by the string "api and the name of the controlles which is "Character"
    [ApiController] //attribute that indicates this controller serves http api responses, enables http routing and responses
    public class CharacterController : ControllerBase //ControllerBase has a property for User ClaimsPrincipal
    {
        private readonly ICharacterService _characterService; //dependency injection of the character service into the controller

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }
        //[AllowAnonymous] // if we use this attribute, that specidic request can be used by anyone
        [HttpGet("GetAll")] //routing attributes to specify which http get method is being used 
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> Get()
        {
            //Attention, the User bellow is not the User model that exists in the database but the User Property of the 
            //Controller Base
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            return Ok(await _characterService.GetAllCharacters(userId));
        }

        [HttpGet("{id}")] //routing attributes to specify which http get method is being used... use {} because we pass a parameter
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharacter(id));
        }
        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> Delete(int id)
        {
            var response = await _characterService.DeleteCharacter(id);
            if(response.Data == null)
            {
                return NotFound(response.Message);
            }
            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> AddCharacter(AddCharacterDTO newCharacter)
        {
            return Ok(await _characterService.AddCharacter(newCharacter));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> UpdateCharachter(UpdateCharacterDTO updatedCharacter)
        {
            var response = await _characterService.UpdateCharacter(updatedCharacter);
            if (response.Data == null) 
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
