using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess
{
	public class ApplicationDbContext: IdentityDbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>  options):base(options)
        {
            
        }
		public DbSet<ApplicationUser> Users {  get; set; }
		public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}
}
