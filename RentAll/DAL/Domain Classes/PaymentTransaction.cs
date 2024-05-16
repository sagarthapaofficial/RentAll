using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentAll.DAL.Domain_Classes
{
    public class PaymentTransaction
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        [Column(TypeName ="money")]
        public decimal Amount {  get; set; }

        [Required]
        [StringLength(100)]
        public string PaymentMethod { get; set; } = "";

        [Required]
        [StringLength(50)]
        public string status { get; set; } = "";
    }
}
