using Investimentos.Api.Models;

namespace Investimentos.Api.Repositories.Interfaces
{
    public interface IPosicaoRepository
    {
        Task<List<Posicao>> GetPosicoesPorUsuarioAsync(int usuarioId);
        Task<List<Posicao>> GetPosicoesPorAtivoAsync(int ativoId);
        Task AtualizarPLAsync(int ativoId, decimal precoAtual);
        Task<Posicao?> GetPosicaoAsync(int usuarioId, int ativoId);
        Task SalvarAsync(Posicao posicao);
    }
}
