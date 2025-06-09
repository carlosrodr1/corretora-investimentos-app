using Investimentos.Api.Models;

namespace Investimentos.Api.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByIdAsync(int id);
        Task<decimal> GetTotalCorretagemAsync(int usuarioId);
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByEmailAsync(string email);
        Task CreateAsync(Usuario usuario);

    }
}
