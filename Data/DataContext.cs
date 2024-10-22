using Microsoft.EntityFrameworkCore;

namespace DotNet_RPG.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Character> Character { get; set; }
    }
}
