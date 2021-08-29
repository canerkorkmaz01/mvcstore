using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MvcStoreData.Infrastructure
{
    public class UserAddress : IUserAddress
    {

        public virtual int Id { get; set; }

        public virtual int UserId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Address { get; set; }

        public virtual int CityId { get; set; }

        public virtual User User { get; set; }

        public virtual City City { get; set; }

    }

    public class UserAddressEntityTypeConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder
                .HasIndex(p => new { p.Name, p.UserId })
                .IsUnique();

            builder
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(p => p.Address)
                .IsRequired();

        }
    }
}
