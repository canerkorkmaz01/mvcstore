using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace MvcStoreData
{

    public enum Genders
    {
        Male, Female, Unspecified
    }
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }

        public Genders Gender { get; set; }

        public ICollection<Brand> Brands { get; set; } = new HashSet<Brand>();
        public ICollection<Category> Categories { get; set; } = new HashSet<Category>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public ICollection<CommentLike> CommentLikes { get; set; } = new HashSet<CommentLike>();
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
        public ICollection<ProductImage> ProductImages { get; set; } = new HashSet<ProductImage>();
        public ICollection<Rayon> Rayons { get; set; } = new HashSet<Rayon>();


    }

    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder
                .HasMany(p => p.Brands)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(p => p.Categories)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .HasMany(p => p.Comments)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .HasMany(p => p.CommentLikes)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .HasMany(p => p.Orders)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .HasMany(p => p.OrderItems)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .HasMany(p => p.Products)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .HasMany(p => p.ProductImages)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .HasMany(p => p.Rayons)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}