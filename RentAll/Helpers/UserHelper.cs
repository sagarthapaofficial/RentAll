namespace RentAll.Helpers
{
    public class UserHelper
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string StreetAddress { get; set; } = "";

        public string City { get; set; } = "";

        public string Province { get; set; } = "";

        public string PostalCode { get; set; } = "";

        //This will be generated JWT
        public string? Token { get; set; } = "";
    }
}
