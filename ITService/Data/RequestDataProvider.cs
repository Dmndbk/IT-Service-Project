using ITService.DataAccess.Context;
using ITService.Model;
using Microsoft.EntityFrameworkCore;

namespace ITService.UI.Data
{
    public interface IRequestDataProvider
    {
        Task<IEnumerable<Request>?> Get();
        Task<IEnumerable<Request>?> GetAll();
        Task<Request?> GetById(int id);
        Task<IEnumerable<Request>?> GetByDepartmentId(int departmentId);
        Task<IEnumerable<Request>?> GetByClientId(int clientId);
        void Add(Request request);
        void Remove(int id);
        void Update(Request request);
    }
    class RequestDataProvider : IRequestDataProvider
    {
        private ItServiceDb _context;

        public RequestDataProvider(ItServiceDb context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Request>?> Get()
        {
            return await _context.Requests
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Request>?> GetAll()
        {
            return await _context.Requests
                .AsNoTracking()
                .Include(r => r.RelatedEmployee)
                .Include(r => r.Assessment)
                .Include(r => r.CurrentStatus)
                .Include(r => r.RelatedAsset)
                .Include(r => r.RelatedClient)
                .Include(r => r.Service)
                .ToListAsync();
        }
        public async Task<Request?> GetById(int id)
        {
            return await _context.Requests
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<IEnumerable<Request>?> GetByDepartmentId(int departmentId)
        {
            return await _context.Requests
                .AsNoTracking()
                .Include(r => r.RelatedEmployee)
                .Include(r => r.Assessment)
                .Include(r => r.CurrentStatus)
                .Include(r => r.RelatedAsset)
                .Include(r => r.RelatedClient)
                .Include(r => r.Service)
                .Where(r => r.Service.DepartmentId == departmentId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Request>?> GetByClientId(int clientId)
        {
            return await _context.Requests
                .AsNoTracking()
                .Include(r => r.RelatedEmployee)
                .Include(r => r.CurrentStatus)
                .Include(r => r.RelatedAsset)
                .Include(r => r.RelatedClient)
                .Include(r => r.Service)
                .Where(r => r.RelatedClientId == clientId)
                .ToListAsync();
        }
        public void Add(Request request)
        {
            _context.Add(request);
            _context.SaveChanges();
            _context.Entry(request).State = EntityState.Detached;
        }

        public void Remove(int id)
        {
            var request = _context.Requests.AsNoTracking().FirstOrDefault(a => a.Id == id);
            _context.Remove(request);
            _context.SaveChanges();
            _context.Entry(request).State = EntityState.Detached;
        }
        public void Update(Request request)
        {
            _context.Update(request);
            _context.SaveChanges();
            _context.Entry(request).State = EntityState.Detached;
        }
    }
}
