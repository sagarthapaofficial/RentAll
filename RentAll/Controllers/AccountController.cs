using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using RentAll.DTO.Account;
using RentAll.Model;
using RentAll.Services;
using System.Security.Claims;
using System.Text;

namespace RentAll.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTService _jwtService;
        //is responsible signing the user.
        private readonly SignInManager<User> _signInManager;
        //is responsible for creawting user
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration Config;


        public EmailService EmailService { get; }

        //injecting JWTServices in the controller.
        //services needs to be added in the program.cs to do dependency injection.
        public AccountController(JWTService jwtService, SignInManager<User> signInManager, UserManager<User> userManager, EmailService emailService, IConfiguration config)
        {
            _jwtService = jwtService;
            _signInManager = signInManager;
            _userManager = userManager;
            EmailService = emailService;
            Config = config;
        }
        //it makes the end available to authorize user only
        [Authorize]
        [HttpGet("refresh-user-token")]

        //this creates new token
        public async Task<ActionResult<UserDto>> RefreshUserToken()
        {
            //finds the particular user
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            return CreateApplicationuserDto(user);
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            //finds user and return if not returns null
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null) return Unauthorized("Invalid username or password");
            if (user.EmailConfirmed == false) return Unauthorized("Please confirm your email.");

            //false to not lockout user
            //it checks the password if it matchess or not
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded) return Unauthorized("Invalid username or password");

            //this is going to call helper method and return userDto
            return CreateApplicationuserDto(user);

        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {


            //check if email exists or not
            if (await CheckEmailExistsAsync(model.Email))
            {
                return BadRequest($"An existing account is using {model.Email} email address. Please try with another email address");
            }

            //creating user object
            var userToAdd = new User
            {
                FirstName = model.FirstName.ToLower(),
                LastName = model.LastName.ToLower(),
                Email = model.Email.ToLower(),
                UserName = model.Email.ToLower(),
                StreetAddress = model.StreetAddress.ToLower(),
                City = model.City.ToLower(),
                Province = model.Province.ToLower(),
                PostalCode = model.PostalCode.ToLower(),
                PhoneNumber = model.PhoneNumber.ToLower(),
                EmailConfirmed = false,
                PaymentMethod = "",

            };

            //create user in the db if not returns null
            var result = await _userManager.CreateAsync(userToAdd, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);

            }
            try
            {
                if (await SendConfirmEmailAsync(userToAdd))
                {
                    //create a Json object and sends it back to client

                    return Ok(new JsonResult(new { title = "Account Created", message = "Your account has been created, please confirm you email address" }));
                }

                return BadRequest("Failed to send email. Please contact admin");
            }
            catch (Exception)
            {
                return BadRequest("Failed to send email. Please contact admin");
            }


        }


        [HttpPut("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto model)
        {
            //we will get the user by email
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                return Unauthorized("This email address has not been registered yet");

            }
            if (user.EmailConfirmed == true)
            {
                return BadRequest("Your email was confirmed before. Please login to your account");
            }

            try
            {
                //decodes the emaail to byte
                var decodedTokenBytes = WebEncoders.Base64UrlDecode(model.Token);
                var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

                //confirmEmailAsyncs sets the email confirmed to true meaning 1 in database.
                var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

                if (result.Succeeded)
                {
                    return Ok(new JsonResult(new { title = "Email Confirmed", message = "Your email address is confirmed. You can login now" }));
                }

                return BadRequest("Invalid token. Please try again");
            }
            catch (Exception)
            {
                return BadRequest("Invalid token. Please try again");
            }


        }

        //Resed Email Confirmation
        //provide email in attribute
        [HttpPost("resendEmailConfirmation/{email}")]
        public async Task<IActionResult> ResendEmailConfirmation(string email)
        {
            if (string.IsNullOrEmpty(email)) return BadRequest("Invalid email address");

            var user = await _userManager.FindByEmailAsync(email.ToUpper());

            if (user == null) return Unauthorized("This email address has not been registered yet.");
            if (user.EmailConfirmed == true) return BadRequest("Your email address was confirmed already. Please login to your account.");
            try
            {
                if (await SendConfirmEmailAsync(user))
                {
                    return Ok(new JsonResult(new { title = "Confirmation Link sent", message = "Please confirm your email address" }));
                }
                return BadRequest("Failed to send email. Please contact admin");
            }
            catch (Exception)
            {
                return BadRequest("Failed to send email. Please contact admin");
            }
        }

        //endpoint for handling forgot username or reset password
        //user provides email and we then provide username and password reset link
        [HttpPost("forgotUserNamePassword/{email}")]
        public async Task<IActionResult> ForgotUsernamePassword(string email)
        {
            if (string.IsNullOrEmpty(email)) return BadRequest("Invalid email");
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest("This email address has not been registered");

            if (user.EmailConfirmed == false) return BadRequest("Please confirm your email address first");

            try
            {
                if (await sendForgotUsernamePasswordEmail(user))
                {
                    return Ok(new JsonResult(new { title = "Password reset link email sent", message = "Please check your email address" }));
                }

                return BadRequest("Failed to send email. Please contact admin");
            }
            catch (Exception)
            {
                return BadRequest("Failed to send email. Please contact admin");
            }

        }

        //handles resetPassword request
        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized("This email address has not been registered yet");
            if (user.EmailConfirmed == false) return BadRequest("Please confirm your email address first");

            try
            {

                //decodes the emaail to byte
                var decodedTokenBytes = WebEncoders.Base64UrlDecode(model.Token);
                var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

                var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);


                if (result.Succeeded)
                {
                    return Ok(new JsonResult(new { title = "Password Reset Successfull", message = "Your Password has been changed." }));
                }

                return BadRequest("Invalid token. Please try again");

            }
            catch (Exception)
            {
                return BadRequest("Invalid token. Please try again");
            }
        }



        #region Private Helper Methods

        private UserDto CreateApplicationuserDto(User user)
        {

            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = _jwtService.CreateJWT(user),
            };
        }

        //Check if email exist in the user table or not
        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            //check if the user table contains any oif that email address if yes it returns true/false.
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());

        }


        private async Task<bool> SendConfirmEmailAsync(User user)
        { //generates token from the provided user
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var url = $"{Config["JWT:ClientUrl"]}/{Config["Email:ConfirmEmailPath"]}?token={token}&email={user.Email}";

            //using string interpolation to create html code that will be sent to client.
            var body = $"<p>Hello: {user.FirstName} {user.LastName}</p>" +
                "<p>Please confirm your email address by clicking on the following link. </p>" +
                $"<p><a href=\"{url}\">Click here</a></p>" +
                "<p>Thank you, </p>" +
                $"<br>{Config["Email:ApplicationName"]}";

            var emailSend = new EmailSendDto(user.Email, "Confirm your email", body);

            return await EmailService.SendEmailAsync(emailSend);
        }

        //sends an email if user forgot the username or password
        private async Task<bool> sendForgotUsernamePasswordEmail(User user)
        {
            //generates token from the provided user
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var url = $"{Config["JWT:ClientUrl"]}/{Config["Email:ResetPasswordPath"]}?token={token}&email={user.Email}";


            //using string interpolation to create html code that will be sent to client.
            var body = $"<p>Hello: {user.FirstName} {user.LastName}</p>" +
                $"<p>Username: {user.UserName} </p>" +
                $"<p> In order to reset your password, please click on the link below.</p>" +
                $"<p><a href=\"{url}\">Reset Password</a></p>" +
                "<p>Thank you, </p>" +
                $"<br>{Config["Email:ApplicationName"]}";

            var emailSend = new EmailSendDto(user.Email, "Forgot username or password", body);

            return await EmailService.SendEmailAsync(emailSend);


        }



        #endregion

    }
}
