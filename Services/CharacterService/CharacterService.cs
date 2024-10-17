
using DotNet_RPG.Models;

namespace DotNet_RPG.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> Charachers = new List<Character>
        {
            new Character(),
            new Character{Id = 1 ,Name = "Frodo"}
        };
        public async Task<ServiceResponse<List<Character>>> AddCharacter(Character newCharacter)  // async asynchronous calls for multithreaded application and faster responses 
        {
            var serviceResponse = new ServiceResponse<List<Character>>();
            Charachers.Add(newCharacter);
            serviceResponse.Data = Charachers;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Character>>> GetAllCharacters()
        {
            return new ServiceResponse<List<Character>> { Data = Charachers};
        }

        public async Task<ServiceResponse<Character>> GetCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<Character>();
            var character = Charachers.FirstOrDefault(c => c.Id == id);
            serviceResponse.Data = character;
            return serviceResponse;
        }
    }
}
