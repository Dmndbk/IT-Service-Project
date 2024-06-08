using ITService.DataAccess.Context;
using ITService.Model;
using Microsoft.EntityFrameworkCore;

namespace ITService.UI.Data
{
    public interface IChangeDataProvider
    {
        Task<IEnumerable<Change>?> Get();
        Task<IEnumerable<Change>?> GetAll();
        Task<Change?> GetById(int id);
        void Add(Change change);
        void Remove(int id);
        void Update(Change change);
    }
    class ChangeDataProvider : IChangeDataProvider
    {
        private ItServiceDb _context;

        public ChangeDataProvider(ItServiceDb context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Change>?> Get()
        {
            return await _context.Changes
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Change>?> GetAll()
        {
            return await _context.Changes
                .AsNoTracking()
                .Include(c => c.RelatedEmployee)
                .Include(c=> c.RelatedService)
                .Include(c=> c.CurrentStatus)
                .ToListAsync();
        }
        public async Task<Change?> GetById(int id)
        {
            return await _context.Changes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public void Add(Change change)
        {
            _context.Add(change);
            _context.SaveChanges();
            _context.Entry(change).State = EntityState.Detached;
        }

        public void Remove(int id)
        {
            var change = _context.Changes.AsNoTracking().FirstOrDefault(a => a.Id == id);
            _context.Remove(change);
            _context.SaveChanges();
            _context.Entry(change).State = EntityState.Detached;
        }
        public void Update(Change change)
        {
            //var asset = _context.Assets.AsNoTracking().Single(a => a.Id == id);
            _context.Update(change);
            _context.SaveChanges();
            _context.Entry(change).State = EntityState.Detached;
        }
    }
}
