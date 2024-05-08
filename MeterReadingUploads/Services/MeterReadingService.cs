using MeterReadingApi.Models;
using MeterReadingApi.Repository;
using MeterReadingUploads.Mappers;
using MeterReadingUploads.Models;

namespace MeterReadingUploads.Services
{
    // Service class for storing meter readings in the database and enforcing business constraints
    public sealed class MeterReadingService(ILogger<MeterReadingService> logger, IRepository<MeterReading> repository, IRepository<Account> accountRepository, IMapper<string[], MeterReading> mapper) : IMeterReadingService
    {
        public async Task<IEnumerable<MeterReading>> GetAllMeterReadingsAsync(CancellationToken cancellationToken)
        {
            return await repository.GetAllAsync(cancellationToken);
        }

        async Task<(int Successful, int Failed)> IMeterReadingService.StoreMeterReadingsAsync(IEnumerable<string[]> meterReadings, CancellationToken cancellationToken)
        {
            // TODO: Need to add the business constraints here in the service class
            var accountIds = (await accountRepository.GetAllAsync(cancellationToken)).Select(account => account.AccountId).ToList();
            var successfulReadings = 0;
            var failedReadings = 0;
            foreach (var reading in meterReadings)
            {
                try
                {
                    // Validate the meter reading is not Empty/Null
                    if (reading == null)
                    {
                        logger.LogError("Meter reading is null");
                        failedReadings++;
                        continue;
                    }

                    var meterReading = mapper.Map(reading);

                    // Validate the account id provided in the meter reading exists
                    if (!accountIds.Contains(meterReading.AccountID))
                    {
                        logger.LogError($"Invalid Account Id for meter read. Account with ID {meterReading.AccountID} does not exist");
                        failedReadings++;
                        continue;
                    }

                    // Remove the account id from the list of account ids to avoid duplicate readings
                    accountIds.Remove(meterReading.AccountID);

                    // Store the meter reading in the database
                    await repository.AddAsync(meterReading, cancellationToken);
                    logger.LogDebug($"Stored meter reading with ID {meterReading.AccountID}");
                    successfulReadings++;
                }
                catch (Exception ex)
                {
                    // This also catches the exceptions thrown by the mapper
                    logger.LogError(ex, $"Failed to store meter reading with Account ID {reading[0]}");
                    failedReadings++;
                }
                finally
                {
                    // Save the changes to the database
                    await repository.SaveAsync(cancellationToken);
                }
            }

            return (successfulReadings, failedReadings);
        }
    }
}
