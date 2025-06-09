using Investimentos.Api.Models;

namespace Investimentos.Api.Repositories.Interfaces
{
    public interface ICotacaoRepository
    {
        Task<Cotacao?> GetUltimaCotacaoAsync(int ativoId);
    }
}
