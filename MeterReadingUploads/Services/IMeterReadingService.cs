using MeterReadingApi.Models;

namespace MeterReadingUploads.Services
{
    public interface IMeterReadingService
    {
        // Stores meter readings in the database
        // Returns the number of successful and failed readings
        Task<(int Successful, int Failed)> StoreMeterReadingsAsync(IEnumerable<string[]> meterReadings, CancellationToken cancellationToken);

        Task<IEnumerable<MeterReading>> GetAllMeterReadingsAsync(CancellationToken cancellationToken);
    }
}