using MeterReadingApi.Models;
using MeterReadingApi.Repository;
using MeterReadingUploads.Mappers;
using MeterReadingUploads.Models;
using MeterReadingUploads.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace MeterReadingUploadsTests
{
    public class MeterReadingServiceTests
    {
        [Fact]
        public async Task StoreMeterReadings_Should_ReturnSuccessfulAndFailedReadingsCount()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<MeterReadingService>>();
            var repositoryMock = new Mock<IRepository<MeterReading>>();
            var accountRepositoryMock = new Mock<IRepository<Account>>();
            var mapperMock = new StringArrayToMeterReading(Mock.Of<ILogger<StringArrayToMeterReading>>());

            var meterReadings = new List<string[]>
                {
                    new string[] { "1", "22.4.2019 09:24", "100" },
                    new string[] { "2", "22.4.2019 09:24", "200" },
                    new string[] { "3", "22.4.2019 09:24", "300" }
                };

            var accounts = new List<Account>
                {
                    new() { AccountId = 1, FirstName = "Account1", LastName = "Last1" },
                    new() { AccountId = 2, FirstName = "Account2", LastName = "Last2" },
                    new() { AccountId = 3, FirstName = "Account3", LastName = "Last3" }
                };

            repositoryMock.Setup(r => r.AddAsync(It.IsAny<MeterReading>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            accountRepositoryMock.Setup(a => a.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(accounts);

            IMeterReadingService service = new MeterReadingService(loggerMock.Object, repositoryMock.Object, accountRepositoryMock.Object, mapperMock);

            // Act
            var result = await service.StoreMeterReadingsAsync(meterReadings, CancellationToken.None);

            // Assert
            Assert.Equal(3, result.Successful);
            Assert.Equal(0, result.Failed);
        }

        [Fact]
        public async Task StoreMeterReadings_Should_IncrementFailedReadingsCount_When_MeterReadingIsNull()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<MeterReadingService>>();
            var repositoryMock = new Mock<IRepository<MeterReading>>();
            var accountRepositoryMock = new Mock<IRepository<Account>>();
            var mapperMock = new StringArrayToMeterReading(Mock.Of<ILogger<StringArrayToMeterReading>>());

            var meterReadings = new List<string[]>
                {
                    null,
                    new string[] { "2", "22.4.2019 09:24", "200" },
                    new string[] { "3", "22.4.2019 09:24", "300" }
                };

            var accounts = new List<Account>
                {
                    new() { AccountId = 1, FirstName = "Account1", LastName = "Last1" },
                    new() { AccountId = 2, FirstName = "Account2", LastName = "Last2" },
                    new() { AccountId = 3, FirstName = "Account3", LastName = "Last3" }
                };

            repositoryMock.Setup(r => r.AddAsync(It.IsAny<MeterReading>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            accountRepositoryMock.Setup(a => a.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(accounts);

            IMeterReadingService service = new MeterReadingService(loggerMock.Object, repositoryMock.Object, accountRepositoryMock.Object, mapperMock);

            // Act
            var result = await service.StoreMeterReadingsAsync(meterReadings, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Successful);
            Assert.Equal(1, result.Failed);
        }

        [Fact]
        public async Task StoreMeterReadings_Should_IncrementFailedReadingsCount_When_InvalidAccountId()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<MeterReadingService>>();
            var repositoryMock = new Mock<IRepository<MeterReading>>();
            var accountRepositoryMock = new Mock<IRepository<Account>>();
            var mapperMock = new StringArrayToMeterReading(Mock.Of<ILogger<StringArrayToMeterReading>>());

            var meterReadings = new List<string[]>
                {
                    new string[] { "1", "22.4.2019 09:24", "100" },
                    new string[] { "4", "22.4.2019 09:24", "200" },
                    new string[] { "3", "22.4.2019 09:24", "300" }
                };

            var accounts = new List<Account>
                {
                    new() { AccountId = 1, FirstName = "Account1", LastName = "Last1" },
                    new() { AccountId = 2, FirstName = "Account2", LastName = "Last2" },
                    new() { AccountId = 3, FirstName = "Account3", LastName = "Last3" }
                };

            repositoryMock.Setup(r => r.AddAsync(It.IsAny<MeterReading>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            accountRepositoryMock.Setup(a => a.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(accounts);
            
            IMeterReadingService service = new MeterReadingService(loggerMock.Object, repositoryMock.Object, accountRepositoryMock.Object, mapperMock);

            // Act
            var result = await service.StoreMeterReadingsAsync(meterReadings, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Successful);
            Assert.Equal(1, result.Failed);
        }

        [Fact]
        public async Task StoreMeterReadings_Should_IncrementFailedReadingsCount_When_ExceptionIsThrown()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<MeterReadingService>>();
            var repositoryMock = new Mock<IRepository<MeterReading>>();
            var accountRepositoryMock = new Mock<IRepository<Account>>();
            var mapperMock = new StringArrayToMeterReading(Mock.Of<ILogger<StringArrayToMeterReading>>());

            var meterReadings = new List<string[]>
                {
                    new string[] { "1", "22.4.2019 09:24", "100" },
                    new string[] { "2", "22.4.2019 09:24", "200" },
                    new string[] { "3", "22.4.2019 09:24", "300" }
                };

            var accounts = new List<Account>
                {
                    new() { AccountId = 1, FirstName = "Account1", LastName = "Last1" },
                    new() { AccountId = 2, FirstName = "Account2", LastName = "Last2" },
                    new() { AccountId = 3, FirstName = "Account3", LastName = "Last3" }
                };

            repositoryMock.Setup(r => r.AddAsync(It.IsAny<MeterReading>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            accountRepositoryMock.Setup(a => a.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(accounts);

            IMeterReadingService service = new MeterReadingService(loggerMock.Object, repositoryMock.Object, accountRepositoryMock.Object, mapperMock);

            // Act
            var result = await service.StoreMeterReadingsAsync(meterReadings, CancellationToken.None);

            // Assert
            Assert.Equal(0, result.Successful);
            Assert.Equal(3, result.Failed);
        }
    }
}
