using Microsoft.Extensions.Configuration;
using SecretManagerExtensions;


namespace AWS.CustomSecretManager
{
    public class AwsSecretsManagerConfigurationSource : IConfigurationSource
    {
        private readonly string _secretName;
        private readonly string _region;

        public AwsSecretsManagerConfigurationSource(string secretName, string region)
        {
            _region = region;
            _secretName = secretName;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AwsSecretsManagerConfigurationProvider(_secretName, _region);
        }
    }
}
