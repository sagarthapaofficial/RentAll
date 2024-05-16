using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RentAll.DAL.Domain_Classes
{
    public class Order
    {

        public Order()
        {
            OrderItems = new HashSet<OrderItems>();
        }


        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        public String PaymentStatus { get; set; } = "";

        public virtual ICollection<OrderItems> OrderItems { get; set; }
    }
}
