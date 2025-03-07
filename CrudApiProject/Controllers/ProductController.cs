using CrudApiProject.Data;
using CrudApiProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudApiProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		public ProductController(ApplicationDbContext context)
		{
			_context = context;
		}
		[HttpGet]
		public IActionResult GetProducts()
		{
			return Ok(_context.Products.Include(p => p.Category).ToList());
		}
		[HttpGet("{id}")]
		public IActionResult GetProduct(int id)
		{
			var product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == id);
			return Ok(product);
		}
		[HttpPost]
		public IActionResult CreateProduct(Product product)
		{
			_context.Products.Add(product);
			_context.SaveChanges();
			return Ok("product created sucsesfully");
		}

		[HttpPut("{id}")]
		public IActionResult UpdateProduct(int id, Product product)
		{
			if (id != product.ProductId)
				return BadRequest();
			var existingProduct = _context.Products.Find(id);
			if (existingProduct == null)
				return NotFound("Product not found.");
			existingProduct.ProductName = product.ProductName;
			existingProduct.Price = product.Price;
			existingProduct.Description = product.Description;

			_context.Products.Update(product);
			_context.SaveChanges();
			return Ok("updated sucsesfully");
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteProduct(int id)
		{
			var product = _context.Products.Find(id);
			if (product == null)
				return NotFound();
			_context.Products.Remove(product);
			_context.SaveChanges();
			return Ok("removed sucsesfully");
		}

		[HttpPut("batchUpdate")]
		public IActionResult BatchUpdateProducts(List<Product> products)
		{
			var existingProducts = _context.Products.Where(p => products.Select(x => x.ProductId).Contains(p.ProductId)).ToList();
			if (existingProducts.Count != products.Count)
				return NotFound();

			foreach (var product in products)
			{
				var existingProduct = existingProducts.First(p => p.ProductId == product.ProductId);
				existingProduct.ProductName = product.ProductName;
				existingProduct.Price = product.Price;
				existingProduct.Description = product.Description;
				existingProduct.CategoryId = product.CategoryId;
			}
			_context.SaveChanges();
			return Ok("updated sucsesfully");
		}
	}
}
