
using Microsoft.EntityFrameworkCore;
using src.Models;

namespace src.Model
{
    public class ChallengeDBContext : DbContext
    {
        public DbSet<OFXTransactionModel> OFXTransactionModels { get; set; }

        public ChallengeDBContext(DbContextOptions<ChallengeDBContext> options) :
            base(options)
        {
        }
    }
}