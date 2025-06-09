using Investimentos.Api.Models;

namespace Investimentos.Api.Repositories.Interfaces
{
    public interface IAtivoRepository
    {
        Task<List<Ativo>> GetAllAsync();
        Task<Ativo?> GetByCodigoAsync(string codigo);
        Task<List<Ativo>> BuscarPorPrefixoAsync(string termo);

    }
}
