using Microsoft.EntityFrameworkCore;
using ITService.Model;

namespace ITService.DataAccess.Context
{
    public class ItServiceDb : DbContext
    {
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<Change> Changes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<InfluenceLevel> InfluenceLevels { get; set; }
        public DbSet<Objective> Objectives { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Data Source=.;Initial Catalog=ITServiceDb;Integrated Security=True;TrustServerCertificate=true;Trusted_Connection=True");
        }
    }
}
