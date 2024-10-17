namespace DotNet_RPG.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<List<Character>> GetAllCharacters();
        Task<Character> GetCharacter(int id);
        Task<List<Character>> AddCharacter(Character newCharacter);
    }
}
