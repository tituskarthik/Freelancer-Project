using freelancers;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Freelancer> Freelancers { get; set; }
    public DbSet<Skillset> Skillsets { get; set; }
    public DbSet<Hobby> Hobbies { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Skillset>()
            .HasOne<Freelancer>()
            .WithMany(f => f.Skillsets)
            .HasForeignKey(s => s.FreelancerId);

        modelBuilder.Entity<Hobby>()
            .HasOne<Freelancer>()
            .WithMany(f => f.Hobbies)
            .HasForeignKey(h => h.FreelancerId);
    }
}
