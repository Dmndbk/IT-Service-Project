using ITService.DataAccess.Context;
using ITService.Model;
using Microsoft.EntityFrameworkCore;

namespace ITService.UI.Data
{
    public interface IStatusDataProvider
    {
        Task<IEnumerable<Status>?> Get();
    }

    class StatusDataProvider : IStatusDataProvider
    {
        private ItServiceDb _context;

        public StatusDataProvider(ItServiceDb context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Status>?> Get()
        {
            return await _context.Status
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
