using System.Security.Claims;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using Utility;

namespace UrlShortenerWebApp.Controllers
{
	[Route("url")]
	public class UrlController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		public UrlController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<IActionResult> Index()
		{
			var ShortUrlVm = new ShortUrlVM()
			{
				UrlRequest = new(),
				ShortUrls =(await _unitOfWork.ShortenedUrlRepository.GetAllAsync(includeProperties:"ApplicationUser")).ToList()
			};
			return View(ShortUrlVm);
		}
		[HttpPost]
		public async Task<IActionResult> CreateShortUrl(ShortUrlVM shortUrlVM)
		{
			if(ModelState.IsValid)
			{
				if(!Uri.TryCreate(shortUrlVM.UrlRequest.Url, UriKind.Absolute, out var _))
				{
					TempData["error"] = "Url is not valid!";
					return RedirectToAction("Index");
				}
				if (await _unitOfWork.ShortenedUrlRepository.GetAsync(u => u.LongUrl == shortUrlVM.UrlRequest.Url) != null)
				{
                    TempData["error"] = "Url was already shortened!";
                    return RedirectToAction("Index");
                }
				var code = await GenerateUrlCode();
				ShortenedUrl shortenedUrl = new()
				{
					Code = code,
					UserId = HttpContext.User.Claims.Where(u => u.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value,
					CreatedDate = DateTime.Now,
					LongUrl = shortUrlVM.UrlRequest.Url,
					ShortUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/shorturl/{code}"
				};
				await _unitOfWork.ShortenedUrlRepository.AddAsync(shortenedUrl);
				_unitOfWork.Save();
                TempData["success"] = "Url was successfully shortened!";
            }
			return RedirectToAction("Index"); 
		}
		
        private async Task<string> GenerateUrlCode()
		{
			Random random = new Random();
			var urls=await _unitOfWork.ShortenedUrlRepository.GetAllAsync();
			while (true)
			{
				char[] chars = new char[SD.MAXCHARACTERS];
				for(int i = 0;i<SD.MAXCHARACTERS;i++)
				{
					chars[i] = SD.allCharacters[random.Next(SD.allCharacters.Length)];
				}
				string code = new string(chars);
				if (!urls.Any(u=>u.Code==code))
				{
					return code;
				}
			}
		}
		[Route("delete/{code}")]
		public async Task<IActionResult> Delete(string code)
		{
			var shorturl=await _unitOfWork.ShortenedUrlRepository.GetAsync(u=>u.Code == code);
			if (shorturl!=null && (HttpContext.User.IsInRole(SD.RoleAdmin) || 
				HttpContext.User.Claims.Where(u => u.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value==shorturl.UserId))
			{
				_unitOfWork.ShortenedUrlRepository.Delete(shorturl);
				_unitOfWork.Save();
                TempData["success"] = "Url was successfully deleted!";
            }
			return RedirectToAction("Index");
		}
	}
}
