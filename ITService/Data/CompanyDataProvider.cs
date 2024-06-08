using ITService.DataAccess.Context;
using ITService.Model;
using Microsoft.EntityFrameworkCore;

namespace ITService.UI.Data
{
    public interface ICompanyDataProvider
    {
        Task<IEnumerable<Company>?> Get();
    }

    class CompanyDataProvider : ICompanyDataProvider
    {
        private ItServiceDb _context;

        public CompanyDataProvider(ItServiceDb context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Company>?> Get()
        {
            return await _context.Companies
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
