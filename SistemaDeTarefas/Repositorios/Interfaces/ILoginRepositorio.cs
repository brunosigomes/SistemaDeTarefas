using Microsoft.AspNetCore.Mvc;
using SistemaDeTarefas.Models;

namespace SistemaDeTarefas.Repositorios.Interfaces
{
    public interface ILoginRepositorio
    {
        public Task<LoginModel> Registrar(LoginModel model);
        public string Login(LoginModel login);
        public bool VerificarSenha(string senhaDigitada, string senhaHash);
        public string GerarTokenJWT(LoginModel login);
        public bool Apagar(int Id);
    }
}
