using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaDeTarefas.Integracao.Interfaces;
using SistemaDeTarefas.Integracao.Response;

namespace SistemaDeTarefas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CepController : ControllerBase
    {
        private readonly IViaCepIntegracao _viaCepIntegracao;
        public CepController(IViaCepIntegracao viaCepIntegracao)
        {
            _viaCepIntegracao = viaCepIntegracao;
        }

        [HttpGet("{cep}")]
        public async Task<ActionResult<ViaCepResponse>> ListarDadosDoEndereco(string cep)
        {
            var response = await _viaCepIntegracao.ObterDadosViaCep(cep);

            if (response == null)
            {
                return BadRequest("CEP não encontrado!");
            }

            return Ok(response);
        }
    }
}
