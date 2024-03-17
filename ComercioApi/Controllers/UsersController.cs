using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComercioApi.Context;
using ComercioApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using NuGet.Common;
using ComercioApi.Services.UserService;

namespace ComercioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            var usuarios = await _userService.GetUsersAsync();
            return Ok(usuarios);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            bool result=await _userService.UpdateUserAsync(id, user);
            if (result)
            {
                return Ok(new { message = "Usuario actualizado" });
            }
            else
            {
                return NotFound(new { message = "Error al actualizar Usuario" });
            }
        }



        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool result= await _userService.DeleteUserAsync(id);
            if (result)
            {
                return Ok(new { message = "Usuario eliminado" });
            }
            else
            {
                return NotFound(new { message = "Error al eliminar Usuario" });
            }
        }


        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(string usuario, string password)
        {
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Usuario y contraseña son requeridos.");
            }
            string token=await _userService.LoginAsync(usuario, password);
            if (token == null)
            {
                return Ok(new{message="Credenciales invalidas"});
            }
            return Ok(new { token = token });

        }


        // POST: api/Users/registro
        [HttpPost("registro")]
        public async Task<ActionResult<User>> RegistroUser(User user)
        {
            try
            {
                string token = await _userService.RegistroUserAsync(user);
                if (token == null)
                {
                    return Ok(new { message = "Credenciales invalidas" });
                }
                return Ok(new { token = token });               
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al registrar"+ ex);
            }
        }
        
    }
}
