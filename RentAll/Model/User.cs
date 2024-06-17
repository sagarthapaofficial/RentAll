using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RentAll.Model
{

    [Index(nameof(Email), IsUnique = true)]

    //IdentityUser is class that AspNetCore.Identity provides for user authentication/authorization
    //user inherites from the IdentityUser
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; } = "";
        [Required]
        public string LastName { get; set; } = "";

        [Required]
        public string Salt { get; set; } = "";

        [Required]
        public string StreetAddress { get; set; } = "";
        [Required]
        public string City { get; set; } = "";
        [Required]
        public string Province { get; set; } = "";
        [Required]
        public string PostalCode { get; set; } = "";

        //check if user is verified or not.
        [Required]
        public string UserStatus { get; set; } = "";

        [AllowNull]
        [StringLength(100)]
        public string PaymentMethod { get; set; } = "";
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;





    }
}
