using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentAll.Model
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string StreetAddress { get; set; } = "";
        [Required]
        public string City { get; set; } = "";
        [Required]
        public string Province { get; set; } = "";
        // we don't store an actual password
        [Required]
        public string PostalCode { get; set; } = "";

    }
}
