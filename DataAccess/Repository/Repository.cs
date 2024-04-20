using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _db;
		 DbSet<T> _dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db= db;
			_dbSet = _db.Set<T>();
        }
        public async Task AddAsync(T item)
		{
			await _dbSet.AddAsync(item);
		}

		public void Delete(T item)
		{
			_dbSet.Remove(item);
		}
		public void DeleteRange(IEnumerable<T> items)
		{
			_dbSet.RemoveRange(items);
		}

		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
		{
			IQueryable<T> query = _dbSet;
			if(filter != null)
			{
				query=query.Where(filter);
			}
			return await query.ToListAsync();
		}

		public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
		{
			IQueryable<T> query = _dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			return await query.FirstOrDefaultAsync();
		}
	}
}
