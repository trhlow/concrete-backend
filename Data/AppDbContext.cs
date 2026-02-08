using Concrete.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Concrete.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Order> Orders => Set<Order>(); // ⬅ BẮT BUỘC
}
