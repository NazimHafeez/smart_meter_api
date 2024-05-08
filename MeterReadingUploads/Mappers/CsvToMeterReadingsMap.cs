using MeterReadingApi.Models;
using System.Text.RegularExpressions;

namespace MeterReadingUploads.Mappers
{
    public class StringArrayToMeterReading(ILogger<StringArrayToMeterReading> logger) : IMapper<string[], MeterReading>
    {
        public MeterReading Map(string[] meterReading)
        {
            return new MeterReading
            {
                AccountID = GetAccountId(meterReading),
                MeterReadingDateTime = GetMeterReadingDateTime(meterReading),
                MeterReadValue = GetMeterReadValue(meterReading)
            };
        }

        private string GetMeterReadValue(string[] meterReading)
        {
            string meterReadingValue = meterReading[2];
            // Meter reading value should be a 5 digit number (NNNNN)
            bool isDigitsOnly = Regex.IsMatch(meterReadingValue, @"^\d+$");

            if (isDigitsOnly)
            {
                // Pad the meter reading value with leading zeros if it is less than 5 digits and then take the first 5 digits trimming any additional digits
                return meterReadingValue.PadLeft(5, '0').Substring(0, 5);
            }

            logger.LogError("Invalid Meter Reading Value: {meterReadingValue}", meterReadingValue);
            throw new ArgumentException("Invalid Meter Reading Value");
        }

        private DateTime GetMeterReadingDateTime(string[] meterReading)
        {
            if (DateTime.TryParse(meterReading[1], out DateTime meterReadingDateTime))
            {
                return meterReadingDateTime;
            }

            logger.LogError("Invalid DateTime format : {dateTime}", meterReading[1]);
            throw new ArgumentException("Invalid Meter Reading date time");
        }

        private int GetAccountId(string[] meterReading)
        {
            if(int.TryParse(meterReading[0], out int accountId))
            {
                return accountId;
            }

            logger.LogError("Invalid Account ID: {accountId}", meterReading[0]);
            throw new ArgumentException("Invalid Account ID");
        }
    }
}
