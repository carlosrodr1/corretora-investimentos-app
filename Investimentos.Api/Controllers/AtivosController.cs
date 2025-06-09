using Investimentos.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Investimentos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtivosController : ControllerBase
    {
        private readonly IAtivoRepository _ativoRepository;

        public AtivosController(IAtivoRepository ativoRepository)
        {
            _ativoRepository = ativoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ativos = await _ativoRepository.GetAllAsync();
            return Ok(ativos);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar(string termo)
        {
            var ativos = await _ativoRepository.BuscarPorPrefixoAsync(termo);
            return Ok(ativos.Select(a => new { codigo = a.Codigo, nome = a.Nome }));
        }

        [HttpGet("codigo/{codigo}")]
        public async Task<IActionResult> GetPorCodigo(string codigo)
        {
            var ativo = await _ativoRepository.GetByCodigoAsync(codigo);
            if (ativo == null)
                return NotFound("Ativo não encontrado.");

            return Ok(new { id = ativo.Id, codigo = ativo.Codigo, nome = ativo.Nome });
        }


    }
}
