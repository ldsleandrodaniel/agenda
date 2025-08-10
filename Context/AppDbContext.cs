using Agenda.Models;
using Microsoft.EntityFrameworkCore;


namespace Agenda.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<ModeloAgenda> agenda { get; set; }

    }

}