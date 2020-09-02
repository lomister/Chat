namespace TeamABootcampAplication.Controllers
{
    #pragma warning disable SA1600 // Elements should be documented

    using System;
    using System.Data.SqlClient;

    public class DBConnect
    {
        private string connectionString = "Server=tcp:teamabootcampaplicationdbserver.database.windows.net,1433;Initial Catalog=TeamABootcampAplication_db;Persist Security Info=False;User ID=teama;Password=Bootcamp2020;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public DBConnect()
        {
            try
            {
                this.Connection = new SqlConnection(this.connectionString);
                this.Connection.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"You got an exception! {ex.Message}");
            }
        }

        public SqlConnection Connection { get; set; }
    }
    #pragma warning restore SA1600 // Elements should be documented
}