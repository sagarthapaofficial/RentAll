namespace RentAll.DTO.Account
{
    public class UserDto
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        
        public string? Token { get; set; } //token is nullable


    }
}
