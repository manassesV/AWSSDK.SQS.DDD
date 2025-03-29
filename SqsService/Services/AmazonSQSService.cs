using Amazon.SQS.Model;
using Amazon.SQS;
using Microsoft.Extensions.Logging;

namespace SqsService.Services;

public class AmazonSQSService : IAmazonSQSService
{
    private readonly IAmazonSQS _amazonSQS;
    private readonly ILogger<AmazonSQSService> _logger;

    public AmazonSQSService(ILogger<AmazonSQSService> logger, IAmazonSQS amazonSQS)
    {
        _logger = logger;
        _amazonSQS = amazonSQS;
    }

    public async Task<DeleteMessageResponse> DeleteMessageAsync(string queueUrl, string receiptHandle, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            var response = await _amazonSQS.DeleteMessageAsync(queueUrl, receiptHandle, cancellationToken)
             .ConfigureAwait(false);  // Avoid deadlocks on UI-thread (if applicable)

            return response;
        }
        catch (AmazonSQSException sqsEx)
        {
            // Handle specific SQS exceptions (e.g., invalid queue, permissions, etc.)
            // Log or take action on the exception
            // Log.Error($"SQS Delete Message failed: {sqsEx.Message}", sqsEx);
            throw new ApplicationException("An error occurred while deleting the message from the queue.", sqsEx);
        }
        catch (Exception ex)
        {
            // Handle general exceptions
            // Log or take action on the exception
            // Log.Error($"An unexpected error occurred: {ex.Message}", ex);
            throw new ApplicationException("An unexpected error occurred while deleting the message from the queue.", ex);
        }
    }

   
    public async Task<string> GetQueueUrl(string queueName)
    {
        try
        {
            var response = await _amazonSQS.GetQueueUrlAsync(queueName);

            return response.QueueUrl;
        }
        catch (QueueDoesNotExistException)
        {
            _logger.LogInformation("Queue {queueName} doesn't exist. Creating...", queueName);
            var responseUrl = await _amazonSQS.CreateQueueAsync(queueName);
            return responseUrl.QueueUrl;
        }
    }

    public async Task<IEnumerable<Message>> ReceiveMessageAsync(ReceiveMessageRequest receiveMessage)
    {
        try
        {
            var response = await _amazonSQS.ReceiveMessageAsync(receiveMessage);

            return response.Messages;
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Queue {queueName} doesn't exist. Creating...", ex.Message);

        }
        return Enumerable.Empty<Message>();
    }

    public void Dispose()
    {
        _amazonSQS?.Dispose();
    }

}
