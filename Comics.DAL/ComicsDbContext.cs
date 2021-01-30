using System;
using Microsoft.EntityFrameworkCore;
using Comics.Domain;
using Comics.Domain.CrossRefModel;
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
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // BaseItem - User
            builder.Entity<Like>()
                .HasKey(like => new { like.Id });

            builder.Entity<Like>()
                .HasOne(lk => lk.Item)
                .WithMany(lk => lk.Likes)
                .HasForeignKey(lk => lk.ItemId);

            builder.Entity<Like>()
                .HasOne(mp => mp.User)
                .WithMany(mp => mp.Likes)
                .HasForeignKey(mp => mp.UserId);

            base.OnModelCreating(builder);
        }
    }
}
