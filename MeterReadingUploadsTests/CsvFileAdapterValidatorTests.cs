using FluentValidation.TestHelper;
using MeterReadingUploads.Validators;
using Microsoft.AspNetCore.Http;

namespace MeterReadingUploadsTests
{
    public class CsvFileAdapterValidatorTests
    {
        private readonly CsvFileAdapterValidator _validator;

        public CsvFileAdapterValidatorTests()
        {
            _validator = new CsvFileAdapterValidator();
        }

        [Fact]
        public void Should_HaveValidationError_When_FileIsEmpty()
        {
            // Arrange
            var file = new FormFile(null, 0, 0, "file", "file.txt");

            // Act
            var result = _validator.TestValidate(file);

            // Assert
            result.ShouldHaveValidationErrorFor(f => f.Length)
                .WithErrorMessage("Empty File");
        }

        [Fact]
        public void Should_HaveValidationError_When_FileHasInvalidFormat()
        {
            // Arrange
            var file = new FormFile(null, 0, 1, "file", "file.doc");

            // Act
            var result = _validator.Validate(file);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Invalid file format. Only CSV files are allowed", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void Should_NotHaveValidationError_When_FileIsValid()
        {
            // Arrange
            var file = new FormFile(null, 0, 1, "file", "file.csv");

            // Act
            var result = _validator.TestValidate(file);

            // Assert
            result.ShouldNotHaveValidationErrorFor(f => f);
        }
    }
}
