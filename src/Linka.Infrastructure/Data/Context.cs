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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
}