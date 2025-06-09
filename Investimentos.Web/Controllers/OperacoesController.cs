using Microsoft.AspNetCore.Mvc;
using Investimentos.Web.Services;
using Investimentos.Web.ViewModel;

namespace Investimentos.Web.Controllers
{
    public class OperacoesController : Controller
    {
        private readonly OperacaoService _operacaoService;
        private readonly UsuarioService _usuarioService;

        public OperacoesController(OperacaoService operacaoService, UsuarioService usuarioService)
        {
            _operacaoService = operacaoService;
            _usuarioService = usuarioService;
        }

        public async Task<IActionResult> Index(int usuarioId = 1)
        {
            var operacoes = await _operacaoService.GetOperacoesPorUsuarioAsync(usuarioId);
            return View(operacoes);
        }

        public async Task<IActionResult> Cadastrar()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
                return RedirectToAction("Entrar", "Login");

            ViewBag.UsuarioId = usuarioId;

            var usuario = await _usuarioService.GetUsuarioAsync(usuarioId.Value);
            ViewBag.Corretagem = usuario?.CorretagemPercentual ?? 0;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(OperacaoViewModel model)
        {
            try
            {
                model.UsuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;

                var usuario = await _usuarioService.GetUsuarioAsync(model.UsuarioId);
                model.Corretagem = usuario?.CorretagemPercentual ?? 0;
                model.DataHora = DateTime.Now;

                await _operacaoService.CriarOperacaoAsync(model);
                return RedirectToAction("Index", new { usuarioId = model.UsuarioId });
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Erro = "Erro ao cadastrar operação: " + ex.Message;
                ViewBag.UsuarioId = model.UsuarioId;

                var usuario = await _usuarioService.GetUsuarioAsync(model.UsuarioId);
                ViewBag.Corretagem = usuario?.CorretagemPercentual ?? 0;

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> BuscarAtivos(string termo, [FromServices] AtivoService ativoService)
        {
            var ativos = await ativoService.GetAtivosAsync();

            var filtrados = ativos
                .Where(a => a.Codigo.Contains(termo, StringComparison.OrdinalIgnoreCase)
                         || a.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase))
                .Select(a => new { id = a.Id, codigo = a.Codigo, nome = a.Nome })
                .Take(10)
                .ToList();

            return Json(filtrados);
        }


    }
}
