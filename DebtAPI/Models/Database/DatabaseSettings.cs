namespace DebtAPI.Models.Database
{
    public class DatabaseSettings
    {
        public string ConnectionStrings { get; set; }

        public string DatabaseName { get; set; }

        public int PageSize { get; set; }
    }
}