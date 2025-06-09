using Investimentos.Api.Context;
using Investimentos.Api.Models;
using Investimentos.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Investimentos.Api.Repositories
{
    public class PosicaoRepository : IPosicaoRepository
    {
        private readonly AppDbContext _context;

        public PosicaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Posicao>> GetPosicoesPorUsuarioAsync(int usuarioId)
        {
            return await _context.Posicoes
                .Include(p => p.Ativo)
                .Where(p => p.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<List<Posicao>> GetPosicoesPorAtivoAsync(int ativoId)
        {
            return await _context.Posicoes
                .Include(p => p.Usuario)
                .Where(p => p.AtivoId == ativoId)
                .ToListAsync();
        }

        public async Task<Posicao?> GetPosicaoAsync(int usuarioId, int ativoId)
        {
            return await _context.Posicoes
                .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId && p.AtivoId == ativoId);
        }

        public async Task AtualizarPLAsync(int ativoId, decimal precoAtual)
        {
            var posicoes = await _context.Posicoes.Where(p => p.AtivoId == ativoId).ToListAsync();
            foreach (var pos in posicoes)
            {
                pos.PL = (precoAtual - pos.PrecoMedio) * pos.Quantidade;
            }
            await _context.SaveChangesAsync();
        }

        public async Task SalvarAsync(Posicao posicao)
        {
            if (posicao.Id == 0)
                _context.Posicoes.Add(posicao);
            else
                _context.Posicoes.Update(posicao);

            await _context.SaveChangesAsync();
        }
    }
}
