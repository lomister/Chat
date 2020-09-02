namespace TeamABootcampAplication.Controllers
{
    #pragma warning disable SA1600 // Elements should be documented

    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using TeamABootcampAplication.Models;

    public static class UserHandler
    {
        public static List<string> ConnectedIds = new List<string>();
        public static List<string> Usernames = new List<string>();
        public static List<string> AllUsers = new List<string>();
        public static List<string> AllGroups = new List<string>();
        public static List<int> UserSeeds = new List<int>();
        public static int ToUser;
    }

    public class ChatHub : Hub
    {
        private List<string> messageHistoryMessages = new List<string>();
        private List<string> messageHistoryUsernames = new List<string>();
        private List<string> messageHistoryDateTime = new List<string>();

        public override Task OnConnectedAsync()
        {
            UserHandler.ConnectedIds.Add(this.Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            UserHandler.Usernames.RemoveAt(UserHandler.ConnectedIds.IndexOf(this.Context.ConnectionId));
            UserHandler.ConnectedIds.Remove(this.Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        public void GetAllUsers(string yourUsername)
        {
            var connection = new DBConnect();
            string queryString = "Select Username From Users";

            SqlCommand command = new SqlCommand(queryString, connection.Connection);
            SqlDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    if (reader["Username"].ToString() != yourUsername)
                    {
                        UserHandler.AllUsers.Add(reader["Username"].ToString());
                    }
                }
            }
            catch (ArgumentNullException)
            {
            }
            finally
            {
                reader.Close();
                connection.Connection.Close();
                command.Dispose();
            }
        }

        private bool SaveGroupMessageToDatabase(string username, string message, int groupID)
        {
            int userID = 0;
            var connection = new DBConnect();
            string queryString = "SELECT * FROM Users";

            SqlCommand command = new SqlCommand(queryString, connection.Connection);
            SqlDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    if (reader["Username"].ToString() == username)
                    {
                        userID = Convert.ToInt32(reader["ID"], CultureInfo.CurrentCulture);
                    }
                }

                reader.Close();

                if (userID != 0)
                {
                    queryString =
                        "INSERT INTO GroupChatMessages (SenderID,Message,Date,GroupChatID) VALUES (@SenderID,@Message,@Date,@GroupChatID)";

                    SqlCommand command2 = new SqlCommand(queryString, connection.Connection);

                    command2.Parameters.AddWithValue("@Message", message);
                    command2.Parameters.AddWithValue("@SenderID", userID);
                    command2.Parameters.AddWithValue("@Date", DateTime.Now.ToString(CultureInfo.CurrentCulture));
                    command2.Parameters.AddWithValue("@GroupChatID", groupID);

                    SqlDataReader reader2 = command2.ExecuteReader();
                    reader2.Close();
                    connection.Connection.Close();
                    command2.Dispose();

                    return true;
                }

                return false;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            finally
            {
                command.Dispose();
            }
        }

        private bool SaveToDatabase(string user, int toUser, string message)
        {
            int userID = 0;
            var connection = new DBConnect();
            string queryString = "SELECT * FROM Users";

            SqlCommand command = new SqlCommand(queryString, connection.Connection);
            SqlDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    if (reader["Username"].ToString() == user)
                    {
                        userID = Convert.ToInt32(reader["ID"], CultureInfo.CurrentCulture);
                    }
                }

                reader.Close();

                queryString =
                    "INSERT INTO PrivateChatMessages (SenderID,Message,Date,ReceiverID) VALUES (@SenderID,@Message,@Date, @ReceiverID)";
                SqlCommand command2 = new SqlCommand(queryString, connection.Connection);

                command2.Parameters.AddWithValue("@Message", message);
                command2.Parameters.AddWithValue("@SenderID", userID);
                command2.Parameters.AddWithValue("@Date", DateTime.Now.ToString(CultureInfo.CurrentCulture));
                command2.Parameters.AddWithValue("@ReceiverID", toUser);

                SqlDataReader reader2 = command2.ExecuteReader();
                reader2.Close();
                connection.Connection.Close();
                command2.Dispose();

                return true;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            finally
            {
                command.Dispose();
            }
        }

        private bool PrivateSaveToDatabase(string user, string message, string receiverUser)
        {
            int userID = 0, receiverID = 0;
            var connection = new DBConnect();

            string queryString = "SELECT * FROM Users";
            SqlCommand command = new SqlCommand(queryString, connection.Connection);

            SqlDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    if (reader["Username"].ToString() == user)
                    {
                        userID = Convert.ToInt32(reader["ID"], CultureInfo.CurrentCulture);
                    }

                    if (reader["Username"].ToString() == receiverUser)
                    {
                        receiverID = Convert.ToInt32(reader["ID"], CultureInfo.CurrentCulture);
                    }
                }

                reader.Close();

                queryString = "INSERT INTO PrivateChatMessages (SenderID,Message,Date,ReceiverID) VALUES (@SenderID,@Message,@Date,@ReceiverID)";
                SqlCommand command2 = new SqlCommand(queryString, connection.Connection);

                command2.Parameters.AddWithValue("@Message", message);
                command2.Parameters.AddWithValue("@SenderID", userID);
                command2.Parameters.AddWithValue("@Date", DateTime.Now.ToString(CultureInfo.CurrentCulture));
                command2.Parameters.AddWithValue("@ReceiverID", receiverID);

                SqlDataReader reader2 = command2.ExecuteReader();
                reader2.Close();
                connection.Connection.Close();
                command2.Dispose();

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                command.Dispose();
            }
        }

        private bool GetChatMessageHistory(int chatID, int groupID = 0)
        {
            var connection = new DBConnect();
            string queryString;
            if (chatID == 1)
            {
                queryString = "SELECT PublicChatMessages.Date,PublicChatMessages.Message,PublicChatMessages.SenderID,Users.Username FROM PublicChatMessages LEFT JOIN Users ON PublicChatMessages.SenderID = Users.ID";
            }
            else if (chatID == 2)
            {
                queryString = "SELECT PrivateChatMessages.Date,PrivateChatMessages.Message,PrivateChatMessages.SenderID,PrivateChatMessages.ReceiverID,Users.Username FROM PrivateChatMessages LEFT JOIN Users ON PrivateChatMessages.SenderID = Users.ID";
            }
            else if (chatID == 3)
            {
                queryString = "SELECT GroupChatMessages.Date,GroupChatMessages.Message,GroupChatMessages.SenderID,GroupChatMessages.ID,Users.Username FROM GroupChatMessages LEFT JOIN Users ON GroupChatMessages.SenderID = Users.ID";
            }
            else
            {
                return false;
            }

            SqlCommand command = new SqlCommand(queryString, connection.Connection);
            SqlDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    this.messageHistoryUsernames.Add(reader["Username"].ToString());
                    this.messageHistoryMessages.Add(reader["Message"].ToString());
                    this.messageHistoryDateTime.Add(reader["Date"].ToString());
                }

                return true;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            finally
            {
                reader.Close();
                connection.Connection.Close();
                command.Dispose();
            }
        }

        public void CreateNewChat(string username, string userWhoCreated)
        {
            this.CreateNewPrivateChat(username, userWhoCreated);
        }

        public void RemoveGroupChat(string groupChatName)
        {
            var connection = new DBConnect();
            string queryString = "DELETE FROM GroupChats WHERE GroupChatName = @GroupChatName";

            SqlCommand command = new SqlCommand(queryString, connection.Connection);
            command.Parameters.AddWithValue("@GroupChatName", groupChatName);
            SqlDataReader reader = command.ExecuteReader();

            reader.Close();
            connection.Connection.Close();
            command.Dispose();
        }

        public async Task GetOnlineGroupChats()
        {
            var connection = new DBConnect();
            string queryString = "SELECT * FROM GroupChats";

            SqlCommand command = new SqlCommand(queryString, connection.Connection);
            SqlDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    await this.Clients.All.SendAsync("RecieveOpenGroupChats", reader["GroupChatName"].ToString(), Convert.ToInt32(reader["ID"], CultureInfo.CurrentCulture)).ConfigureAwait(true);
                }

                reader.Close();
            }
            catch (ArgumentNullException)
            {
                reader.Close();
            }
            finally
            {
                command.Dispose();
            }
        }

        public async Task RemoveFromAllGroups()
        {
            for(int x = 0; x < UserHandler.AllGroups.Count(); x++)
            {
                await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, UserHandler.AllGroups[x]).ConfigureAwait(true);
            }
        }

        public async Task RemoveFromAllPrivates()
        {
            for (int x = 0; x < UserHandler.UserSeeds.Count(); x++)
            {
                await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, UserHandler.UserSeeds[x].ToString()).ConfigureAwait(true);
            }
        }

        public async Task GetGroupChatMessage(string groupName, int groupID, string username)
        {
            var connection = new DBConnect();
            string queryString = "SELECT GroupChatMessages.Date, GroupChatMessages.Message, GroupChatMessages.SenderID, GroupChatMessages.ID, GroupChatMessages.GroupChatID, Users.Username FROM GroupChatMessages LEFT JOIN Users ON GroupChatMessages.SenderID = Users.ID WHERE GroupChatID=@groupID";

            SqlCommand command = new SqlCommand(queryString, connection.Connection);
            command.Parameters.AddWithValue("@groupID", groupID);
            SqlDataReader reader = command.ExecuteReader();
            RemoveFromAllGroups();
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName).ConfigureAwait(true);
            try
            {
                while (reader.Read())
                {

                    await Clients.Group(groupName).SendAsync("RecieveGroupChatMessage", reader["Message"].ToString(), reader["Username"].ToString(), reader["Date"].ToString(), false);
                }

                reader.Close();
            }
            catch (ArgumentNullException)
            {
                reader.Close();
            }
            finally
            {
                command.Dispose();
            }

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName).ConfigureAwait(true);
        }

        public async Task SendMessage(string user, int toUser, string message, string privateChatID)
        {
            toUser = UserHandler.ToUser;
            await this.Clients.Caller.SendAsync("GetOnline", UserHandler.Usernames.Distinct().ToList()).ConfigureAwait(true);
            if (!this.SaveToDatabase(user, toUser, message))
            {
                return;
            }

            string date = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            await this.Clients.Group(privateChatID).SendAsync("ReceiveMessage", user, message, date, false).ConfigureAwait(true);

            //await Clients.Others.SendAsync("ReceiveMessage", user, message, date, false);
        }

        public async Task SendMessageToGroup(string user, string message, string groupName, int groupID)
        {
            await this.Clients.Caller.SendAsync("GetOnline", UserHandler.Usernames).ConfigureAwait(true);
            if (!this.SaveGroupMessageToDatabase(user, message, groupID))
            {
                return;
            }

            string date = DateTime.Now.ToString(CultureInfo.CurrentCulture);

            await this.Clients.Group(groupName).SendAsync("RecieveGroupChatMessage", message, user, date, false).ConfigureAwait(true);

        }

        public async Task GetPublicChatMessages(string user, int count = 100)
        {
            await this.Clients.Caller.SendAsync("GetOnline", UserHandler.Usernames).ConfigureAwait(true);

            this.GetChatMessageHistory(1);

            /*if (messageHistoryMessages.Count > count)
              {
                internalCount = count;
              }
            else*/

            int internalCount = this.messageHistoryMessages.Count;

            for (int i = internalCount - 100; i < internalCount; i++)
            {
                if (this.messageHistoryUsernames[i] == user)
                {
                    await this.Clients.Caller.SendAsync("ReceiveMessage", this.messageHistoryUsernames[i], this.messageHistoryMessages[i], this.messageHistoryDateTime[i], false).ConfigureAwait(true);
                }
                else
                {
                    await this.Clients.Caller.SendAsync("ReceiveMessage", this.messageHistoryUsernames[i], this.messageHistoryMessages[i], this.messageHistoryDateTime[i], true).ConfigureAwait(true);
                }
            }
        }

        public async Task GetPrivateChatMessages(string username, string groupID, int count = 100)
        {
            int internalCount = 0;
            internalCount = this.messageHistoryMessages.Count;


        }

        public async Task GetGroupChatMessages(int groupID, int count = 100)
        {
            int internalCount = 0;
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, groupID.ToString(CultureInfo.CurrentCulture)).ConfigureAwait(true);
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupID.ToString(CultureInfo.CurrentCulture)).ConfigureAwait(true);
            this.GetChatMessageHistory(3, groupID);
            if (this.messageHistoryMessages.Count > count)
            {
                internalCount = count;
            }
            else
            {
                internalCount = this.messageHistoryMessages.Count;
            }

            for (int i = 0; i < internalCount; i++)
            {
                await this.Clients.Caller.SendAsync("ReceiveMessage", this.messageHistoryUsernames[i], this.messageHistoryMessages[i]).ConfigureAwait(true);
            }
        }

        public async Task GetPrivateChatUsers(string yourUsername)
        {
            this.GetAllUsers(yourUsername);
            await this.Clients.Caller.SendAsync("Private-Chat-User-List", UserHandler.AllUsers).ConfigureAwait(true);
        }

        public void CreateNewPrivateChat(string user, string userWhoCheated)
        {
            this.PrivateSaveToDatabase(userWhoCheated, $"Congratulations {userWhoCheated}. You created a chat with me! {user}", user);
        }

        public async Task GetYourPrivateChats(string yourUsername)
        {
            int yourID = 0;
            var connection = new DBConnect();
            List<int> usersIDs = new List<int>();

            string queryGetUser = "SELECT ID From Users Where Username=@username";

            SqlCommand command = new SqlCommand(queryGetUser, connection.Connection);
            command.Parameters.AddWithValue("@username", yourUsername);
            SqlDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    yourID = (int)reader["ID"];
                }

                reader.Close();
            }
            catch (ArgumentNullException)
            {
                reader.Close();
            }
            finally
            {
                command.Dispose();
            }

            string queryString = "SELECT SenderID FROM PrivateChatMessages Where ReceiverID=@yourID";

            command = new SqlCommand(queryString, connection.Connection);
            command.Parameters.AddWithValue("@yourID", yourID);
            reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    usersIDs.Add(Convert.ToInt32(reader["SenderID"], CultureInfo.CurrentCulture));

                    UserHandler.UserSeeds.Add(Convert.ToInt32(reader["SenderID"], CultureInfo.CurrentCulture) + yourID);
                }

                reader.Close();
            }
            catch (ArgumentNullException)
            {
                reader.Close();
            }
            finally
            {
                command.Dispose();
            }

            string queryString2 = "SELECT ReceiverID FROM PrivateChatMessages Where SenderID=@yourID";

            command = new SqlCommand(queryString2, connection.Connection);
            command.Parameters.AddWithValue("@yourID", yourID);
            reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    usersIDs.Add(Convert.ToInt32(reader["ReceiverID"], CultureInfo.CurrentCulture));
                    UserHandler.UserSeeds.Add(Convert.ToInt32(reader["ReceiverID"], CultureInfo.CurrentCulture) + yourID);
                }

                reader.Close();
            }
            catch (ArgumentNullException)
            {
                reader.Close();
            }

            usersIDs = usersIDs.Distinct().ToList();
            UserHandler.UserSeeds = UserHandler.UserSeeds.Distinct().ToList();

            for (int x = 0; x < usersIDs.Count; x++)
            {
                string queryString3 = "SELECT Username FROM Users Where ID=@user";

                command = new SqlCommand(queryString3, connection.Connection);
                command.Parameters.AddWithValue("@user", usersIDs[x]);
                reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        await this.Clients.Caller.SendAsync("RecievePrivateChats", reader["Username"].ToString(), UserHandler.UserSeeds[x]).ConfigureAwait(true);
                    }

                    reader.Close();
                }
                catch (ArgumentNullException)
                {
                    reader.Close();
                }
            }

            command.Dispose();
            connection.Connection.Close();
        }

        public async Task CreateGroupChat(string groupName)
        {
            var connection = new DBConnect();
            string queryString = "INSERT INTO GroupChats (UserID,GroupName) VALUES (@UserID,@GroupName)";

            SqlCommand command = new SqlCommand(queryString, connection.Connection);

            try
            {
                command.Parameters.AddWithValue("@UserID", 0);
                command.Parameters.AddWithValue("@GroupName", groupName);

                SqlDataReader reader = command.ExecuteReader();

                reader.Close();

                command.Parameters.Clear();
                await this.GetAllGroupChats().ConfigureAwait(true);
            }
            catch (ArgumentNullException)
            {
                command.Dispose();
            }
            finally
            {
                command.Dispose();
            }
        }

        public async Task GetAllGroupChats()
        {
            var connection = new DBConnect();
            string queryString = "SELECT ID, GroupName From GroupChats";

            SqlCommand command = new SqlCommand(queryString, connection.Connection);
            SqlDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    UserHandler.AllGroups.Add(reader["GroupName"].ToString());
                    await this.Clients.Caller.SendAsync("ReceiveGroups", reader["GroupName"].ToString(), Convert.ToInt32(reader["ID"], CultureInfo.CurrentCulture)).ConfigureAwait(true);
                }

                reader.Close();
                connection.Connection.Close();
            }
            catch (ArgumentNullException)
            {
                reader.Close();
            }
            finally
            {
                command.Dispose();
            }
        }

        public async Task RecievePrivateChatsForUser()
        {
            var connection = new DBConnect();
            string queryString = "SELECT * FROM GroupChats";
            List<string> groupChatName = new List<string>();
            SqlCommand command = new SqlCommand(queryString, connection.Connection);
            SqlDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    groupChatName.Add(reader["GroupChatName"].ToString());
                }

                await this.Clients.Caller.SendAsync("RecieveOpenPrivateChats", groupChatName).ConfigureAwait(true);
                reader.Close();
            }
            catch (ArgumentNullException)
            {
                reader.Close();
            }
            finally
            {
                command.Dispose();
            }
        }

        public async Task RecievePrivateChats1vs1(string yourUsername, string otherUsername, string seed)
        {
            int yourUserID = 0;
            int otherUserID = 0;

            this.messageHistoryUsernames = new List<string>();
            this.messageHistoryMessages = new List<string>();
            this.messageHistoryDateTime = new List<string>();
            var connection = new DBConnect();
            string queryGetYourUsername = "Select ID FROM Users Where Username=@yourUsername";

            SqlCommand command = new SqlCommand(queryGetYourUsername, connection.Connection);
            command.Parameters.AddWithValue("@yourUsername", yourUsername);
            SqlDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    yourUserID = (int)reader["ID"];
                }

                reader.Close();
            }
            catch (SqlException)
            {
                reader.Close();
            }
            finally
            {
                command.Dispose();
            }

            queryGetYourUsername = "Select ID FROM Users Where Username=@otherUsername";

            command = new SqlCommand(queryGetYourUsername, connection.Connection);
            command.Parameters.AddWithValue("@otherUsername", otherUsername);
            reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    otherUserID = (int)reader["ID"];
                }

                reader.Close();
            }
            catch (SqlException)
            {
                reader.Close();
            }
            finally
            {
                command.Dispose();
            }

            UserHandler.ToUser = otherUserID;

            string queryString = "SELECT * FROM PrivateChatMessages WHERE (PrivateChatMessages.ReceiverID = @otherID AND PrivateChatMessages.SenderID = @yourID) OR (PrivateChatMessages.ReceiverID = @myID AND PrivateChatMessages.SenderID = @elseID) ORDER BY PrivateChatMessages.ID ASC;";
            List<string> groupChatName = new List<string>();

            command = new SqlCommand(queryString, connection.Connection);
            command.Parameters.AddWithValue("@yourID", yourUserID);
            command.Parameters.AddWithValue("@otherID", otherUserID);
            command.Parameters.AddWithValue("@myID", yourUserID);
            command.Parameters.AddWithValue("@elseID", otherUserID);
            reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    if (reader["SenderID"].ToString() == yourUserID.ToString(CultureInfo.CurrentCulture))
                    {
                        this.messageHistoryUsernames.Add(yourUsername);
                        this.messageHistoryMessages.Add(reader["Message"].ToString());
                        this.messageHistoryDateTime.Add(reader["Date"].ToString());
                    }
                    else
                    {
                        this.messageHistoryUsernames.Add(otherUsername);
                        this.messageHistoryMessages.Add(reader["Message"].ToString());
                        this.messageHistoryDateTime.Add(reader["Date"].ToString());
                    }
                }

                reader.Close();
            }
            catch
            {
                reader.Close();
            }
            finally
            {
                command.Dispose();
            }

            RemoveFromAllPrivates();
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, seed).ConfigureAwait(true);

            for (int i = 0; i < this.messageHistoryMessages.Count; i++)
            {
                await this.Clients.Group(seed).SendAsync("ReceiveMessage", this.messageHistoryUsernames[i], this.messageHistoryMessages[i], this.messageHistoryDateTime[i], false).ConfigureAwait(true);
            }
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, seed).ConfigureAwait(true);
        }

        public void StoreUsername(string username)
        {
            UserHandler.Usernames.Add(username);
        }
    }
    #pragma warning restore SA1600 // Elements should be documented
}