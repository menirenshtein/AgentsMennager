using AgentsMennager.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentsMennager.DAL
{

    // Represents the data layer of the application, using Entity Framework Core
    public class DataLayer : DbContext
    
    {
        // DbSet properties represent collections of entities that can be queried and saved
        public DbSet<AgentModel> agents { get; set; }
        public DbSet<TargetModel> targets { get; set; }
        public DbSet<MissionModel> missions { get; set; }
        public DbSet<PositionModel> positions { get; set; }

    
        // Constructor for the DataLayer class, ensures the database is created
        public DataLayer(DbContextOptions<DataLayer> option) : base(option)
        {
            Database.EnsureCreated();
        }
    }
}
