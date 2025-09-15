
using AutoMapper;
using DotNet_RPG.Data;
using DotNet_RPG.DTO.Character;
using DotNet_RPG.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; //use this insted of using System.Data.Entity; because it causes an error 
                              //System.InvalidOperationException: The source IQueryable doesn't implement IDbAsyncEnumerable

namespace DotNet_RPG.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
      
        private readonly IMapper _mapper;

        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper,DataContext context, IHttpContextAccessor httpContextAccessor) //dbcontext dependence injection
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // async asynchronous calls for multithreaded application and faster responses 
        public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter)
        {
            //we can pass as data to the service response, since it is a generic property, the list of characters
            // the two other properties have default values, but we can use them to pass error messages during runtime
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            Character character = _mapper.Map<Character>(newCharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(u=>u.Id == GetUserId());
            //the Characters type is Charecter, but the newCharacter type is AddCharacterDTO
            //so bellow we map the Character type to AddCharacterDTO type
            _context.Character.Add(character);
            await _context.SaveChangesAsync(); //with he savechanges we write the new character to the databases

            // the charactes type is Character, and we use lamda to map every charactes c in the list
            // with the _mapper to the dto GetCharacterDTO, and the method ToList because characters is a list
            serviceResponse.Data =  await _context
                .Character
                .Select(c => _mapper.Map<GetCharacterDTO>(c))
                .ToListAsync(); //implementation with entity framework
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            var dbCharacters = await _context.Character.Where(c=>c.User!.Id == GetUserId()).ToListAsync();

            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDTO>();
            var dbCharacter = await _context.Character.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
            serviceResponse.Data = _mapper.Map<GetCharacterDTO>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacter)
        {
            ServiceResponse<GetCharacterDTO> response
               = new ServiceResponse<GetCharacterDTO>();

            try
            {
                var character = 
                    await _context
                    .Character
                    .FirstOrDefaultAsync(c => c.Id == updateCharacter.Id && c.User!.Id == GetUserId());
                if (character == null)
                {
                    throw new Exception($" Character with id '{updateCharacter.Id}' does not exist");
                }

                //_mapper.Map(updateCharacter, character); //do this instead of that bellow
                //if we want to update only specific properties it's better not to use automapper

                character.Name = updateCharacter.Name;
                character.Strenght = updateCharacter.Strenght;
                character.Deffence = updateCharacter.Deffence;
                character.HitPoints = updateCharacter.HitPoints;
                character.Class = updateCharacter.Class;
                character.Intelligence = updateCharacter.Intelligence;

                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetCharacterDTO>(character);

                
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

            }

            return response;

        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDTO>> response = new ServiceResponse<List<GetCharacterDTO>>();

            try
            {
                var character = await _context.Character
                    .Include(u=>u.User) //include User entity in the character entity, so we can carry the actual user id that corresponds to the character
                    .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
                if (character == null || character.User!.Id != GetUserId())
                {
                    throw new Exception($" Character with id '{id}' does not exist");
                }
                _context.Remove(character);

                await _context.SaveChangesAsync();

                response.Data = 
                    await _context
                    .Character
                    .Select(c=> _mapper.Map<GetCharacterDTO>(c)).ToListAsync();

            }
            catch (Exception ex)
            {
                response.Success=false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
