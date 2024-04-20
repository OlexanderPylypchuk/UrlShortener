using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DataAccess.Repository.IRepository
{
	public interface IUserRepository:IRepository<ApplicationUser>
	{
		void Update(ApplicationUser user);
		Task<bool> Login(string username, string password);
		Task<bool> Register(string username, string password, string name, string role);
		Task Logout();
	}
}
