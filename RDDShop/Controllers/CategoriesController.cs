using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RDDShop.Data;
using RDDShop.DTO.Request;
using RDDShop.DTO.Response;
using RDDShop.Models;

namespace RDDShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        ApplicationDbContext context = new ApplicationDbContext();
        //Get for Admin
        [HttpGet("All")]
        public IActionResult GetAll()
        {
            var categories = context.Categories.ToList().Adapt<List<CategoryResponseDTO>>();
            return Ok(new { message = "All Admin Categories", categories });
        }
        //Get for Costumer
        [HttpGet("CategoryCustomer")]
        public IActionResult GetCustomerCategories()
        {
            var categories = context.Categories.Where(c=> c.Status == Status.Active).ToList().Adapt<List<CategoryResponseDTO>>();
            return Ok(new { message = "All Customer Categories", categories });
        }
        [HttpPost("Create")]
        public IActionResult Create(CategoryRequestDTO request)
        {
            var categoryInDb = request.Adapt<Category>();
            context.Add(categoryInDb);
            context.SaveChanges();

            return Ok(new { message = "Category created successfully" });

        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }
            var categoryResponse = category.Adapt<CategoryResponseDTO>();
            return Ok(new { message = "Category found", category = categoryResponse });
        }

        [HttpPatch("Update/{id}")]
        public IActionResult Update(int id, CategoryRequestDTO request)
        {
            var categoryInDb = context.Categories.FirstOrDefault(c => c.Id == id);
            if (categoryInDb == null)
            {
                return NotFound(new { message = "Category not found" });
            }
            categoryInDb.Name = request.Name;
            context.SaveChanges();
            return Ok(new { message = "Category Name updated successfully" });
        }

        [HttpPatch("ToggleStatus/{id}")]
        public IActionResult ToggleStatus(int id)
        {
            var categoryInDb = context.Categories.FirstOrDefault(c => c.Id == id);
            if (categoryInDb == null)
            {
                return NotFound(new { message = "Category not found" });
            }
            categoryInDb.Status = categoryInDb.Status == Status.Active ? Status.Inactive : Status.Active;
            context.SaveChanges();
            return Ok(new { message = "Category Status updated successfully" });
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var categoryInDb = context.Categories.FirstOrDefault(c => c.Id == id);
            if (categoryInDb == null)
            {
                return NotFound(new { message = "Category not found" });
            }
            context.Categories.Remove(categoryInDb);
            context.SaveChanges();
            return Ok(new { message = "Category deleted successfully" });
        }

        [HttpDelete ("DeleteAll")]
        public IActionResult DeleteAll()
        {
            var categories = context.Categories.ToList();
            if (!categories.Any())
            {
                return NotFound(new { message = "No categories found to delete" });
            }
            context.Categories.RemoveRange(categories);
            context.SaveChanges();
            return Ok(new { message = "All categories deleted successfully" });
        }
    }
}
