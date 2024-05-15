using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentAll.DAL.Domain_Classes
{
    public class Product
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [StringLength(200)]
        public string Id { get; set; }
   
    }
}
