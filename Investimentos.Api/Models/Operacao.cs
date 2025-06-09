using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Investimentos.Api.Models
{
    [Table("operacao")]
    public class Operacao
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

        [Column(TypeName = "decimal(15,4)")]
        public decimal PrecoUnitario { get; set; }

        [Required]
        [MaxLength(10)]
        public string TipoOperacao { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Corretagem { get; set; }

        public DateTime DataHora { get; set; }
    }

}
