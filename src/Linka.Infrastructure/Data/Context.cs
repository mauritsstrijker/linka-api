using Linka.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Linka.Infrastructure.Data;
public class Context : DbContext, IContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {

    }
    public DbSet<Event> Events { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<EventJob> EventJobs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Volunteer> Volunteers { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>()
             .HasOne(e => e.Organization)
             .WithMany(x => x.Events);

        modelBuilder.Entity<Event>()
            .HasMany(e => e.Jobs)
            .WithOne(x => x.Event);

        modelBuilder.Entity<Event>()
            .HasOne(x => x.Address)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Organization>()
            .HasOne(x => x.Address)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Volunteer>()
           .HasOne(x => x.Address)
           .WithMany()
           .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Organization>()
           .HasOne(x => x.User)
           .WithMany();


        modelBuilder.Entity<Volunteer>()
           .HasOne(x => x.User)
           .WithMany();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
}