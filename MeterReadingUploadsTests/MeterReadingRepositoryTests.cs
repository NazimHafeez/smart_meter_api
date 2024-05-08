using MeterReadingApi.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace MeterReadingApi.Repository.Tests
{
    public class MeterReadingRepositoryTests
    {
        private MeterReadingRepository _repository;

        public MeterReadingRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<MeterReadingContext>().UseInMemoryDatabase("TestDb").Options;
            
            var context = new MeterReadingContext(options);

            _repository = new MeterReadingRepository(context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddMeterReadingToContext()
        {
            // Arrange
            var meterReading = new MeterReading { AccountID = 1, MeterReadingDateTime = DateTime.UtcNow, MeterReadValue = "00001" };

            // Act
            await _repository.AddAsync(meterReading, CancellationToken.None);
            await _repository.SaveAsync(CancellationToken.None);

            var meterReadings = await _repository.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.Contains(meterReading, meterReadings);
        }

    }
}
