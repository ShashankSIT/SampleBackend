namespace SampleBackend.Model.Model
{
    public class ApplicationSettings
    {
        public string? JWT_Secret { get; set; }
        public string? EmailEnableSsl { get; set; }
        public string? EmailHostName { get; set; }
        public string? EmailAppPassword { get; set; }
        public string? EmailPort { get; set; }
        public string? EmailUsername { get; set; }
        public string? FromEmail { get; set; }
        public string? FromName { get; set; }
    }

    public class ConnectionStrings
    {
        public string? DefaultConnection { get; set; }
        public string? NavisionConnection { get; set; }
        public string? NpgSqlConnection { get; set; }
        public string? NpgSqlConnection2 { get; set; }
    }
}
