using ITService.DataAccess.Context;
using ITService.Model;
using Microsoft.EntityFrameworkCore;

namespace ITService.UI.Data
{
    public interface IServiceDataProvider
    {
        Task<IEnumerable<Service>?> Get();
        void Add(Service service);
        void Remove(int id);
        void Update(Service service);
    }
    class ServiceDataProvider : IServiceDataProvider
    {
        private ItServiceDb _context;

        public ServiceDataProvider(ItServiceDb context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Service>?> Get()
        {
            return await _context.Services
                .AsNoTracking()
                .Include(s => s.Department)
                .ToListAsync();
        }
        public void Add(Service service)
        {
            _context.Add(service);
            _context.SaveChanges();
            _context.Entry(service).State = EntityState.Detached;
        }

        public void Remove(int id)
        {
            var service = _context.Services.AsNoTracking().FirstOrDefault(a => a.Id == id);
            _context.Remove(service);
            _context.SaveChanges();
            _context.Entry(service).State = EntityState.Detached;
        }
        public void Update(Service service)
        {
            _context.Update(service);
            _context.SaveChanges();
            _context.Entry(service).State = EntityState.Detached;
        }
    }
}
