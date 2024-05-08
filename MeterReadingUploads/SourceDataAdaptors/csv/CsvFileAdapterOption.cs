namespace MeterReadingUploads.SourceDataAdaptors
{
    // This class is used to provide options for the CSV file adapter
    public class CsvFileAdapterOption: IOptions
    {
        // CSV file send via the POST endpoint
        public required IFormFile File { get; set; }

        // This can be used to identify whether the first line of the CSV file is a header or not
        public bool HasHeaderRecord { get; set; } = true;

        // This can be used to identify the delimiter used in the CSV file
        public char Delimiter { get; set; } = ',';
    }
}
