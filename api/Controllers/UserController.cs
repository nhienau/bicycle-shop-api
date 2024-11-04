using api.Dtos.Product;
using api.Dtos.User;
using api.Interfaces;
using api.Mappers;
using api.Models;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequestDTO userDTO)
        {
            User user= userDTO.ToUserFromCreateDTO();

            await _userRepo.CreateAsync(user);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user.ToUserDto());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            User? user = await _userRepo.GetByIdAsync(id);
            if (user == null || (user != null && !user.Status))
            {
                return NotFound();
            }
            return Ok(user.ToUserDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserRequestDTO userDTO)
        {
            User? user = await _userRepo.UpdateAsync(id, userDTO);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.ToUserDto());
        }


        [HttpPut]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            User? user= await _userRepo.DeleteAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.ToUserDto());
        }
    }
}
