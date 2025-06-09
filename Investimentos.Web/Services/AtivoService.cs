using Investimentos.Web.ViewModel;

namespace Investimentos.Web.Services
{
    public class AtivoService
    {
        private readonly HttpClient _client;

        public AtivoService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<AtivoViewModel>> GetAtivosAsync()
        {
            return await _client.GetFromJsonAsync<List<AtivoViewModel>>("/api/ativos") ?? new();
        }
    }
}
