using Investimentos.Api.Models;
using Investimentos.Api.Repositories;
using Investimentos.Api.Repositories.Interfaces;
using Investimentos.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Investimentos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperacoesController : ControllerBase
    {
        private readonly IOperacaoRepository _operacaoRepository;
        private readonly IAtivoRepository _ativoRepository;

        public OperacoesController(IOperacaoRepository operacaoRepository, IAtivoRepository ativoRepository)
        {
            _operacaoRepository = operacaoRepository;
            _ativoRepository = ativoRepository;
        }


        [HttpGet("por-usuario/{usuarioId:int}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            var operacoes = await _operacaoRepository.GetOperacoesPorUsuarioAsync(usuarioId);

            var resultado = operacoes.Select(o => new OperacaoViewModel
            {
                Id = o.Id,
                UsuarioId = o.UsuarioId,
                AtivoCodigo = o.Ativo.Codigo,
                TipoOperacao = o.TipoOperacao,
                Quantidade = o.Quantidade,
                PrecoUnitario = o.PrecoUnitario,
                Corretagem = o.Corretagem,
                DataHora = o.DataHora
            }).ToList();

            return Ok(resultado);
        }


        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] OperacaoViewModel model)
        {
            var ativo = await _ativoRepository.GetByCodigoAsync(model.AtivoCodigo);
            if (ativo == null)
                return BadRequest("Ativo não encontrado.");

            var operacao = new Operacao
            {
                UsuarioId = model.UsuarioId,
                AtivoId = ativo.Id, 
                TipoOperacao = model.TipoOperacao,
                Quantidade = model.Quantidade,
                PrecoUnitario = model.PrecoUnitario,
                Corretagem = model.Corretagem,
                DataHora = model.DataHora
            };

            await _operacaoRepository.CreateAsync(operacao);
            return CreatedAtAction(nameof(GetByUsuario), new { usuarioId = model.UsuarioId }, operacao);
        }


    }
}
