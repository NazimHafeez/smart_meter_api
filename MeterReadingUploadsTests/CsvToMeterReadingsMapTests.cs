using MeterReadingApi.Models;
using MeterReadingUploads.Mappers;
using Microsoft.Extensions.Logging.Abstractions;

namespace MeterReadingUploadsTests
{
    public class CsvToMeterReadingsMapTests
    {
        private readonly StringArrayToMeterReading _mapper;

        public CsvToMeterReadingsMapTests()
        {
            _mapper = new StringArrayToMeterReading(new NullLogger<StringArrayToMeterReading>());
        }

        [Fact]
        public void Map_Should_ReturnValidMeterReading()
        {
            // Arrange
            string[] meterReading = new string[] { "12345", "2022-01-01", "12345" };

            // Act
            MeterReading result = _mapper.Map(meterReading);

            // Assert
            Assert.Equal(12345, result.AccountID);
            Assert.Equal(new DateTime(2022, 01, 01), result.MeterReadingDateTime);
            Assert.Equal("12345", result.MeterReadValue);
        }

        [Fact]
        public void Map_Should_Return_Trimmed_MeterReading_If_More_Than_5_Digit_Long()
        {
            // Arrange
            string[] meterReading = new string[] { "12345", "2022-01-01", "12345678" };

            // Act
            MeterReading result = _mapper.Map(meterReading);

            // Assert
            Assert.Equal(12345, result.AccountID);
            Assert.Equal(new DateTime(2022, 01, 01), result.MeterReadingDateTime);
            Assert.Equal("12345", result.MeterReadValue);
        }

        [Fact]
        public void Map_Should_ThrowException_When_InvalidMeterReadingValue()
        {
            // Arrange
            string[] meterReading = new string[] { "12345", "2022-01-01", "abcde" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _mapper.Map(meterReading));
        }

        [Fact]
        public void Map_Should_ThrowException_When_InvalidDateTimeFormat()
        {
            // Arrange
            string[] meterReading = new string[] { "12345", "2022-13-01", "12345" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _mapper.Map(meterReading));
        }

        [Fact]
        public void Map_Should_ThrowException_When_InvalidAccountId()
        {
            // Arrange
            string[] meterReading = new string[] { "abcde", "2022-01-01", "12345" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _mapper.Map(meterReading));
        }
    }
}
