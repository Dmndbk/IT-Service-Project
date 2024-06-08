using ITService.DataAccess.Context;
using ITService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ITService.UI.Data
{
    public interface IInfluenceLevelDataProvider
    {
        Task<IEnumerable<InfluenceLevel>?> Get();
    }

    class InfluenceLevelDataProvider : IInfluenceLevelDataProvider
    {
        private ItServiceDb _context;

        public InfluenceLevelDataProvider(ItServiceDb context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InfluenceLevel>?> Get()
        {
            return await _context.InfluenceLevels
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
