using Investimentos.Api.ViewModels;

namespace Investimentos.Api.Services.Interfaces
{
    public interface IInvestimentoService
    {
        Task<List<PosicaoViewModel>> CalcularPosicoesAsync(int usuarioId);
        Task<decimal> CalcularTotalInvestidoAsync(int usuarioId);
        Task<decimal> CalcularTotalPLAsync(int usuarioId);
        Task<List<UsuarioRankingViewModel>> GetTop10PorInvestimentoAsync();
        Task<List<UsuarioRankingViewModel>> GetTop10PorCorretagemAsync();
        Task<decimal> CalcularPrecoMedioAsync(int usuarioId, int ativoId);

    }
}
