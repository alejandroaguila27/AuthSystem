using AuthSystem.API.Models;
using AuthSystem.Application.Services;
using AuthSystem.Domain.Entities;
using AuthSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuthSystem.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IUserRepository _userRepository;

        public UserController(UserService userService, IUserRepository userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                var newUser = await _userService.CreateUser(createUserDto.Username, createUserDto.Email, createUserDto.Password);
                return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Manejar errores de forma adecuada
            }
        }

        //[HttpPut("change-password")]
        //[Authorize] // Agregar autorización para este endpoint
        //public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        //{
        //    try
        //    {
        //        var userId = _userRepository.GetCurrentUserId();

        //        if (userId == null)
        //        {
        //            return Unauthorized();
        //        }

        //        var user = await _userRepository.GetByIdAsync(userId);

        //        if (user == null)
        //        {
        //            return NotFound();
        //        }

        //        if (!_userService.VerifyPassword(changePasswordDto.OldPassword, user.PasswordHash))
        //        {
        //            var problemDetails = new ProblemDetails
        //            {
        //                Status = 400,
        //                Detail = "La contraseña actual es incorrecta.",
        //            };
        //            return BadRequest(problemDetails);
        //        }

        //        user.PasswordHash = _userService.HashPassword(changePasswordDto.NewPassword);

        //        await _userRepository.UpdateAsync(user);

        //        return Ok("Contraseña actualizada con éxito");
        //    }
        //    catch (Exception ex)
        //    {
        //        var problemDetails = new ProblemDetails
        //        {
        //            Status = 400,
        //            Detail = ex.Message,
        //        };
        //        return BadRequest(problemDetails);
        //    }
        //}




        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id); // Obtener el usuario directamente desde el repositorio

            if (user == null)
            {
                return NotFound();
            }

            // Proyectar los datos del usuario al DTO
            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Username,
                Email = user.Email
            };

            return Ok(userDto); // Devolver el DTO en lugar del objeto de usuario completo
        }
    }
}
