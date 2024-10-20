
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
        public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(GetCharacterDTO newCharacter)   
        {
            //we can pass as data to the service response, since it is a generic property, the list of characters
            // the two other properties have default values, but we can use them to pass error messages during runtime
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>(); 
            //the Characters type is Charecter, but the newCharacter type is AddCharacterDTO
            //so bellow we map the Character type to AddCharacterDTO type
            characters.Add(_mapper.Map<Character>(newCharacter)); 
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
    }
}
