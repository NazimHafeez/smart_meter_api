using MeterReadingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterReadingApi.Repository
{

    public class MeterReadingRepository : IRepository<MeterReading>
    {
        private readonly MeterReadingContext dbContext;

        public MeterReadingRepository(MeterReadingContext context)
        {
            dbContext = context;
        }

        public async Task AddAsync(MeterReading model, CancellationToken cancellationToken)
        {
            await dbContext.MeterReadings.AddAsync(model, cancellationToken);
        }

        public async Task<IEnumerable<MeterReading>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await dbContext.MeterReadings.ToListAsync(cancellationToken);

        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}