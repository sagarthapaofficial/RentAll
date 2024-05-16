using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentAll.DAL.Domain_Classes
{
    public class OrderItems
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }

        [Required] 
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [Required]
        public int QtyOrdered { get; set; }

        [Required]
        public int QtySold { get; set; }

        [Required]
        [Column(TypeName ="money")]
        public decimal Amount { get; set; } 

    }
}
