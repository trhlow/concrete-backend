using Microsoft.EntityFrameworkCore;
using Concrete.Api.Models;

namespace Concrete.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
}
