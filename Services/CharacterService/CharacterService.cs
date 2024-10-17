
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
        public async Task<List<Character>> AddCharacter(Character newCharacter)  // async asynchronous calls for multithreaded application and faster responses 
        {
            Charachers.Add(newCharacter);
            return Charachers;
        }

        public async Task<List<Character>> GetAllCharacters()
        {
            return Charachers;
        }

        public async Task<Character> GetCharacter(int id)
        {
            return Charachers.FirstOrDefault(c => c.Id == id);
        }
    }
}
