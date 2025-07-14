using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RDDShop.Data;
using RDDShop.DTO.Request;
using RDDShop.DTO.Response;
using RDDShop.Models;
using RDDShop.Models.Category;

namespace RDDShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        ApplicationDbContext context = new ApplicationDbContext();

        private readonly IStringLocalizer<SharedResource> _localizer;
        public CategoriesController(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }



        //Get for Admin
        [HttpGet("All")]
        public IActionResult GetAll([FromQuery] string lang ="en")
        {

            var categories = context.Categories.Include(c => c.CategoryTranslations).ToList().Adapt<List<CategoryResponseDTO>>();

            var result = categories.Select(c => new
            {
                Id = c.Id,
                Name = c.CategoryTranslations.FirstOrDefault(ct => ct.Language == lang).Name
            });


            return Ok(new { message = _localizer["All Admin Categories"].Value, result });
        }
        
        //Get for Costumer
        [HttpGet("CategoryCustomer")]
        public IActionResult GetCustomerCategories([FromQuery] string lang = "en")
        {
            var categories = context.Categories.Include(c => c.CategoryTranslations).ToList().Adapt<List<CategoryResponseDTO>>();

            var result = categories.Select(c => new
            {
                Id = c.Id,
                Name = c.CategoryTranslations.FirstOrDefault(ct => ct.Language == lang).Name
            });
            return Ok(new { message = _localizer["All Customer Categories"].Value, result });
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] CategoryRequestDTO request)
        {
            var categoryInDb = request.Adapt<Category>();
            context.Add(categoryInDb);
            context.SaveChanges();

            return Ok(new { message = _localizer["Category created successfully"].Value  });

        }

        [HttpGet("{id}")]
        public IActionResult Details([FromRoute] int id, [FromQuery] string lang = "en")
        {
            var category = context.Categories
                .Include(c => c.CategoryTranslations)
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound(new { message = _localizer["Category not found"].Value });
            }

            var translation = category.CategoryTranslations.FirstOrDefault(ct => ct.Language == lang);

            var response = new
            {
                Id = category.Id,
                Name = translation?.Name ?? _localizer["No translation available"].Value
            };

            return Ok(new { message = _localizer["Category found"].Value, category = response });
        }


        [HttpPatch("Update/{id}")]
        public IActionResult Update( [FromRoute] int id, [FromBody] CategoryRequestDTO request )
        {
            var categoryInDb = context.Categories.FirstOrDefault(c => c.Id == id);
            if (categoryInDb == null)
            {
                return NotFound(new { message = _localizer["Category not found"].Value });
            }
           // categoryInDb.Name = request.Name;
            context.SaveChanges();
            return Ok(new { message = _localizer["Category Name updated successfully"].Value });
        }

        [HttpPatch("ToggleStatus/{id}")]
        public IActionResult ToggleStatus([FromRoute] int id)
        {
            var categoryInDb = context.Categories.FirstOrDefault(c => c.Id == id);
            if (categoryInDb == null)
            {
                return NotFound(new { message = _localizer["Category not found"].Value });
            }
            categoryInDb.Status = categoryInDb.Status == Status.Active ? Status.Inactive : Status.Active;
            context.SaveChanges();
            return Ok(new { message = _localizer["Category Status updated successfully"].Value });
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var categoryInDb = context.Categories.FirstOrDefault(c => c.Id == id);
            if (categoryInDb == null)
            {
                return NotFound(new { message =  _localizer["Category not found"].Value });
            }
            context.Categories.Remove(categoryInDb);
            context.SaveChanges();
            return Ok(new { message = _localizer["Category deleted successfully"].Value });
        }

        [HttpDelete ("DeleteAll")]
        public IActionResult DeleteAll()
        {
            var categories = context.Categories.ToList();
            if (!categories.Any())
            {
                return NotFound(new { message = _localizer["No categories found to delete"].Value });
            }
            context.Categories.RemoveRange(categories);
            context.SaveChanges();
            return Ok(new { message = _localizer["All categories deleted successfully"].Value });
        }
    }
}
