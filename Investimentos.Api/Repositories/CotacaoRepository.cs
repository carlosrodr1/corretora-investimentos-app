using Investimentos.Api.Context;
using Investimentos.Api.Models;
using Investimentos.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Investimentos.Api.Repositories
{
    public class CotacaoRepository : ICotacaoRepository
    {
        private readonly AppDbContext _context;

        public CotacaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cotacao?> GetUltimaCotacaoAsync(int ativoId)
        {
            return await _context.Cotacoes
                .Where(c => c.AtivoId == ativoId)
                .OrderByDescending(c => c.DataHora)
                .FirstOrDefaultAsync();
        }
    }
}
