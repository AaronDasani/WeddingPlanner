using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Models
{
    public class WeddingPlannerContext:DbContext
    {
        public WeddingPlannerContext(DbContextOptions<WeddingPlannerContext> option):base(option){}

        public DbSet<Wedding> Weddings{get;set;}
        public DbSet<User> Users{get;set;}
        public DbSet<WeddingGuest> WeddingGuests{get;set;}

    }
}