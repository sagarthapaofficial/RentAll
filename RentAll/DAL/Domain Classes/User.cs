using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace RentAll.DAL.Domain_Classes
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        // we don't store an actual password
        [Required]
        public string Hash { get; set; }
        [Required]
        public string Salt { get; set; }

        [ForeignKey("RoleId")]
        public Role? Role { get; set; } //generates foreign key

        [Required]
        public int RoleId { get; set; }

        [ForeignKey("AddressId")]
        public Address? Address { get; set; } //generates foreign key
        [Required]
        public int AddressId { get; set; }


    }
}
