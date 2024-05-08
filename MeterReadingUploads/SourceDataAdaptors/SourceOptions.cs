namespace MeterReadingUploads.SourceDataAdaptors
{
    public class SourceOptions<OptionType> where OptionType : IOptions
    {
            public SourceOptions(OptionType options)
            {
                Options = options;
            }

            public OptionType Options { get; }
        }
}