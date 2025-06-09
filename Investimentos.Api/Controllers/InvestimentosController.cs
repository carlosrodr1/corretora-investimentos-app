using Investimentos.Api.Services.Interfaces;
using Investimentos.Api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Investimentos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvestimentosController : ControllerBase
    {
        private readonly IInvestimentoService _investimentoService;

        public InvestimentosController(IInvestimentoService investimentoService)
        {
            _investimentoService = investimentoService;
        }

        [HttpGet("posicoes/{usuarioId:int}")]
        public async Task<IActionResult> GetPosicoes(int usuarioId)
        {
            var posicoes = await _investimentoService.CalcularPosicoesAsync(usuarioId);
            return Ok(posicoes);
        }

        [HttpGet("total-investido/{usuarioId:int}")]
        public async Task<IActionResult> GetTotalInvestido(int usuarioId)
        {
            var total = await _investimentoService.CalcularTotalInvestidoAsync(usuarioId);
            return Ok(new { total_investido = Math.Round(total, 2) });
        }

        [HttpGet("pl/{usuarioId:int}")]
        public async Task<IActionResult> GetPL(int usuarioId)
        {
            var pl = await _investimentoService.CalcularTotalPLAsync(usuarioId);
            return Ok(new { lucro_prejuizo = Math.Round(pl, 2) });
        }

        [HttpGet("top10-investimento")]
        public async Task<IActionResult> GetTop10Investimento()
        {
            var result = await _investimentoService.GetTop10PorInvestimentoAsync();
            return Ok(result);
        }

        [HttpGet("top10-corretagem")]
        public async Task<IActionResult> GetTop10Corretagem()
        {
            var result = await _investimentoService.GetTop10PorCorretagemAsync();
            return Ok(result);
        }

        [HttpGet("preco-medio/{usuarioId:int}/{ativoId:int}")]
        public async Task<IActionResult> GetPrecoMedio(int usuarioId, int ativoId)
        {
            var precoMedio = await _investimentoService.CalcularPrecoMedioAsync(usuarioId, ativoId);
            if (precoMedio == 0)
                return NotFound("Nenhuma compra encontrada para esse ativo.");

            return Ok(new { preco_medio = precoMedio });
        }




    }
}
