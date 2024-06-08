using ITService.DataAccess.Context;
using ITService.Model;
using Microsoft.EntityFrameworkCore;

namespace ITService.UI.Data
{
    public interface IAssetDataProvider
    {
        Task<IEnumerable<Asset>?> Get();
        Task<IEnumerable<Asset>?> GetAll();
        Task<Asset?> GetById(int id);
        void Add(Asset asset);
        void Remove(int id);
        void Update(Asset asset);
    }
    
    public class AssetDataProvider : IAssetDataProvider
    {   
        private ItServiceDb _context;

        public AssetDataProvider(ItServiceDb context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Asset>?> Get()
        {
            return await _context.Assets
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<Asset>?> GetAll()
        {
            return await _context.Assets
                .AsNoTracking()
                .Include(a => a.Company)
                .Include(a=>a.AssetType)
                .ToListAsync();
        }
        public async Task<Asset?> GetById(int id)
        {
            return await _context.Assets
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public void Add(Asset asset)
        {
            _context.Add(asset);
            _context.SaveChanges();
            _context.Entry(asset).State = EntityState.Detached;
        }

        public void Remove(int id)
        {
            var asset = _context.Assets.AsNoTracking().FirstOrDefault(a => a.Id == id);
            _context.Remove(asset);
            _context.SaveChanges();
            _context.Entry(asset).State = EntityState.Detached;
        }
        public void Update(Asset asset)
        {
            //var asset = _context.Assets.AsNoTracking().Single(a => a.Id == id);
            _context.Update(asset);
            _context.SaveChanges();
            _context.Entry(asset).State = EntityState.Detached;
        }
    }
}
