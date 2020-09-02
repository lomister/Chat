namespace TeamABootcampAplication.Models.Services
{
    #pragma warning disable SA1600 // Elements should be documented
    using System;
    using System.Globalization;
    using TeamABootcampAplication.Models.Repositorys;

    public class SessionService : ISessionService
    {
        private readonly Random random = new Random();
        private ISessionRepository repository;

        public SessionService(ISessionRepository repository)
        {
            this.repository = repository;
        }

        public string GenerateSessionID(int userID)
        {
            if (!ValidateUserID(userID))
            {
                return string.Empty;
            }
            else
            {
                string sessionID;

                do
                {
                    sessionID = this.random.Next(10000000, 99999999).ToString(CultureInfo.CurrentCulture);
                }
                while (!this.repository.CheckIfSessionIDExists(sessionID));

                this.repository.AssignSessionID(sessionID, userID);

                return sessionID;
            }
        }

        public bool RemoveSessionID(string sessionID)
        {
            if (!ValidateSessionID(sessionID))
            {
                return false;
            }
            else
            {
                if (this.repository.RemoveSessionID(sessionID))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        protected static bool ValidateUserID(int userID)
        {
            if (userID > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected static bool ValidateSessionID(string sessionID)
        {
            if (sessionID != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public interface ISessionService
    {
        string GenerateSessionID(int userID);

        bool RemoveSessionID(string sessionID);
    }

    #pragma warning restore SA1600 // Elements should be documented
}