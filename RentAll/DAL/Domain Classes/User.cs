using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;

namespace RentAll.DAL.Domain_Classes
{

    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = "";
        [Required]
        public string LastName { get; set; } = "";

        [Required]
        public string Email { get; set; } = "";
        // we don't store an actual password
        [Required]
        public string Hash { get; set; } = "";
        [Required]
        public string Salt { get; set; } = "";

        [Required]
        [ForeignKey("RoleId")]
        public Role? Role { get; set; } //generates foreign key

        [Required]
        [ForeignKey("AddressId")]
        public Address? Address { get; set; } //generates foreign key
       
        [AllowNull]
        [StringLength(100)]
        public string PaymentMethod { get; set; }
     
    


    }
}
