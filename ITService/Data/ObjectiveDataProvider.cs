    using ITService.DataAccess.Context;
    using ITService.Model;
    using Microsoft.EntityFrameworkCore;

    namespace ITService.UI.Data
    {
        public interface IObjectiveDataProvider
        {
            Task<IEnumerable<Objective>?> Get();
            Task<IEnumerable<Objective>?> GetAll();
            Task<Objective?> GetById(int id);
            void Add(Objective objective);
            void Remove(int id);
            void Update(Objective objective);
        }
        class ObjectiveDataProvider : IObjectiveDataProvider
        {
            private ItServiceDb _context;

            public ObjectiveDataProvider(ItServiceDb context)
            {
                _context = context;
            }
            public async Task<IEnumerable<Objective>?> Get()
            {
                return await _context.Objectives
                    .AsNoTracking()
                    .ToListAsync();
            }

            public async Task<IEnumerable<Objective>?> GetAll()
            {
                return await _context.Objectives
                    .AsNoTracking()
                    .Include(o => o.ResponsibleEmployee)
                    .Include(o => o.CurrentStatus)
                    .ToListAsync();
            }
            public async Task<Objective?> GetById(int id)
            {
                return await _context.Objectives
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Id == id);
            }
            public void Add(Objective objective)
            {
                _context.Add(objective);
                _context.SaveChanges();
                _context.Entry(objective).State = EntityState.Detached;
            }

            public void Remove(int id)
            {
                var objective = _context.Objectives.AsNoTracking().FirstOrDefault(a => a.Id == id);
                _context.Remove(objective);
                _context.SaveChanges();
                _context.Entry(objective).State = EntityState.Detached;
            }
            public void Update(Objective objective)
            {
                _context.Update(objective);
                _context.SaveChanges();
                _context.Entry(objective).State = EntityState.Detached;
            }
        }
    }
