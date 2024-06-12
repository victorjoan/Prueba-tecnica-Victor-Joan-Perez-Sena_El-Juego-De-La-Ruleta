using System.ComponentModel.DataAnnotations;

namespace BackEnd_Ruleta.Models
{
    public class usuario
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        public string Nombre { get; set; }

        [Required]
        public decimal? Monto { get; set; }
    }
}
