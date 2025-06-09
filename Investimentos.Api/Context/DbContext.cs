using Investimentos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Investimentos.Api.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ativo> Ativos { get; set; }
        public DbSet<Operacao> Operacoes { get; set; }
        public DbSet<Cotacao> Cotacoes { get; set; }
        public DbSet<Posicao> Posicoes { get; set; }
    }
}
