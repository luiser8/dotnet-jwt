namespace DotnetJWT.Request.User.Payloads
{
    public class LoginPayload
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}