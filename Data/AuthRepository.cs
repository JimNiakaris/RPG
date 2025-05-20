
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DotNet_RPG.Data
{
    public class AuthRepository : IAuthRepository        
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context) 
        {
            _context = context;
        }
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name.ToLower().Equals(username.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not Found";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong Password";
            }
            else response.Data= user.Id.ToString();

            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();

            if (await UserExists(user.Name))
            {
                response.Data = 0;
                response.Success = false;
                response.Message = "User already exists.";
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            response.Data = user.Id;
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(u => u.Name.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                Console.WriteLine("Stored Hash: " + BitConverter.ToString(passwordHash));
                Console.WriteLine("Stored Salt: " + BitConverter.ToString(passwordSalt));
            }
        }

        private bool VerifyPasswordHash (string password, byte[] passwordSalt, byte[] passwordHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                Console.WriteLine("Computed Hash: " + BitConverter.ToString(computedHash));
                Console.WriteLine("Stored Hash: " + BitConverter.ToString(passwordHash));
                Console.WriteLine("Stored Salt: " + BitConverter.ToString(passwordSalt));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
