using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
	public class ProductDAO
	{
		public static List<Product> GetProducts()
		{
			var listProducts = new List<Product>();
			try
			{
				using (var context = new MyDbContext())
				{
					listProducts = context.Products.ToList();
					listProducts.ForEach(f =>
					{
						f.Category = context.Categories.Find(f.CategoryId);
					});
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
			return listProducts;
		}

		public static List<Product> Search(string keyword)
		{
			var listProducts = new List<Product>();
			try
			{
				using (var context = new MyDbContext())
				{
					listProducts = context.Products.Where(f => f.ProductName.Contains(keyword)).ToList();
					listProducts.ForEach(f =>
					{
						f.Category = context.Categories.Find(f.CategoryId);
					});
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
			return listProducts;
		}

		public static List<Product> FindAllProductsByCategoryId(int categoryId)
		{
			var listProducts = new List<Product>();
			try
			{
				using (var context = new MyDbContext())
				{
					listProducts = context.Products.Where(f => f.CategoryId == categoryId).ToList();
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
			return listProducts;
		}

		public static Product FindProductById(int ProductId)
		{
			var Product = new Product();
			try
			{
				using (var context = new MyDbContext())
				{
					Product = context.Products.SingleOrDefault(f => f.ProductId == ProductId);
					Product.Category = context.Categories.Find(Product.CategoryId);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return Product;
		}

		public static void SaveProduct(Product Product)
		{
			try
			{
				using (var context = new MyDbContext())
				{
					context.Products.Add(Product);
					context.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public static void UpdateProduct(Product Product)
		{
			try
			{
				using (var context = new MyDbContext())
				{
					context.Entry(Product).State =
						Microsoft.EntityFrameworkCore.EntityState.Modified;
					context.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public static void DeleteProduct(Product Product)
		{
			try
			{
				using (var context = new MyDbContext())
				{
					var ProductToDelete = context
						.Products
						.SingleOrDefault(f => f.ProductId == Product.ProductId);
					context.Products.Remove(ProductToDelete);
					context.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
