namespace TeamABootcampAplication.Models
{
    #pragma warning disable SA1600 // Elements should be documented
    using System;
    using System.Data.SqlClient;
    using System.Globalization;
    using TeamABootcampAplication.Controllers;

    public class Database
    {
        public static bool RemoveSessionID(string sessionID)
        {
            DBConnect connection = new DBConnect();

            try
            {
                string queryString = "UPDATE Users SET SessionID='' WHERE SessionID=@sessionID";

                SqlCommand command = new SqlCommand(queryString, connection.Connection);
                command.Parameters.AddWithValue("@sessionID", sessionID);
                SqlDataReader reader = command.ExecuteReader();

                command.Dispose();
                command.Parameters.Clear();

                return true;
            }
            catch (SqlException)
            {
                connection.Connection.Close();
                return false;
            }
        }

        public bool CheckIfUserExists(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            DBConnect connection = new DBConnect();
            try
            {
                string queryString = "SELECT * FROM Users";

                SqlCommand command = new SqlCommand(queryString, connection.Connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["Username"].ToString() == user.Username && reader["Email"].ToString() == user.Email)
                    {
                        command.Dispose();
                        reader.Close();
                        return true;
                    }
                }

                command.Dispose();
                command.Parameters.Clear();
                reader.Close();
                connection.Connection.Close();

                return false;
            }
            catch (SqlException)
            {
                connection.Connection.Close();
                return false;
            }
        }

        public bool CreateUser(User userToCreate)
        {
            if (userToCreate == null)
            {
                throw new ArgumentNullException(nameof(userToCreate));
            }

            DBConnect connection = new DBConnect();
            try
            {
                string queryString = "INSERT INTO Users (Username,Password,Email,Avatar) VALUES (@Username,@Password,@Email,@Avatar)";

                SqlCommand command = new SqlCommand(queryString, connection.Connection);
                command.Parameters.AddWithValue("@Username", userToCreate.Username);
                command.Parameters.AddWithValue("@Password", userToCreate.Password);
                command.Parameters.AddWithValue("@Email", userToCreate.Email);
                command.Parameters.AddWithValue("@Avatar", userToCreate.Avatar);
                SqlDataReader reader = command.ExecuteReader();

                command.Dispose();
                command.Parameters.Clear();
                connection.Connection.Close();
                return true;
            }
            catch (SqlException)
            {
                connection.Connection.Close();
                return false;
            }
        }

        public int Login(string username, string password)
        {
            DBConnect connection = new DBConnect();
            string queryString = "SELECT * FROM dbo.Users WHERE Username = @Username AND Password = @Password";

            SqlCommand command = new SqlCommand(queryString, connection.Connection);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);

            SqlDataReader reader = command.ExecuteReader();
            try
            {
                try
                {
                    reader.Read();
                }
                catch (SqlException)
                {
                    return 0;
                }

                if (reader.HasRows)
                {
                    return Convert.ToInt32(reader["Id"], CultureInfo.CurrentCulture);
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
            finally
            {
                command.Dispose();
                reader.Close();
                connection.Connection.Close();
            }
        }

        public static bool AssignSessionID(string sessionID, int userID)
        {
            DBConnect connection = new DBConnect();

            try
            {
                string queryString = "UPDATE Users SET SessionID=@sessionID WHERE ID=@userID";

                SqlCommand command = new SqlCommand(queryString, connection.Connection);
                command.Parameters.AddWithValue("@sessionID", sessionID);
                command.Parameters.AddWithValue("@userID", userID);
                SqlDataReader reader = command.ExecuteReader();

                command.Dispose();
                command.Parameters.Clear();

                return true;
            }
            catch (SqlException)
            {
                connection.Connection.Close();
                return false;
            }
        }

        public static bool CheckIfSessionIDExists(string sessionID)
        {
            DBConnect connection = new DBConnect();

            try
            {
                string queryString = "SELECT SessionID FROM Users";

                SqlCommand command = new SqlCommand(queryString, connection.Connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["SessionID"].ToString() == sessionID)
                    {
                        command.Dispose();
                        reader.Close();

                        return false;
                    }
                }

                command.Dispose();
                command.Parameters.Clear();
                reader.Close();
            }
            catch (SqlException)
            {
                connection.Connection.Close();
            }

            connection.Connection.Close();
            return true;
        }

        public bool ChangePassword(string username, string email, string newPassword)
        {
            DBConnect connection = new DBConnect();
            string queryString = "UPDATE Users SET Password = @NewPassword WHERE Username=@Username AND Email=@Email";
            try
            {
                SqlCommand command = new SqlCommand(queryString, connection.Connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@NewPassword", newPassword);

                command.ExecuteReader();
                connection.Connection.Close();
                command.Dispose();

                return true;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        public bool ValidateUserPasswordFromLoginPanel(string oldpassword, string username)
        {
            DBConnect connection = new DBConnect();
            string queryString = "SELECT * FROM dbo.Users WHERE  Password = @Password";

            SqlCommand command = new SqlCommand(queryString, connection.Connection);
            command.Parameters.AddWithValue("@Password", oldpassword);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                command.Dispose();
                return true;
            }
            else
            {
                command.Dispose();
                return false;
            }
        }

        public bool ChangePasswordFromLoginPanel(string username, string password)
        {
            DBConnect connection = new DBConnect();
            string queryString = "UPDATE Users SET Password = @NewPassword WHERE Username=@Username";
            try
            {
                SqlCommand command = new SqlCommand(queryString, connection.Connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@NewPassword", password);

                command.ExecuteReader();
                connection.Connection.Close();
                command.Dispose();
                return true;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }
    }
    #pragma warning restore SA1600 // Elements should be documented

}