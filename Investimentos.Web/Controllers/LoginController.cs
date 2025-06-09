using Investimentos.Web.Services;
using Investimentos.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Investimentos.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public LoginController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult Entrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Entrar(LoginViewModel model)
        {
            var usuario = await _usuarioService.LoginAsync(model);
            if (usuario == null)
            {
                ViewBag.Erro = "Login inválido.";
                return View();
            }

            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
            HttpContext.Session.SetString("UsuarioEmail", usuario.Email);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(CadastroViewModel model)
        {
            var sucesso = await _usuarioService.CadastrarAsync(model);
            if (!sucesso)
            {
                ViewBag.Erro = "Falha ao cadastrar.";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Entrar");
        }
    }
}
