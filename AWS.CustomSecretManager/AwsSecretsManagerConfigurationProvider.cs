using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace AWS.CustomSecretManager;

public class AwsSecretsManagerConfigurationProvider:ConfigurationProvider
{
    private readonly string _region;
    private readonly string _secretName;

    public AwsSecretsManagerConfigurationProvider(string region, string secretName)
    {
        _region = region;
        _secretName = secretName;
    }

    public override async void Load()
    {
        try { 
        var secret = await GetSecretName();

        if (string.IsNullOrEmpty(secret))
            throw new Exception("Secret retrieved is null or empty");

        var settingsRoot = JsonSerializer.Deserialize<SettingsRoot>(secret);

        if (settingsRoot is null)
            throw new Exception("Deserialized settings object is null.");

          Data = settingsRoot.ToDictionary();
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"JSON Deserialization error: {jsonEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading AWS secret: {ex.Message}");
            throw;
        }
    }


    private async Task<string> GetSecretName()
    {
        var request = new GetSecretValueRequest
        {
            SecretId = _secretName,
            VersionStage = "AWSCURRENT"
        };

        using (var cliente = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(_region)))
        {
            var response = await cliente.GetSecretValueAsync(request);

            if (response.SecretString != null)
            {
                return response.SecretString;
            }
            else
            {
                using var memoryStream = response.SecretBinary;
                using var reader = new StreamReader(memoryStream);
                return Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
            }
        }
    }
}
