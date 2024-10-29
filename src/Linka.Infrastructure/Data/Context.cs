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
    public DbSet<JobVolunteerActivity> JobVolunterActivities { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostLike> PostLikes { get; set; }
    public DbSet<PostComment> PostComments { get; set; }
    public DbSet<PostShare> PostShares { get; set; }
    public DbSet<Follow> Follows { get; set; }
    public DbSet<ConnectionRequest> ConnectionRequests { get; set; }
    public DbSet<Connection> Connections { get; set; }
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
           .WithMany()
           .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<Volunteer>()
           .HasOne(x => x.User)
           .WithMany()
           .OnDelete(DeleteBehavior.NoAction);

        //
        modelBuilder.Entity<EventJob>()
            .HasMany(x => x.Volunteers)
            .WithMany(x => x.Jobs);
        //

        modelBuilder.Entity<Post>()
            .HasMany(p => p.Likes)
            .WithOne(pl => pl.Post);

        modelBuilder.Entity<Post>()
           .HasOne(x => x.Author)
           .WithMany(u => u.Posts)
           .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity <PostLike>()
           .HasOne(x => x.User)
           .WithMany(u => u.Likes)
           .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<PostShare>()
         .HasOne(x => x.User)
         .WithMany(u => u.Shares)
         .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<PostComment>()
         .HasOne(x => x.Author)
         .WithMany(u => u.Comments)
         .OnDelete(DeleteBehavior.NoAction);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
}