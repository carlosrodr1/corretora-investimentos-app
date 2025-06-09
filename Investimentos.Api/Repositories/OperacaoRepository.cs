using Investimentos.Api.Context;
using Investimentos.Api.Models;
using Investimentos.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Investimentos.Api.Repositories
{
    public class OperacaoRepository : IOperacaoRepository
    {
        private readonly AppDbContext _context;

        public OperacaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Operacao>> GetOperacoesPorUsuarioAsync(int usuarioId)
        {
            return await _context.Operacoes
                .Include(o => o.Ativo)
                .Where(o => o.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<List<Operacao>> GetComprasPorUsuarioEAtivoAsync(int usuarioId, int ativoId)
        {
            return await _context.Operacoes
                .Where(o => o.UsuarioId == usuarioId && o.AtivoId == ativoId && o.TipoOperacao == "compra")
                .ToListAsync();
        }

        public async Task<List<Usuario>> GetTodosUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }
        public async Task CreateAsync(Operacao operacao)
        {
            _context.Operacoes.Add(operacao);
            await _context.SaveChangesAsync();
        }


    }
}
