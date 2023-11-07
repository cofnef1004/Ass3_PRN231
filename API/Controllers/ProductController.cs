using BusinessObjects;
using ProductWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.impl;
using BusinessObjects.Models;

namespace ProductWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductRepository repository = new ProductRepository();

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts() => repository.GetProducts();

        [HttpGet("Search/{keyword}")]
        public ActionResult<IEnumerable<Product>> Search(string keyword) => repository.Search(keyword);

        [HttpGet("{id}")]
        public ActionResult<Product> GetProductById(int id) => repository.GetProductById(id);

		[HttpPost]
        public IActionResult PostProduct(PostProduct postProduct)
        {
            if (repository.GetProducts().FirstOrDefault(f => f.ProductName.ToLower().Equals(postProduct.ProductName.ToLower())) != null) {
                return BadRequest();
            }
            var f = new Product
            {
                ProductName = postProduct.ProductName,
                UnitPrice = postProduct.UnitPrice,
                UnitInStock = postProduct.UnitInStock,
                Weight = postProduct.Weight,
                CategoryId = postProduct.CategoryID,
            };
            repository.SaveProduct(f);
            return NoContent();
        }

		[HttpDelete("Delete/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var f = repository.GetProductById(id);
            if (f == null)
            {
                return NotFound();
            }
            else
            {
                repository.DeleteProduct(f);
            }
            return NoContent();
        }

		[HttpPut("{id}")]
        public IActionResult PutProduct(int id, PostProduct postProduct)
        {
            var fTmp = repository.GetProductById(id);
            if (fTmp == null)
            {
                return NotFound();
            }

            if (!fTmp.ProductName.ToLower().Equals(postProduct.ProductName.ToLower()) 
                && repository.GetProducts().FirstOrDefault(f => f.ProductName.ToLower().Equals(postProduct.ProductName.ToLower())) != null)
            {
                return BadRequest();
            } else
            {
                fTmp.ProductName = postProduct.ProductName;
            }

            fTmp.Weight = postProduct.Weight;
            fTmp.UnitPrice = postProduct.UnitPrice;
            fTmp.UnitInStock = postProduct.UnitInStock;
            fTmp.CategoryId = postProduct.CategoryID;

            repository.UpdateProduct(fTmp);
            return NoContent();
        }
    }
}
