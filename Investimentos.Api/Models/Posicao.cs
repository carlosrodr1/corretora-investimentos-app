using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Investimentos.Api.Models
{
    [Table("posicao")]
    public class Posicao
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        [ForeignKey("Ativo")]
        public int AtivoId { get; set; }
        public Ativo Ativo { get; set; } = null!;

        public int Quantidade { get; set; }

        [Column("preco_medio", TypeName = "decimal(15,4)")]
        public decimal PrecoMedio { get; set; }

        [Column("pl", TypeName = "decimal(15,2)")]
        public decimal PL { get; set; }
    }

}
