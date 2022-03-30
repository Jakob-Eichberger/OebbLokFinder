using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    // All the code in this file is included in all platforms.
    public class Database : DbContext
    {
        public Database(DbContextOptions options) : base(options)
        {
        }

        protected Database()
        {
        }
    }
}