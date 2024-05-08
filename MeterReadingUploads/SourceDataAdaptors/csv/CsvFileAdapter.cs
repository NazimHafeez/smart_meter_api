namespace MeterReadingUploads.SourceDataAdaptors
{
    public class CsvFileAdapter(ILogger<CsvFileAdapter> logger) : ISourceDataAdapter<CsvFileAdapterOption, string[]>
    {
        public async Task<IReadOnlyList<string[]>> ReadAsync(SourceOptions<CsvFileAdapterOption> inputOptions, CancellationToken cancellationToken)
        {
            IList<string[]> rows = [];

            try
            {
                using (var reader = new StreamReader(inputOptions.Options.File.OpenReadStream()))
                {
                    if (inputOptions.Options.HasHeaderRecord)
                    {
                        logger.LogDebug("Skipping the header record in the CSV file");
                        // We need to skip the header record
                        await reader.ReadLineAsync(cancellationToken);
                    }

                    // Read the CSV file and process the data
                    while (!reader.EndOfStream)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var line = await reader.ReadLineAsync(cancellationToken);
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            logger.LogInformation("Skipping empty line in the CSV file");
                            continue;
                        }
                        logger.LogDebug("Processing line: {line}", line);
                        string[] values = line.Split(inputOptions.Options.Delimiter);
                        rows.Add(values);
                    }
                }
                logger.LogDebug("Processed Csv file for {count} meter readings", rows.Count);
                return rows.AsReadOnly();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the CSV file");
                throw;
            }
        }
    }
}
