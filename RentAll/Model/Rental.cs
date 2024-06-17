using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentAll.Model
{
    public class Rental
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [StringLength(100)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [Required]
        [ForeignKey("userID")]
        public User? User { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }


        [Required]
        [ForeignKey("RentalStatusId")]
        public RentalStatus? RentalStatus { get; set; }

    }
}
