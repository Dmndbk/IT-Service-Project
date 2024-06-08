using ITService.DataAccess.Context;
using ITService.Model;
using Microsoft.EntityFrameworkCore;

namespace ITService.UI.Data
{
    public interface IAssetTypeDataProvider
    {
        Task<IEnumerable<AssetType>?> Get();
    }
    public class AssetTypeDataProvider : IAssetTypeDataProvider
    {
        private ItServiceDb _context;

        public AssetTypeDataProvider(ItServiceDb context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AssetType>?> Get()
        {
            return await _context.AssetTypes
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
