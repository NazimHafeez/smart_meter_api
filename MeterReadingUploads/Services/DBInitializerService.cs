using MeterReadingApi.Repository;
using MeterReadingUploads.Mappers;
using MeterReadingUploads.Models;
using MeterReadingUploads.SourceDataAdaptors;

namespace MeterReadingUploads.Services
{
    public class DBInitializerService(MeterReadingContext dbContext, ISourceDataAdapter<CsvFileAdapterOption, string[]> csvSourceData, IMapper<string[], Account> mapper) : IDBInitializer
    {

        public async Task InitializeAsync()
        {
            dbContext.Database.EnsureCreated();

            if (dbContext.Accounts.Any())
            {
                return;
            }

            IFormFile formFile;
            IReadOnlyList<string[]> accountRows;
            using (var fileStream = File.OpenRead(@"Data/Test_Accounts 2.csv"))
            {
               formFile = new FormFile(fileStream, 0, fileStream.Length, "name", "Accounts");
                // Read the CSV file and add the data to the database
                accountRows = await csvSourceData.ReadAsync(new SourceOptions<CsvFileAdapterOption>(new CsvFileAdapterOption() { File = formFile }), CancellationToken.None);
            };

            var mappedAccounts = accountRows.Select(mapper.Map);
            await dbContext.Accounts.AddRangeAsync(mappedAccounts);
            await dbContext.SaveChangesAsync();
        }
    }
}
