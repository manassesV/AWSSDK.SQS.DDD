using Amazon.SQS.Model;

namespace SqsService.Services
{
    public interface IAmazonSQSService
    {
        Task<DeleteMessageResponse> DeleteMessageAsync(string queueUrl, string receiptHandle, CancellationToken cancellationToken = default);
        Task<string> GetQueueUrl(string queueName);
        Task<IEnumerable<Message>> ReceiveMessageAsync(ReceiveMessageRequest receiveMessage);
        void Dispose();
    }
}