﻿using DotNet_RPG.DTO.Character;
using DotNet_RPG.Services.CharacterService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNet_RPG.Controllers
{
    [Route("api/[controller]")] // enables us to access the controller by the string "api and the name of the controlles which is "Character"
    [ApiController] //attribute that indicates this controller serves http api responses, enables http routing and responses
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService; //dependency injection of the character service into the controller

        public CharacterController(ICharacterService characterService) 
        {
            _characterService = characterService;
        }

        [HttpGet("GetAll")] //routing attributes to specify which http get method is being used 
        public  async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> Get()
        {
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("{id}")] //routing attributes to specify which http get method is being used... use {} because we pass a parameter
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharacter(id));
        }
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> AddCharacter (AddCharacterDTO newCharacter)
        {
           return Ok(await _characterService.AddCharacter(newCharacter));
        }
    }
}
