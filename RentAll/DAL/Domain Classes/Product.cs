using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentAll.DAL.Domain_Classes
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required]
        [ForeignKey("BrandId")]
        public  Brand? Brand { get; set; }

        [Required]
        [ForeignKey("ProductCategoryId")]
        public  ProductCategory? ProductCategory { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Required]
        public int QtyOnHand { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; } = "";

   
    }
}
