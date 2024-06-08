using ITService.DataAccess.Context;
using ITService.Model;
using Microsoft.EntityFrameworkCore;

namespace ITService.UI.Data
{
    public interface IEmployeeDataProvider
    {
        Task<IEnumerable<Employee>?> Get();
        Task<IEnumerable<Employee>?> GetAll();
        Task<Employee?> GetById(int id);
        Task<Employee?> GetByUserId(int userId);
        Task<bool> HasRequestsAsync(int employeeId);
        void Add(Employee employee);
        void Remove(int id);
        void Update(Employee employee);
    }
    class EmployeeDataProvider : IEmployeeDataProvider
    {
        private ItServiceDb _context;

        public EmployeeDataProvider(ItServiceDb context)
        {
            _context = context;
        }
        public async Task<bool> HasRequestsAsync(int employeeId)
        {
            return await _context.Requests.AsNoTracking()
               .AnyAsync(r => r.RelatedEmployeeId == employeeId);
        }
        public async Task<IEnumerable<Employee>?> Get()
        {
            return await _context.Employees
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>?> GetAll()
        {
            return await _context.Employees
                .AsNoTracking()
                .Include(e => e.Department)
                .ToListAsync();
        }
        public async Task<Employee?> GetById(int id)
        {
            return await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<Employee?> GetByUserId(int userId)
        {
            return await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }
        public void Add(Employee employee)
        {
            _context.Add(employee);
            _context.SaveChanges();
            _context.Entry(employee).State = EntityState.Detached;
        }

        public void Remove(int id)
        {
            var employee = _context.Employees.AsNoTracking().FirstOrDefault(a => a.Id == id);
            _context.Remove(employee);
            _context.SaveChanges();
            _context.Entry(employee).State = EntityState.Detached;
        }
        public void Update(Employee employee)
        {
            _context.Update(employee);
            _context.SaveChanges();
            _context.Entry(employee).State = EntityState.Detached;
        }
    }
}
