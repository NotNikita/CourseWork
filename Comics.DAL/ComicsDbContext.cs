using System;
using Microsoft.EntityFrameworkCore;
using Comics.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Comics.DAL
{
    public class ComicsDbContext : IdentityDbContext<User>
    {
        public ComicsDbContext(DbContextOptions<ComicsDbContext> options) : base(options) { }

        public DbSet<BaseItem> BaseItems { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Comic> Comics { get; set; }
        public DbSet<Whisky> Whiskies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Collection> Collections { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
