using Investimentos.Api.Models;
using Investimentos.Api.Repositories.Interfaces;
using Investimentos.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Investimentos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuariosController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
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
    }
}
