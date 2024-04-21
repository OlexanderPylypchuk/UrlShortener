using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.ViewModels;
using Utility;

namespace UrlShortenerWebApp.Controllers
{
	public class UserController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
			_roleManager = roleManager;
        }
		public async Task<IActionResult> Register()
		{
			if(!await _roleManager.RoleExistsAsync(SD.RoleCustomer))
			{
				await _roleManager.CreateAsync(new IdentityRole(SD.RoleCustomer));
				await _roleManager.CreateAsync(new IdentityRole(SD.RoleAdmin));
			}
			var roles = _roleManager.Roles.Select(x=>new SelectListItem()
			{
				Text = x.Name,
				Value = x.Name
			}).ToList();
			RegisterVM registerVM = new RegisterVM()
			{
				Roles=roles
			};
			return View(registerVM);
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterVM registerVM)
		{
			if (ModelState.IsValid && registerVM.ConfirmPassword == registerVM.Password)
			{
				var isSuccessful=await _unitOfWork.UserRepository.Register(registerVM.UserName, registerVM.Password,registerVM.Name,registerVM.Role);
				if (isSuccessful)
				{
					await _unitOfWork.UserRepository.Login(registerVM.UserName, registerVM.Password);
					TempData["success"] = "Registered successfully";
					return RedirectToAction("Index", "Home");
                }
			}
			var roles = _roleManager.Roles.Select(x => new SelectListItem()
			{
				Text = x.Name,
				Value = x.Name
			}).ToList();
			registerVM.Roles = roles;
			if(registerVM.ConfirmPassword != registerVM.Password)
			{
                TempData["error"] = "Password does not match";
            }
			else TempData["error"] = "Something went wrong";
			return View(registerVM);
		}
		public IActionResult Login()
		{
			LoginVM loginVM=new LoginVM();
			return View(loginVM);
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginVM loginVM)
		{
			if(ModelState.IsValid)
			{
				var isSuccessful=await _unitOfWork.UserRepository.Login(loginVM.UserName, loginVM.Password);
				if(isSuccessful)
				{
					TempData["success"] = "Loged in successfully";
                    return RedirectToAction("Index", "Home");
                }
			}
			TempData["error"] = "Something went wrong";
			return View(loginVM);
		}
		public async Task<IActionResult> Logout()
		{
			await _unitOfWork.UserRepository.Logout();
			TempData["success"] = "Loged out successfully";
			return RedirectToAction("Index","Home");
		}
	}
}
