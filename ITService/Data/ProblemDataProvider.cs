using ITService.DataAccess.Context;
using ITService.Model;
using Microsoft.EntityFrameworkCore;

namespace ITService.UI.Data
{
    public interface IProblemDataProvider
    {
        Task<IEnumerable<Problem>?> Get();
        Task<IEnumerable<Problem>?> GetAll();
        Task<Problem?> GetById(int id);
        void Add(Problem problem);
        void Remove(int id);
        void Update(Problem problem);
    }
    class ProblemDataProvider : IProblemDataProvider
    {
        private ItServiceDb _context;

        public ProblemDataProvider(ItServiceDb context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Problem>?> Get()
        {
            return await _context.Problems
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Problem>?> GetAll()
        {
            return await _context.Problems
                .AsNoTracking()
                .Include(p => p.CurrentInfluenceLevel)
                .Include(p => p.CurrentStatus)
                .ToListAsync();
        }
        public async Task<Problem?> GetById(int id)
        {
            return await _context.Problems
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public void Add(Problem problem)
        {
            _context.Add(problem);
            _context.SaveChanges();
            _context.Entry(problem).State = EntityState.Detached;
        }

        public void Remove(int id)
        {
            var problem = _context.Problems.AsNoTracking().FirstOrDefault(a => a.Id == id);
            _context.Remove(problem);
            _context.SaveChanges();
            _context.Entry(problem).State = EntityState.Detached;
        }
        public void Update(Problem problem)
        {
            _context.Update(problem);
            _context.SaveChanges();
            _context.Entry(problem).State = EntityState.Detached;
        }
    }
}
