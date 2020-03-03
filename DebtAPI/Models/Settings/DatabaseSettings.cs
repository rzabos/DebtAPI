namespace DebtAPI.Models.Settings
{
    public class DatabaseSettings
    {
        public string AuthenticationDatabase { get; set; }

        public string ConnectionStrings { get; set; }

        public string HistoryDatabase { get; set; }

        public int PageSize { get; set; }
    }
}