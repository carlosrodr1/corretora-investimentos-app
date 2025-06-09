using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Investimentos.Api.Models
{
    [Table("ativo")]
    public class Ativo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

    }
}
