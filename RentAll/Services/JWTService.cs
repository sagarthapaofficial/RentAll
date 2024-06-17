using Microsoft.IdentityModel.Tokens;
using RentAll.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RentAll.Services
{
    //Service that creates JWT Token
    public class JWTService
    {
        //for services we use IConfiguration class

        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _jwtKey;

        //Injected Iconfiguration 
        public JWTService(IConfiguration config)
        {
            _config = config;
            //converts string to binary key
            //takes key from AppSettings.json
            //JWT key is used for both encrypting and decrypting the token.
            _jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
        }

   
        //function that creates JWT Token
        public string CreateJWT(User user)
        {
            //creting list of claims
            var userClaims = new List<Claim>
            {
               new Claim(ClaimTypes.NameIdentifier, user.Id),
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.GivenName, user.FirstName),
               new Claim(ClaimTypes.Surname, user.LastName),
               new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
               new Claim(ClaimTypes.StreetAddress, user.StreetAddress),
               new Claim(ClaimTypes.Locality, user.City),
               new Claim(ClaimTypes.StateOrProvince, user.Province),
               new Claim(ClaimTypes.PostalCode, user.PostalCode),

               //we can put our own key and value
              // new Claim("my own claim name", "this is the value")

            };
            //SigningCredential comes from using System.IdentityModel.Tokens.Jwt;
            var credentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha256);
            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(int.Parse(_config["JWT:ExpiresInDays"])),
                SigningCredentials = credentials,
                Issuer = _config["JWT:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateToken(tokenDescripter);
            return tokenHandler.WriteToken(jwt);


        }
    }
}
