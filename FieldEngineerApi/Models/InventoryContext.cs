using Microsoft.EntityFrameworkCore;

namespace FieldEngineerApi.Models
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options)
            : base(options)
        {

        }

        public DbSet<BoilerPart>? BoilerParts { get; set; }
    }
}