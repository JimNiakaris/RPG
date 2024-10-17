namespace DotNet_RPG.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<Character>>> GetAllCharacters();
        Task<ServiceResponse<Character>> GetCharacter(int id);
        Task<ServiceResponse<List<Character>>> AddCharacter(Character newCharacter);
    }
}
