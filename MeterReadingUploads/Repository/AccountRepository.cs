using MeterReadingApi.Repository;
using MeterReadingUploads.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterReadingUploads.Repository
{
    public class AccountRepository : IRepository<Account>
    {
        private readonly MeterReadingContext _context;

        public AccountRepository(MeterReadingContext context)
        {
            _context = context;
        }

        public Task AddAsync(Account model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Account>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Accounts.ToListAsync(cancellationToken);
        }

        public Task SaveAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
