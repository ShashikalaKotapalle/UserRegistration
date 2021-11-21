using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserRegistration.Infrastructure.Data.EntityConfigurations.User
{
	public class UserEntityConfiguration : IEntityTypeConfiguration<Entities.User>
	{
		public void Configure(EntityTypeBuilder<Entities.User> builder)
		{
			builder.ToTable(nameof(Entities.User)).HasKey(e => e.UserId);

			builder.Property(e => e.UserId).IsRequired().UseIdentityColumn();
			builder.Property(e => e.UserName).IsRequired().HasMaxLength(100);

			builder.HasIndex(e => e.UserName).IsUnique();
		}
	}
}
