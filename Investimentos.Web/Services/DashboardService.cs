using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Investimentos.Web.ViewModel;

namespace Investimentos.Web.Services
{
    public class DashboardService
    {
        private readonly HttpClient _client;

        public DashboardService(HttpClient client)
        {
            _client = client;
        }

        public async Task<DashboardViewModel> GetDashboardAsync(int usuarioId)
        {
            var totalInvestidoTask = _client.GetFromJsonAsync<TotalInvestidoDto>($"/api/investimentos/total-investido/{usuarioId}");
            var plTask = _client.GetFromJsonAsync<LucroPrejuizoDto>($"/api/investimentos/pl/{usuarioId}");
            var corretagemTask = _client.GetFromJsonAsync<TotalCorretagemDto>($"/api/usuarios/{usuarioId}/corretagem");
            var topInvestimentoTask = _client.GetFromJsonAsync<List<TopUsuarioViewModel>>($"/api/investimentos/top10-investimento");
            var topCorretagemTask = _client.GetFromJsonAsync<List<TopUsuarioViewModel>>($"/api/investimentos/top10-corretagem");

            await Task.WhenAll(totalInvestidoTask, plTask, corretagemTask, topInvestimentoTask, topCorretagemTask);

            return new DashboardViewModel
            {
                TotalInvestido = totalInvestidoTask.Result?.total_investido ?? 0,
                LucroPrejuizo = plTask.Result?.lucro_prejuizo ?? 0,
                TotalCorretagem = corretagemTask.Result?.total_corretagem ?? 0,
                Top10Investimento = topInvestimentoTask.Result ?? new(),
                Top10Corretagem = topCorretagemTask.Result ?? new()
            };
        }

        private class TotalInvestidoDto
        {
            public decimal total_investido { get; set; }
        }
        private class LucroPrejuizoDto
        {
            public decimal lucro_prejuizo { get; set; }
        }
        private class TotalCorretagemDto
        {
            public decimal total_corretagem { get; set; }
        }
    }
}
