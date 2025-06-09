using Investimentos.Api.Models;
using Investimentos.Api.Repositories.Interfaces;
using Investimentos.Api.Services.Interfaces;
using Investimentos.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Investimentos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IOperacaoRepository _operacaoRepository;
        private readonly IInvestimentoService _investimentoService;

        public UsuariosController(
            IUsuarioRepository usuarioRepository,
            IOperacaoRepository operacaoRepository,
            IInvestimentoService investimentoService)
        {
            _usuarioRepository = usuarioRepository;
            _operacaoRepository = operacaoRepository;
            _investimentoService = investimentoService;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return Ok(usuarios);
        }


        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar(CadastroViewModel model)
        {
            var existe = await _usuarioRepository.GetByEmailAsync(model.Email);
            if (existe != null)
                return BadRequest("Email já cadastrado.");

            var usuario = new Usuario
            {
                Nome = model.Nome,
                Email = model.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(model.Senha),
                CorretagemPercentual = model.CorretagemPercentual
            };

            await _usuarioRepository.CreateAsync(usuario);
            return Ok(usuario); 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(model.Email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.Senha, usuario.SenhaHash))
                return Unauthorized("Credenciais inválidas.");

            return Ok(usuario); 
        }

        [HttpGet("{usuarioId:int}")]
        public async Task<IActionResult> GetById(int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpGet("{usuarioId}/corretagem")]
        public async Task<IActionResult> GetTotalCorretagem(int usuarioId)
        {
            var total = await _usuarioRepository.GetTotalCorretagemAsync(usuarioId);
            return Ok(new { total_corretagem = Math.Round(total, 2) });
        }

        [HttpGet("detalhes/{usuarioId:int}")]
        public async Task<IActionResult> GetDetalhes(int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
                return NotFound();

            var operacoes = await _operacaoRepository.GetOperacoesPorUsuarioAsync(usuarioId);
            var posicoes = await _investimentoService.CalcularPosicoesAsync(usuarioId);
            var totalInvestido = await _investimentoService.CalcularTotalInvestidoAsync(usuarioId);
            var pl = await _investimentoService.CalcularTotalPLAsync(usuarioId);
            var totalCorretagem = await _usuarioRepository.GetTotalCorretagemAsync(usuarioId);

            var viewModel = new UsuarioDetalheViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                CorretagemPercentual = usuario.CorretagemPercentual,
                TotalInvestido = totalInvestido,
                TotalCorretagem = totalCorretagem,
                PL = pl,
                Posicoes = posicoes,
                Operacoes = operacoes.Select(o => new OperacaoViewModel
                {
                    Id = o.Id,
                    UsuarioId = o.UsuarioId,
                    AtivoCodigo = o.Ativo.Codigo,
                    TipoOperacao = o.TipoOperacao,
                    Quantidade = o.Quantidade,
                    PrecoUnitario = o.PrecoUnitario,
                    Corretagem = o.Corretagem,
                    DataHora = o.DataHora
                }).ToList()
            };

            return Ok(viewModel);
        }

    }
}
