using Investimentos.Api.Context;
using Investimentos.Api.Models;
using Investimentos.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Investimentos.Api.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<decimal> GetTotalCorretagemAsync(int usuarioId)
        {
            return await _context.Operacoes
                .Where(o => o.UsuarioId == usuarioId)
                .SumAsync(o => o.Corretagem);
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task CreateAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }


    }
}
