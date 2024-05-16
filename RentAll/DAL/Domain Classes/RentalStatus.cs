using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAll.DAL.Domain_Classes
{
    public class RentalStatus
    {


        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Status { get; set; } = "";

    }
}
