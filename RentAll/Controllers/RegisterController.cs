using Castle.Core.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentAll.DAL;
using RentAll.DAL.DAO;
using RentAll.DAL.Domain_Classes;
using RentAll.Helpers;
using System.Collections.Generic;
using System.Reflection.Metadata;

//for hasSalt
using System.Security.Cryptography;

namespace RentAll.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        AppDbContext _db;
        public RegisterController(AppDbContext context)
        {
            _db = context;
        }

        //will allow the User's to go to the register page.
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<UserHelper>> Index(UserHelper user)
        {
            UserDAO dao = new UserDAO(_db);
            User already = await dao.GetByEmail(user.Email);
            if (already == null)
            {
                HashSalt hashSalt = GenerateSaltedHash(64, user.Password);
                user.Password = ""; // don’t need the string anymore
                User dbUser = new User();
                dbUser.FirstName = user.FirstName;
                dbUser.LastName = user.LastName;
                dbUser.Email = user.Email;
                dbUser.Hash = hashSalt.Hash;
                dbUser.Salt = hashSalt.Salt;

                //assigning role
                Role role= new Role();
                role.Name = "Tenant";
                dbUser.Role = role;

                //Assigning address
                Address address = new Address();
                address.StreetAddress = user.StreetAddress;
                address.City=user.City;
                address.Province=user.Province;
                address.PostalCode=user.PostalCode;
                dbUser.Address = address;


                dbUser = await dao.Register(dbUser);
                if (dbUser.Id > 0)
                    user.Token = "User registered";
                else
                    user.Token = "User registration failed";
            }
            else
            {
                user.Token = "User registration failed - email already in use";
            }
            return user;
        }



        private static HashSalt GenerateSaltedHash(int size, string password)
        {
            var saltBytes = new byte[size];
            var provider = new RNGCryptoServiceProvider();
            // Fills an array of bytes with a cryptographically strong sequence of random nonzero values.
            provider.GetNonZeroBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);
            // a password, salt, and iteration count, then generates a binary key
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            HashSalt hashSalt = new HashSalt { Hash = hashPassword, Salt = salt };
            return hashSalt;
        }

        //HasSalt class with 2 properties
        private class HashSalt
        {
            public required string Hash { get; set; }
            public required string Salt { get; set; }
        }


    }
}
