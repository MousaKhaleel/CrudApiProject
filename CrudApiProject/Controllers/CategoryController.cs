using CrudApiProject.Data;
using CrudApiProject.Dtos;
using CrudApiProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace CrudApiProject.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		public CategoryController(ApplicationDbContext context)
		{
			_context = context;
		}
		[HttpGet]
		public IActionResult GetCategories()
		{
			return Ok(_context.Categories.ToList());
		}

		[HttpGet("{id}")]
		public IActionResult GetCategory(int id)
		{
			var category = _context.Categories.Find(id);
			return Ok(category);
		}

		[HttpPost]
		public IActionResult CreateCategory(CategoryDto categoryDto)
		{
			var category = new Category
			{
				CategoryName = categoryDto.CategoryName,
			};
			_context.Categories.Add(category);
			_context.SaveChanges();
			return Ok("Created sucsesfully");
		}

		[HttpPut("{id}")]
		public IActionResult UpdateCategory(int id, CategoryDto categoryDto)
		{
			var existingCategory = _context.Categories.Find(id);
			if (existingCategory == null)
				return NotFound("Category not found.");
			existingCategory.CategoryName = categoryDto.CategoryName;

			_context.Categories.Update(existingCategory);
			_context.SaveChanges();
			return Ok("updated sucsesfully");
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteCategory(int id)
		{
			var category = _context.Categories.Find(id);
			if (category == null)
				return NotFound();
			_context.Categories.Remove(category);
			_context.SaveChanges();
			return Ok("deleted sucsesfully");
		}
	}
}
