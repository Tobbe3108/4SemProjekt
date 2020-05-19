namespace ToolBox.Contracts.Auth
{
    public interface AuthenticateUser
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}