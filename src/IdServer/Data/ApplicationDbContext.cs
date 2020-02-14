using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdServer.Models;

namespace IdServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .Ignore(a => a.AccessFailedCount)
                .Ignore(a => a.LockoutEnabled)
                .Ignore(a => a.PhoneNumberConfirmed)
                .Ignore(a => a.PhoneNumber)
                .Ignore(a => a.Email)
                .Ignore(a => a.NormalizedEmail)
                .Ignore(a => a.LockoutEnd)
                .Ignore(a => a.LockoutEnabled)
                .Ignore(a => a.EmailConfirmed)
                .Ignore(a => a.AccessFailedCount)
                .Ignore(a => a.TwoFactorEnabled);
            modelBuilder.Entity<Room>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Room>()
                .Property(b => b.Capacity)
                .HasDefaultValue(4);
            // TODO: Move seed files to separate file
            modelBuilder.Entity<Room>()
                .HasData(
                    new Room { Id = 1, Name = "Public Room 1" },
                    new Room { Id = 2, Name = "Public Room 2", Capacity = 10 }
                );
        }
    }
}
