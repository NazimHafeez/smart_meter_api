using MeterReadingApi.Repository;
using MeterReadingUploads.Mappers;
using MeterReadingUploads.Models;
using MeterReadingUploads.Services;
using MeterReadingUploads.SourceDataAdaptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace MeterReadingUploadsTests
{
    public class DBInitializerServiceTests
    {
        [Fact]
        public async Task Initialize_Should_AddAccounts_When_DatabaseIsEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MeterReadingContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            using var dbContext = new MeterReadingContext(options);
            var csvSourceDataMock = new Mock<ISourceDataAdapter<CsvFileAdapterOption, Account>>();
            ILogger<StringArrayToAccountsMap> accountMapLogger = Mock.Of<ILogger<StringArrayToAccountsMap>>();
            IMapper<string[], Account> stringArrayToAccountsMap = new StringArrayToAccountsMap(accountMapLogger);
            var csvFileAdapter = new CsvFileAdapter(Mock.Of<ILogger<CsvFileAdapter>>());
            DBInitializerService dbInitializerService = new DBInitializerService(dbContext, csvFileAdapter, stringArrayToAccountsMap);

            // Act
            await dbInitializerService.InitializeAsync();

            // Assert
            var accounts = await dbContext.Accounts.ToListAsync();
            Assert.NotEmpty(accounts);
        }

        [Fact]
        public async Task Initialize_Should_NotAddAccounts_When_DatabaseIsNotEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MeterReadingContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            using var dbContext = new MeterReadingContext(options);
            dbContext.Accounts.Add(new Account { AccountId = 1, FirstName = "first", LastName = "last" });
            await dbContext.SaveChangesAsync();
            
            ILogger<StringArrayToAccountsMap> accountMapLogger = Mock.Of<ILogger<StringArrayToAccountsMap>>();
            IMapper<string[], Account> stringArrayToAccountsMap = new StringArrayToAccountsMap(accountMapLogger);
            var csvFileAdapter = new CsvFileAdapter(Mock.Of<ILogger<CsvFileAdapter>>());
            var dbInitializerService = new DBInitializerService(dbContext, csvFileAdapter, stringArrayToAccountsMap);

            // Act
            await dbInitializerService.InitializeAsync();

            // Assert
            var accounts = await dbContext.Accounts.ToListAsync();
            Assert.Single(accounts);
        }
    }
}
