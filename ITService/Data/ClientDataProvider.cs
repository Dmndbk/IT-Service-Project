using ITService.DataAccess.Context;
using ITService.Model;
using Microsoft.EntityFrameworkCore;

namespace ITService.UI.Data
{
    public interface IClientDataProvider
    {
        Task<IEnumerable<Client>?> Get();
        Task<IEnumerable<Client>?> GetAll();
        Task<Client?> GetById(int id);
        Task<Client?> GetByUserId(int userId);
        void Add(Client client);
        void Remove(int id);
        void Update(Client client);
    }
    class ClientDataProvider : IClientDataProvider
    {
        private ItServiceDb _context;

        public ClientDataProvider(ItServiceDb context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Client>?> Get()
        {
            return await _context.Clients
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Client>?> GetAll()
        {
            return await _context.Clients
                .AsNoTracking()
                .Include(c => c.Company)
                .ToListAsync();
        }
        public async Task<Client?> GetById(int id)
        {
            return await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<Client?> GetByUserId(int userId)
        {
            return await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }
        public void Add(Client client)
        {
            _context.Add(client);
            _context.SaveChanges();
            _context.Entry(client).State = EntityState.Detached;
        }

        public void Remove(int id)
        {
            var client = _context.Clients.AsNoTracking().FirstOrDefault(a => a.Id == id);
            _context.Remove(client);
            _context.SaveChanges();
            _context.Entry(client).State = EntityState.Detached;
        }
        public void Update(Client client)
        {
            _context.Update(client);
            _context.SaveChanges();
            _context.Entry(client).State = EntityState.Detached;
        }
    }
}
