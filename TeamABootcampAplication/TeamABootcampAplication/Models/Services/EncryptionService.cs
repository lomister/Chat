using System;

namespace TeamABootcampAplication.Models.Services
{
    #pragma warning disable SA1600 // Elements should be documented
    public class EncryptionService : IEncryptionService
    {
        public string Encrypt(string pass)
        {
            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(pass);
                return System.Convert.ToBase64String(plainTextBytes);
            }
            catch (ArgumentNullException ex)
            {
                return ex.Message;
            }
        }

        public string Decrypt(string pass)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(pass);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (ArgumentNullException ex)
            {
                return ex.Message;
            }
        }
    }

    public interface IEncryptionService
    {
        public string Encrypt(string pass);

        public string Decrypt(string pass);
    }
    #pragma warning restore SA1600 // Elements should be documented
}
