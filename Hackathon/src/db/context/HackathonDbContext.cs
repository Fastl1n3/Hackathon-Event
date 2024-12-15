using Hackathon.db.entity;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.db.context;

public class HackathonDbContext : DbContext
{
    public DbSet<HackathonEntity> Hackathons { get; set; }
    public DbSet<EmployeeEntity> Employees { get; set; }
    public DbSet<WishlistEntity> Wishlists { get; set; }
    public DbSet<TeamEntity> Teams { get; set; }
    public DbSet<WishlistEmployee> WishlistEmployees { get; set; }

    public HackathonDbContext(DbContextOptions<HackathonDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HackathonEntity>(entity => {
            entity.HasKey(h => h.Id);
            entity.Property(h => h.HarmonicMean).IsRequired();
            entity.HasMany(h => h.Teams).WithOne(t => t.Hackathon);
        });
        
        modelBuilder.Entity<EmployeeEntity>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Role).IsRequired();
            entity.HasMany(e => e.Wishlists).WithOne(w => w.Employee);
        }); 
        
        modelBuilder.Entity<TeamEntity>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.HasOne(t => t.Hackathon).WithMany(h => h.Teams);
            entity.HasOne(t => t.Teamlead).WithMany();
            entity.HasOne(t => t.Junior).WithMany();
        });
        
        modelBuilder.Entity<WishlistEntity>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.HasOne(w => w.Employee).WithMany(e => e.Wishlists);
            entity.HasOne(w => w.Hackathon).WithMany();
            entity.HasMany(w => w.DesiredEmployees).WithOne(we => we.Wishlist);
        });
        
        modelBuilder.Entity<WishlistEmployee>(entity =>
        {
            entity.HasKey(we => new { we.WishlistId, we.DesiredEmployeeId });
            entity.HasOne(we => we.Wishlist).WithMany(w => w.DesiredEmployees);
            entity.HasOne(we => we.DesiredEmployee).WithMany();
            entity.Property(we => we.Rank).IsRequired();
        });
    }
}