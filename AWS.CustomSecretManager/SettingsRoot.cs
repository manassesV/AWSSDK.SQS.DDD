namespace AWS.CustomSecretManager
{
    public class SettingsRoot
    {
        public required SecretAppSettings Settings { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            var secretDictionary = new Dictionary<string, string>();

            if(Settings.ConnectionString is not null)
            {
                foreach (var conn in Settings.ConnectionString)
                {
                    secretDictionary[$"ConnectionStrings:{conn.Key}"] = conn.Value;
                }
            }

            if(Settings.Endpoints is not null)
            {
                foreach (var endpoint in Settings.Endpoints)
                {
                    secretDictionary[$"Endpoints:{endpoint.Key}"] = endpoint.Value;
                }

            }

            if (Settings.App is not null)
            {
                foreach (var app in Settings.App)
                {
                    secretDictionary[$"App:{app.Key}"] = app.Value;
                }
            }

            if(Settings.AWS is not null)
            {
                foreach(var aws in Settings.AWS)
                {
                    secretDictionary[$"AWS:{aws.Key}"] = aws.Value;
                }
            }

            if(Settings.EmailConfig is not null)
            {
                foreach (var timeout in Settings.EmailConfig)
                {
                    secretDictionary[$"EmailConfig:{timeout.Key}"] = timeout.Value;
                }
            }

            return secretDictionary;
        }
    }
}
