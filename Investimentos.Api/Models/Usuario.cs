using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Investimentos.Api.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("senha_hash")]
        public string SenhaHash { get; set; } = string.Empty;

        [Column("corretagem_percentual", TypeName = "decimal(5,2)")]
        public decimal CorretagemPercentual { get; set; }
    }
}
