using Investimentos.Api.Repositories.Interfaces;
using Investimentos.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Investimentos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtivosController : ControllerBase
    {
        private readonly IAtivoRepository _ativoRepository;

        private readonly ICotacaoRepository _cotacaoRepository;

        public AtivosController(IAtivoRepository ativoRepository, ICotacaoRepository cotacaoRepository)
        {
            _ativoRepository = ativoRepository;
            _cotacaoRepository = cotacaoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ativos = await _ativoRepository.GetAllAsync();
            var resultado = new List<AtivoViewModel>();

            foreach (var ativo in ativos)
            {
                var cotacao = await _cotacaoRepository.GetUltimaCotacaoAsync(ativo.Id);

                resultado.Add(new AtivoViewModel
                {
                    Id = ativo.Id,
                    Codigo = ativo.Codigo,
                    Nome = ativo.Nome,
                    UltimaCotacao = cotacao?.PrecoUnitario,
                    DataUltimaCotacao = cotacao?.DataHora
                });
            }

            return Ok(resultado);
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
