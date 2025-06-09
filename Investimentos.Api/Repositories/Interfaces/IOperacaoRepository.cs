using Investimentos.Api.Models;

namespace Investimentos.Api.Repositories.Interfaces
{
    public interface IOperacaoRepository
    {
        Task<List<Operacao>> GetOperacoesPorUsuarioAsync(int usuarioId);
        Task<List<Operacao>> GetComprasPorUsuarioEAtivoAsync(int usuarioId, int ativoId);
        Task<List<Usuario>> GetTodosUsuariosAsync();
        Task CreateAsync(Operacao operacao);
    }
}
