using MeterReadingApi.Models;

namespace MeterReadingApi.Repository
{
    public interface IRepository<TModel> where TModel : EntityBase
    {
        Task AddAsync(TModel model, CancellationToken cancellationToken);
        Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
    }
}