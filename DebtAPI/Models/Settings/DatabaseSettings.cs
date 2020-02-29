namespace DebtAPI.Models.Settings
{
    public class DatabaseSettings
    {
        public string ConnectionStrings { get; set; }

        public string DatabaseName { get; set; }

        public int PageSize { get; set; }
    }
}