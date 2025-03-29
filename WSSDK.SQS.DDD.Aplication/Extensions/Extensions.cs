using SecretManagerExtensions;
using System.Diagnostics;

namespace AWS.Aplication.Extensions
{
    public static class Extensions
    {
        public static IConfigurationBuilder AddConfigurationAWSSecret(this IConfigurationBuilder configBuilder,
            IConfiguration configuration)
        {
            
            string region = configuration.GetSection("AmazonSettings:AWSSecretManager:Region").Value;
            if (region == null)
            {
                throw new InvalidOperationException("Region is not configured.");
            }

            string secretName = configuration.GetSection("AmazonSettings:AWSSecretManager:SecretName").Value;
            if (secretName == null)
            {
                throw new InvalidOperationException("SecretName is not configured.");
            }

            try { 
                  configBuilder.AddAWSSecretsManager(region, secretName);

            }
            catch (Exception ex)
            {
                // Log the exception (if you have a logger set up)
                Debug.WriteLine($"Error adding AWS Secrets Manager configuration: {ex.Message}");
                throw;
            }

            return configBuilder;
        }
    }
}
