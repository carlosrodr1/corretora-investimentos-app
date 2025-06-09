using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Investimentos.Web.ViewModel;

namespace Investimentos.Web.Services
{
    public class UsuarioService
    {
        private readonly HttpClient _client;

        public UsuarioService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<UsuarioViewModel>> GetUsuariosAsync()
        {
            return await _client.GetFromJsonAsync<List<UsuarioViewModel>>("/api/usuarios") ?? new();
        }

        public async Task<UsuarioViewModel?> GetUsuarioAsync(int id)
        {
            return await _client.GetFromJsonAsync<UsuarioViewModel>($"/api/usuarios/{id}");
        }

        public async Task<UsuarioViewModel?> LoginAsync(LoginViewModel model)
        {
            var response = await _client.PostAsJsonAsync("/api/usuarios/login", model);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<UsuarioViewModel>();
        }

        public async Task<bool> CadastrarAsync(CadastroViewModel model)
        {
            var response = await _client.PostAsJsonAsync("/api/usuarios/registrar", model);
            return response.IsSuccessStatusCode;
        }
    }
}
