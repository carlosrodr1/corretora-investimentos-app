using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Investimentos.Web.ViewModel;

namespace Investimentos.Web.Services
{
    public class OperacaoService
    {
        private readonly HttpClient _client;

        public OperacaoService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<OperacaoViewModel>> GetOperacoesPorUsuarioAsync(int usuarioId)
        {
            return await _client.GetFromJsonAsync<List<OperacaoViewModel>>($"/api/operacoes/por-usuario/{usuarioId}") ?? new();
        }

        public async Task CriarOperacaoAsync(OperacaoViewModel model)
        {
            var response = await _client.PostAsJsonAsync("/api/operacoes", model);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<AtivoViewModel>> BuscarPorTermoAsync(string termo)
        {
            return await _client.GetFromJsonAsync<List<AtivoViewModel>>($"/api/ativos/buscar?termo={termo}") ?? new();
        }
    }
}
