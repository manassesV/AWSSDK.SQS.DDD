using Amazon.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SqsService.Extensions
{
    public static class SqsExtension
    {

        public static IServiceCollection AddDefault(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonSQS>();

            return services;
        }
    }
}
