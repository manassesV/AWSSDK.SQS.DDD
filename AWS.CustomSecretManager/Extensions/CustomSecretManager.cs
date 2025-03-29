using Microsoft.Extensions.Configuration;


namespace AWS.CustomSecretManager.Extensions
{
    public static class CustomSecretManager
    {
        public static void  AddSecretsManager(this IConfigurationBuilder configurationBuilder,
            string region, 
            string secretName)
        {
            var configurationSource = new AwsSecretsManagerConfigurationSource(region, secretName);
            configurationBuilder.Add(configurationSource);
        }
    }
}
