using AutoTrader.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoTrader.Domain.Abstractions
{
    public interface IAppDbContext
    {
        DbSet<AppUser> Users { get; set; }

        DbSet<MacdBot> Bots { get; set; }

        DbSet<Order> Orders { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
