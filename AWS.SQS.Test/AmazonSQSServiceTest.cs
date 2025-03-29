using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using Moq;
using SqsService.Services;

namespace AWS.SQS.Test
{
    [TestClass]
    public sealed class AmazonSQSServiceTest
    {
        private readonly Mock<IAmazonSQS> _mockSQSClient;
        private readonly Mock<ILogger<AmazonSQSService>> _logger;
        private readonly AmazonSQSService amazonSQSService;


        public AmazonSQSServiceTest()
        {
            _mockSQSClient = new Mock<IAmazonSQS>();
            _logger = new Mock<ILogger<AmazonSQSService>>();
            amazonSQSService = new AmazonSQSService(_logger.Object, _mockSQSClient.Object);

        }
        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public async Task DeleteMessageAsync_ShouldReturnResponse_WhenSuccessful()
        {
            // Arrange
            var queueUrl = "https://sqs.us-east-1.amazonaws.com/123456789012/MyQueue";
            var receiptHandle = "1234abcd-56ef-78gh-90ij-klmnopqrst";

            var deleteResponse = new DeleteMessageResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
            };

            _mockSQSClient.Setup(x => x.DeleteMessageAsync(queueUrl, receiptHandle, CancellationToken.None))
                .ReturnsAsync(deleteResponse);

            //Act
            var result = await amazonSQSService.DeleteMessageAsync(queueUrl, receiptHandle, CancellationToken.None);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, result.HttpStatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public async Task DeleteMessageAsync_ShouldThrowException_WhenFailed()
        {

            // Arrange
            var queueUrl = "https://sqs.us-east-1.amazonaws.com/123456789012/MyQueue";
            var receiptHandle = "1234abcd-56ef-78gh-90ij-klmnopqrst";

            _mockSQSClient.Setup(x => x.DeleteMessageAsync(queueUrl, receiptHandle, CancellationToken.None))
              .ThrowsAsync(new ApplicationException("An error occurred while deleting the message from the queue."));
           
            //Act
           await amazonSQSService.DeleteMessageAsync(queueUrl, receiptHandle, CancellationToken.None);
        }
    }
}
