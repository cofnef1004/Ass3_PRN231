using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.impl;

namespace ProductWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryRepository repository = new CategoryRepository();

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategories() => repository.GetCategories();

        [HttpPost]
        public IActionResult PostCategory(Category category)
        {
            repository.SaveCategory(category);
            return NoContent();
        }
    }
}
