using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaDeTarefas.Data;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorios;
using SistemaDeTarefas.Repositorios.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemaDeTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepositorio _loginRepositorio;

        public LoginController(ILoginRepositorio loginRepositorio)
        {
            _loginRepositorio = loginRepositorio;
        }

        [HttpPost("/register")]
        public async Task<ActionResult<LoginModel>> Registrar(LoginModel model)
        {
            var login = await _loginRepositorio.Registrar(model);

            if (login == null) {
                return BadRequest("Falha ao registrar login no banco de dados.");
            }

            return Ok(login);
        }

        [HttpPost("/login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            var token = _loginRepositorio.Login(login);
            return Ok(token);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _loginRepositorio.Apagar(id);

            return Ok(result);
        }
    }
}
