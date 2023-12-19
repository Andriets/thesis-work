using AutoTrader.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTrader.Infrastructure.Configurations
{
    internal class MacdBotConfiguration : IEntityTypeConfiguration<MacdBot>
    {
        public void Configure(EntityTypeBuilder<MacdBot> builder)
        {
            builder.HasOne(b => b.User)
                .WithMany(u => u.Bots)
                .HasForeignKey(b => b.UserId);
        }
    }
}
