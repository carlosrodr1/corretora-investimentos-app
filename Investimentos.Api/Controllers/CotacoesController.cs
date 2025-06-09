using Investimentos.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Investimentos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CotacoesController : ControllerBase
    {
        private readonly ICotacaoRepository _cotacaoRepository;

        public CotacoesController(ICotacaoRepository cotacaoRepository)
        {
            _cotacaoRepository = cotacaoRepository;
        }

        [HttpGet("ultima/{ativoId:int}")]
        public async Task<IActionResult> GetUltimaCotacao(int ativoId)
        {
            var cotacao = await _cotacaoRepository.GetUltimaCotacaoAsync(ativoId);

            if (cotacao == null)
                return NotFound("Cotação não encontrada.");

            return Ok(new
            {
                preco = Math.Round(cotacao.PrecoUnitario, 4),
                data = cotacao.DataHora
            });
        }


    }
}
