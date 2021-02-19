namespace RestAPICore.Models
{
    public class GoogleConnectionsSettings : IGoogleConnectionsSettings
    {
        public string User { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public interface IGoogleConnectionsSettings
    {
        string User { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
    }
}