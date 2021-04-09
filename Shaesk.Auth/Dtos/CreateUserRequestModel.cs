namespace Shaesk.Auth.Dtos
{
    public class CreateUserRequestModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsHash { get; set; }
    }
}
