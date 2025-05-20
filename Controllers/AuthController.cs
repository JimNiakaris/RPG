using DotNet_RPG.Data;
using DotNet_RPG.DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace DotNet_RPG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        //inject the dependency of Auth repository into the controller
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDTO request)
        {
            var response = await _authRepository.Register (new User { Name = request.Name}, request.Password);

            //if (!response.Success)
            //{
            //    return BadRequest();
            //}
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginDTO request)
        {
            var response = await _authRepository.Login(request.Name,request.Password);

            //if (!response.Success)
            //{
            //    return BadRequest();
            //}
            return Ok(response);
        }
    }
}
