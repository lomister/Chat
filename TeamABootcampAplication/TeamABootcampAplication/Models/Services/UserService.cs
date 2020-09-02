namespace TeamABootcampAplication.Models 
{
    using System;
    using System.Linq;
    using TeamABootcampAplication.Models.Repositorys;

#pragma warning disable SA1600 // Elements should be documented
    public class UserService : IUserService
    {
        private IUserRepository repository;

        public UserService(IUserRepository repository)
        {
            this.repository = repository;
        }

        public bool CreateUser(User userToCreate)
        {
            if (!this.ValidateCreateUser(userToCreate))
            {
                return false;
            }

            try
            {
                return this.repository.CreateUser(userToCreate);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        public string ChangePasswordFromUserPanel(string username, string newpassword, string oldpassword)
        {
            if (!this.repository.ChangePasswordFromUserPanelValidation(oldpassword, username))
            {
                return "Old password is not valid";
            }

            try
            {
                if (this.repository.ChangePasswordFromLoginPanel(username, newpassword))
                {
                    return newpassword;
                }
                else
                {
                    return null;
                }
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }

        public string ChangePassword(string username, string email)
        {
            if (this.ValidateChangePassword(username, email))
            {
                return string.Empty;
            }

            try
            {
                Random random = new Random();

                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                string password = new string(Enumerable.Repeat(chars, 10)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                if (this.repository.ChangePassword(username, email, password))
                {
                    return password;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (ArgumentNullException)
            {
                return string.Empty;
            }
        }

        public int Login(string username, string password)
        {
            if (!ValidateLogin(username, password))
            {
                return 0;
            }

            try
            {
                int id = this.repository.Login(username, password);
                if (id != 0)
                {
                    return id;
                }
                else
                {
                    return 0;
                }
            }
            catch (ArgumentNullException)
            {
                return 0;
            }
        }

        protected static bool ValidateLogin(string username, string password)
        {
            if (username == null && password == null)
            {
                return false;
            }

            return true;
        }

        protected bool ValidateCreateUser(User userToValidate)
        {
            if (userToValidate != null)
            {
                if (this.repository.CheckIfUserExists(userToValidate))
                {
                    return true;
                }
            }

            return false;
        }

        protected bool ValidateChangePassword(string username, string email)
        {
            if (username == null && email == null)
            {
                if (!this.repository.CheckIfUserExists(new User(username, email, string.Empty, string.Empty)))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public interface IUserService
    {
        bool CreateUser(User userToCreate);

        int Login(string username, string password);

        string ChangePassword(string username, string email);

        string ChangePasswordFromUserPanel(string username, string confirmPassword, string currentPassword);
    }
    #pragma warning restore SA1600 // Elements should be documented
}