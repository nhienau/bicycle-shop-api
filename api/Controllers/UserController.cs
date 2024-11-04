using api.Dtos.User;
using api.Interfaces;
using api.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserQueryDTO query)
        {
            PaginatedResponse<UserDTO> list = await _userRepo.GetAllAsync(query);
            return Ok(list);
        }
    }
}
