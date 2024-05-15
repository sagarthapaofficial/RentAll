using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentAll.DAL.Domain_Classes
{



    public class Role
    {
        public enum RoleType
        {
            TENANT,
            HOST,
            ADMIN
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public RoleType Name { get; set; }
    }
}
