namespace AWS.CustomSecretManager;

public class SecretAppSettings
{
    public Dictionary<string, string> ConnectionString { get; set; }
    public Dictionary<string, string> Endpoints { get; set; }
    public Dictionary<string, string> App { get; set; }
    public Dictionary<string, string> AWS { get; set; }
    public Dictionary<string, string> EmailConfig { get; set; }
}
