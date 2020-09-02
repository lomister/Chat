namespace TeamABootcampAplication.Models.Repositorys
{
    using System;

    #pragma warning disable SA1600 // Elements should be documented

    public interface ISessionRepository
    {
        bool AssignSessionID(string sessionID, int userID);

        bool RemoveSessionID(string sessionID);

        bool CheckIfSessionIDExists(string sessionID);
    }

    public class SessionRepository : ISessionRepository
    {
        public bool AssignSessionID(string sessionID, int userID)
        {
            try
            {
                return Database.AssignSessionID(sessionID, userID);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        public bool CheckIfSessionIDExists(string sessionID)
        {
            try
            {
                return Database.CheckIfSessionIDExists(sessionID);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        public bool RemoveSessionID(string sessionID)
        {
            try
            {
                return Database.RemoveSessionID(sessionID);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }
    }
    #pragma warning restore SA1600 // Elements should be documented
}