﻿using Microsoft.AspNetCore.Mvc;

namespace UrlShortenerWebApp.Controllers
{
	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Register()
		{
			return View();
		}
		public IActionResult Login()
		{
			return View();
		}
	}
}
