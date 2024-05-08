using MeterReadingUploads.Models;
using System.Globalization;

namespace MeterReadingUploads.Mappers
{
    public class StringArrayToAccountsMap(ILogger<StringArrayToAccountsMap> logger) : IMapper<string[], Account>
    {
        public Account Map(string[] from)
        {
            logger.LogDebug("Mapping Account from CSV for account row {accountData}", from);

            if(int.TryParse(from[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int accountId))
            {
                return new Account
                {
                    AccountId = accountId,
                    FirstName = from[1],
                    LastName = from[2]
                };
            }

            logger.LogError("Invalid Account ID: {accountId}", from[0]);

            throw new ArgumentException("Invalid Account ID");
        }
    }
}
