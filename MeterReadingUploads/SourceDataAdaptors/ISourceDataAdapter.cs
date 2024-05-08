using MeterReadingApi.Models;

namespace MeterReadingUploads.SourceDataAdaptors
{
    public interface ISourceDataAdapter<OptionType, OutputType> where OptionType : IOptions
    {
        Task<IReadOnlyList<OutputType>> ReadAsync(SourceOptions<OptionType> inputOptions, CancellationToken cancellationToken);
    }
}
