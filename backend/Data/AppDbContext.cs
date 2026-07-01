using Microsoft.EntityFrameworkCore;
using trial.Models;

namespace trial.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<User> Users {get; set;}

        public DbSet<Note> Notes {get; set;}

        public DbSet<NoteGroup> NoteGroups {get; set;} 

        public DbSet<PermissionPolicy> PermissionPolicies {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasOne(n => n.CreatedBy)
                      .WithMany(u => u.Notes)
                      .HasForeignKey(n => n.CreatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict); // TODO check behav
                // More?
                entity.HasOne(n => n.NoteGroup)
                      .WithMany(g => g.GroupNotes)
                      .HasForeignKey(n => n.GroupId); // TODO onDelete?

            });
            modelBuilder.Entity<NoteGroup>(entity =>
            {
                
            });
            modelBuilder.Entity<PermissionPolicy>(entity =>
            {
                entity.HasOne(p => p.Owner)
                      .WithMany(u => u.GivenPermissions)
                      .HasForeignKey(p => p.OwnerId)
                      .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(p => p.Guest)
                      .WithMany(u => u.ReceivedPermissions)
                      .HasForeignKey(p => p.GuestId)
                      .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(p => p.Note)
                      .WithMany(n => n.PermissionPolicies)
                      .HasForeignKey(p => p.NoteId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(p => new { p.OwnerId, p.GuestId, p.NoteId })
                      .IsUnique();
            });

        }
    }
}