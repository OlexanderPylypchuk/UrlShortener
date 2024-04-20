using Microsoft.AspNetCore.Mvc;

namespace UrlShortenerWebApp.Controllers
{
	public class UrlController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult RedirectToPage(string code)
		{
			return View();
		}
	}
}
