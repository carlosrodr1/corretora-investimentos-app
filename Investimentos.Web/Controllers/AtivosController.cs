using Investimentos.Web.Services;
using Investimentos.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Investimentos.Web.Controllers
{
    public class AtivosController : Controller
    {
        private readonly AtivoService _ativoService;

        public AtivosController(AtivoService ativoService)
        {
            _ativoService = ativoService;
        }

        public async Task<IActionResult> Index()
        {
            var ativos = await _ativoService.GetAtivosAsync();
            return View(ativos);
        }
    }
}
