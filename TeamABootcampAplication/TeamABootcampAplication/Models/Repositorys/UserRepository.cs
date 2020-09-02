namespace TeamABootcampAplication.Models.Repositorys
{
    using System;

    #pragma warning disable SA1600 // Elements should be documented
    public class UserRepository : IUserRepository
    {
        private Database db = new Database();

        public bool ChangePasswordFromUserPanelValidation(string oldpassword, string username)
        {
            if (this.db.ValidateUserPasswordFromLoginPanel(oldpassword, username))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ChangePassword(string username, string email, string password)
        {
            try
            {
                return db.ChangePassword(username, email, password);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        public bool CheckIfUserExists(User userToCreate)
        {
            try
            {
                return db.CheckIfUserExists(userToCreate);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        public bool CreateUser(User userToCreate)
        {
            try
            {
                return db.CreateUser(userToCreate);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        public int Login(string username, string password)
        {
            try
            {
                return db.Login(username, password);
            }
            catch (ArgumentNullException)
            {
                return 0;
            }
        }

        public bool ChangePasswordFromLoginPanel(string username, string newpassword)
        {
            if (this.db.ChangePasswordFromLoginPanel(username, newpassword))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public interface IUserRepository
    {
        bool CreateUser(User userToCreate);

        int Login(string username, string password);

        bool CheckIfUserExists(User userToCreate);

        bool ChangePassword(string username, string email, string password);

        bool ChangePasswordFromUserPanelValidation(string oldpassword, string username);

        bool ChangePasswordFromLoginPanel(string username, string newpassword);
    }
    #pragma warning restore SA1600 // Elements should be documented
}
