using Investimentos.Api.Context;
using Investimentos.Api.Models;
using Investimentos.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Investimentos.Api.Repositories
{
    public class AtivoRepository : IAtivoRepository
    {
        private readonly AppDbContext _context;

        public AtivoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ativo>> GetAllAsync()
        {
            return await _context.Ativos.ToListAsync();
        }

        public async Task<Ativo?> GetByCodigoAsync(string codigo)
        {
            return await _context.Ativos.FirstOrDefaultAsync(a => a.Codigo == codigo);
        }

        public async Task<List<Ativo>> BuscarPorPrefixoAsync(string termo)
        {
            return await _context.Ativos
                .Where(a => a.Codigo.StartsWith(termo) || a.Nome.Contains(termo))
                .OrderBy(a => a.Codigo)
                .Take(10)
                .ToListAsync();
        }



    }

}
