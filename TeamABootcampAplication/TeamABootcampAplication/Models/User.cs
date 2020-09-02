namespace TeamABootcampAplication.Models 
{
    #pragma warning disable SA1600 // Elements should be documented
    public class User
    {
        public User(string username, string email, string avatar, string password = "")
        {
            if (avatar == default)
            {
                avatar = string.Empty;
            }

            this.Username = username;
            this.Email = email;
            this.Avatar = avatar;
            this.Password = password;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Avatar { get; set; }

        public string PasswordResetHash { get; set; } = string.Empty;

        public string SessionID { get; set; }

        public string Password { get; set; }
    }
    #pragma warning restore SA1600 // Elements should be documented
}