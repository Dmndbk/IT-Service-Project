using ITService.DataAccess.Context;
using ITService.Model;
using Microsoft.EntityFrameworkCore;

namespace ITService.UI.Data
{
    public interface IUserDataProvider
    {
        Task<IEnumerable<User>?> GetAll();
        Task<User?> GetById(int id);
        void Add(User user);
        void Remove(int id);
        void Update(User user);
    }
    class UserDataProvider : IUserDataProvider
    {
        private ItServiceDb _context;

        public UserDataProvider(ItServiceDb context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>?> GetAll()
        {
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.UserRole)
                .ToListAsync();
        }

        public async Task<User?> GetById(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public void Add(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
            _context.Entry(user).State = EntityState.Detached;
        }

        public void Remove(int id)
        {
            var client = _context.Clients.AsNoTracking().FirstOrDefault(a => a.Id == id);
            _context.Remove(client);
            _context.SaveChanges();
            _context.Entry(client).State = EntityState.Detached;
        }
        public void Update(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
            _context.Entry(user).State = EntityState.Detached;
        }
    }
}
