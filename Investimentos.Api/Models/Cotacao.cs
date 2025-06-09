using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Investimentos.Api.Models
{
    [Table("cotacao")]
    public class Cotacao
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Ativo")]
        public int AtivoId { get; set; }

        public Ativo Ativo { get; set; } = null!;

        [Column(TypeName = "decimal(15,4)")]
        public decimal PrecoUnitario { get; set; }

        public DateTime DataHora { get; set; }
    }

}
