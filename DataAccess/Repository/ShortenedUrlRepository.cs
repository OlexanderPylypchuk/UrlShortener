using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repository.IRepository;
using Models;

namespace DataAccess.Repository
{
	internal class ShortenedUrlRepository : Repository<ShortenedUrl>, IShortenedUrlRepository
	{
		private readonly ApplicationDbContext _db;
        public ShortenedUrlRepository(ApplicationDbContext applicationDbContext):base (applicationDbContext) 
        {
            _db = applicationDbContext;
        }
        public void Update(ShortenedUrl shortenedUrl)
		{
			_db.ShortenedUrls.Update(shortenedUrl);
		}
	}
}
