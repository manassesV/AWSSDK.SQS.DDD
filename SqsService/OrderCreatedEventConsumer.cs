using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqsService.Services;


namespace SqsService
{
    public class OrderCreatedEventConsumer: IHostedService, IDisposable
    {
        private readonly ILogger<OrderCreatedEventConsumer> _logger;
        private readonly IAmazonSQSService _amazonSQSService;

        private const string OrderCreatedEventQueueName = "order-created";


        public OrderCreatedEventConsumer(ILogger<OrderCreatedEventConsumer> logger,
            IAmazonSQSService amazonSQSService)
        {
            _logger = logger;
            _amazonSQSService = amazonSQSService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Polling Queue {queueName}", OrderCreatedEventQueueName);
            
            var queueUrl = await _amazonSQSService.GetQueueUrl(OrderCreatedEventQueueName);
           
            var receiveRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl
            };

            while (!cancellationToken.IsCancellationRequested)
            {
                var response = await _amazonSQSService.ReceiveMessageAsync(receiveRequest);

                if (response is not null)
                {
                    var messages = response.ToList();
                    
                    if (messages.Count > 0)
                    {
                        foreach (var message in messages)
                        {
                            _logger.LogInformation("Received Message from Queue {queueName} with body as : \n {body}", OrderCreatedEventQueueName, message.Body);

                            Task.Delay(2000).Wait();

                            await _amazonSQSService.DeleteMessageAsync(queueUrl, message.ReceiptHandle, cancellationToken);
                        }
                    }
                }
             

            }


        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _amazonSQSService.Dispose();
        }

      
    }
}
