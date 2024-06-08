using ITService.DataAccess.Context;
using ITService.Model;
using Microsoft.EntityFrameworkCore;

namespace ITService.UI.Data
{
    public interface IDepartmentDataProvider
    {
        Task<IEnumerable<Department>?> Get(); 
        void Add(Department department);
        void Remove(int id);
        void Update(Department department);
    }

    class DepartmentDataProvider : IDepartmentDataProvider
    {
        private ItServiceDb _context;

        public DepartmentDataProvider(ItServiceDb context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Department>?> Get()
        {
            return await _context.Departments
                .AsNoTracking()
                .ToListAsync();
        }
        public void Add(Department department)
        {
            _context.Add(department);
            _context.SaveChanges();
            _context.Entry(department).State = EntityState.Detached;
        }

        public void Remove(int id)
        {
            var department = _context.Departments.AsNoTracking().FirstOrDefault(a => a.Id == id);
            _context.Remove(department);
            _context.SaveChanges();
            _context.Entry(department).State = EntityState.Detached;
        }
        public void Update(Department department)
        {
            _context.Update(department);
            _context.SaveChanges();
            _context.Entry(department).State = EntityState.Detached;
        }
    }
}
