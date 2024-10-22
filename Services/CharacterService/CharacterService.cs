
using AutoMapper;
using DotNet_RPG.DTO.Character;
using DotNet_RPG.Models;

namespace DotNet_RPG.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>
        {
            new Character(),
            new Character{Id = 1 ,Name = "Frodo"}
        };
        
        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper) 
        {
            _mapper = mapper;
        }

        // async asynchronous calls for multithreaded application and faster responses 
        public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter)   
        {
            //we can pass as data to the service response, since it is a generic property, the list of characters
            // the two other properties have default values, but we can use them to pass error messages during runtime
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>(); 
            Character character = _mapper.Map<Character>(newCharacter);
            character.Id = characters.Max(x => x.Id) + 1;
            //the Characters type is Charecter, but the newCharacter type is AddCharacterDTO
            //so bellow we map the Character type to AddCharacterDTO type
            characters.Add(character); 
            // the charactes type is Character, and we use lamda to map every charactes c in the list
            // with the _mapper to the dto GetCharacterDTO, and the method ToList because characters is a list
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters()
        {
            return new ServiceResponse<List<GetCharacterDTO>> 
            { 
                Data = characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList() 
            };
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDTO>();
            var character = characters.FirstOrDefault(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDTO>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacter)
        {
            ServiceResponse<GetCharacterDTO> response
               = new ServiceResponse<GetCharacterDTO>();

            try
            {
                Character character = characters.FirstOrDefault(c => c.Id == updateCharacter.Id);

                //_mapper.Map(updateCharacter, character); //do this instead of that bellow
                //if we want to update only specific properties it's better not to use automapper

                character.Name = updateCharacter.Name;
                character.Strenght = updateCharacter.Strenght;
                character.Deffence = updateCharacter.Deffence;
                character.HitPoints = updateCharacter.HitPoints;
                character.Class = updateCharacter.Class;
                character.Intelligence = updateCharacter.Intelligence;

                response.Data = _mapper.Map<GetCharacterDTO>(character);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

            }

            return response;

        }
    }
}
