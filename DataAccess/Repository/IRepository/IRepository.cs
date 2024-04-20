using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
		Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
		Task AddAsync(T item);
		void Delete(T item);
		void DeleteRange(IEnumerable<T> items);
	}
}
