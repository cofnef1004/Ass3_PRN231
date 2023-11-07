using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ProductWebClient.Controllers
{
	[Authorize(Roles = UserRoles.Admin)]
	public class ProductController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Create()
		{
			return View();
		}

		public IActionResult Edit()
		{
			return View();
		}
	}
}
