using FluentValidation;

namespace MeterReadingUploads.Validators
{
    public class CsvFileAdapterValidator : AbstractValidator<IFormFile>
    {
        public CsvFileAdapterValidator()
        {
            RuleFor(file => file).NotNull().WithMessage("No file uploaded");
            RuleFor(file => file.Length).GreaterThan(0).WithMessage("Empty File");
            RuleFor(file => file).Must(file => file.FileName.EndsWith(".csv")).WithMessage("Invalid file format. Only CSV files are allowed");
        }
    }
}
