using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAll.DAL.Domain_Classes
{
    public class ProductImage
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [StringLength(100)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ProductId")]
        public required Product Product { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; } = "";
    }
}
