using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaDeTarefas.Data;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorios.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemaDeTarefas.Repositorios
{
    public class LoginRepositorio : ILoginRepositorio
    {
        private readonly SistemaTarefasDBContext _sistemaTarefasDBContext;
        public LoginRepositorio(SistemaTarefasDBContext sistemaTarefasDBContext)
        {
            _sistemaTarefasDBContext = sistemaTarefasDBContext;
        }

        public async Task<LoginModel> Registrar(LoginModel model)
        {
            var login = new LoginModel
            {
                Login = model.Login,
                Senha = BCrypt.Net.BCrypt.HashPassword(model.Senha) // Criptografando a senha
            };

            await _sistemaTarefasDBContext.Logins.AddAsync(login);
            _sistemaTarefasDBContext.SaveChanges();

            return login;
        }

        public string Login(LoginModel login)
        {
            // Buscar usuário no banco pelo login (email)
            var loginBanco = _sistemaTarefasDBContext.Logins.SingleOrDefault(u => u.Login == login.Login);

            if (loginBanco == null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            // Verificar se a senha é válida (comparando hash)
            if (!VerificarSenha(login.Senha, loginBanco.Senha))
            {
                throw new Exception("Senha inválida.");
            }

            // Gerar token JWT
            var token = GerarTokenJWT(loginBanco);

            return token;
        }

        public string GerarTokenJWT(LoginModel login)
        {
            string chaveSecreta = "50044666-5088-441f-a7c0-14d307ebfbad";

            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta));
            var credencial = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, login.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, login.Login),
            };

            var token = new JwtSecurityToken(
                issuer: "sua_empresa",
                audience: "sua_aplicacao",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credencial
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool VerificarSenha(string senhaDigitada, string senhaHash)
        {
            return BCrypt.Net.BCrypt.Verify(senhaDigitada, senhaHash);
        }

        public bool Apagar(int Id)
        {
            var login = _sistemaTarefasDBContext.Logins.FirstOrDefault(x => x.Id == Id);

            if (login == null) {
                throw new Exception("Falha ao remover registro do banco de dados.");
            }

            _sistemaTarefasDBContext.Remove(login);
            _sistemaTarefasDBContext.SaveChanges();

            return true;
        }
    }
}
