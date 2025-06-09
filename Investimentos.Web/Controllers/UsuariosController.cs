using Microsoft.AspNetCore.Mvc;
using Investimentos.Web.Services;
using Investimentos.Web.ViewModel;

namespace Investimentos.Web.Controllers
{
    public class UsuariosController : Controller
    {

        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioService.GetUsuariosAsync();
            return View(usuarios);
        }

        public async Task<IActionResult> Detalhes()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
                return RedirectToAction("Login", "Auth");

            var vm = await _usuarioService.GetDetalhesAsync(usuarioId.Value);
            if (vm == null)
                return NotFound();

            return View(vm);
        }


    }
}
