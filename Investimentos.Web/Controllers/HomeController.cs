using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Investimentos.Web.Models;
using Investimentos.Web.ViewModel;
using Investimentos.Web.Services;

namespace Investimentos.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DashboardService _dashboardService;

        public HomeController(ILogger<HomeController> logger, DashboardService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
                return RedirectToAction("Entrar", "Login");

            try
            {
                var vm = await _dashboardService.GetDashboardAsync(usuarioId.Value);
                ViewBag.UsuarioId = usuarioId.Value;
                ViewBag.UsuarioNome = HttpContext.Session.GetString("UsuarioNome");
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados do Dashboard.");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
