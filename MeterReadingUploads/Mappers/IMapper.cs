using MeterReadingApi.Models;

namespace MeterReadingUploads.Mappers
{
    public interface IMapper<FROM, TO> where TO: EntityBase
    {
        TO Map(FROM from);
    }
}