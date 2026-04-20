using Microsoft.EntityFrameworkCore;
using MKR1_AspNet.Entities;
using MKR2_AspNet.Entities;

namespace MKR1_AspNet
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Classroom> Classrooms => Set<Classroom>();
        public DbSet<Book> Books => Set<Book>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Classroom>()
                .HasMany(c => c.Students)
                .WithOne(s => s.Classroom)
                .HasForeignKey(s => s.ClassroomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Author).IsRequired().HasMaxLength(100);
            });
        }
    }
}