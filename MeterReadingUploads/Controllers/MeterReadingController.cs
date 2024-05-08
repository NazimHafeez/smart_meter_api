using FluentValidation;
using MeterReadingUploads.Services;
using MeterReadingUploads.SourceDataAdaptors;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class MeterReadingUploadsController : ControllerBase
{
    private readonly IMeterReadingService _meterReadingService;
    private readonly ISourceDataAdapter<CsvFileAdapterOption, string[]> _csvAdapter;
    private readonly IValidator<IFormFile> _validator;

    public MeterReadingUploadsController(IMeterReadingService meterReadingService, ISourceDataAdapter<CsvFileAdapterOption, string[]> csvAdapter, IValidator<IFormFile> validator)
    {
        _meterReadingService = meterReadingService;
        _csvAdapter = csvAdapter;
        _validator = validator;
    }

    [HttpPost("meter-reading-uploads")]
    public async Task<IActionResult> UploadCSV(IFormFile file, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(file, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }

        try
        {
            var meterReadings = await _csvAdapter.ReadAsync(new SourceOptions<CsvFileAdapterOption>(new CsvFileAdapterOption { File = file }), cancellationToken);
            var (success, failures) = await _meterReadingService.StoreMeterReadingsAsync(meterReadings, cancellationToken);
            string failureMessage = failures > 0 ? $" Failed to process {failures} records" : string.Empty;

            return Ok($"Successfully processed {success} records.{failureMessage}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while processing the CSV file: {ex.Message}");
        }
    }

    [HttpGet("meter-readings")]
    public async Task<IActionResult> GetAllMeterReadings(CancellationToken cancellationToken)
    {
        try
        {
            var meterReadings = await _meterReadingService.GetAllMeterReadingsAsync(cancellationToken);
            if(!meterReadings.Any())
            {
                return NoContent();
            }
            return Ok(meterReadings.Select(x=> new { x.AccountID, x.MeterReadingDateTime, x.MeterReadValue}).ToList());
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving the meter readings: {ex.Message}");
        }
    }
}
