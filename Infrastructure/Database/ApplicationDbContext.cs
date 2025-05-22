using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace HeroForge_OnceAgain.Infrastructure.Database
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
