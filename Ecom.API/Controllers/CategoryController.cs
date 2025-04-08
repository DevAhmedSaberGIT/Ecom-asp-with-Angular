using AutoMapper;
using Azure.Core;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }


        // 🔹 Get All Categories
        [HttpGet("get-all")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await Work.CateroryRepository.GetAllAsync();

                // مثال: إذا لم تكن هناك فئات يرجع BadRequest مع رسالة مخصصة
                if (categories == null || !categories.Any())
                {
                    return BadRequest(new ResponseAPI(400, "No categories found."));
                }

                // في حالة نجاح العملية يرجع Ok مع رسالة من ResponseAPI
                return Ok(new ResponseAPI(200, "Categories retrieved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI(500, $"Internal Server Error: {ex.Message}"));
            }
        }

        // 🔹 Get Category By ID
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await Work.CateroryRepository.GetByIdAsync(id);
                if (category == null)
                    return NotFound(new ResponseAPI(404, $"Category with ID {id} not found."));

                // يمكنك إرجاع البيانات ضمن response إضافية إن رغبت، مثل:
                return Ok(new
                {
                    Response = new ResponseAPI(200, "Category retrieved successfully"),
                    Data = category
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI(500, $"Internal Server Error: {ex.Message}"));
            }
        }

        [HttpPost("add-Category")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                var category = Mapper.Map<Category>(categoryDTO);
                await Work.CateroryRepository.AddAsync(category);

                return Ok(new ResponseAPI(200, "Item has been added successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

        [HttpPut("update-Category")]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDTO updateCategoryDTO)
        {
            try
            {
                var category = Mapper.Map<Category>(updateCategoryDTO);
                await Work.CateroryRepository.UpdateAsync(category);
                return Ok(new ResponseAPI(200, "Item has been updated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

        [HttpDelete("Delete-Category")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await Work.CateroryRepository.GetByIdAsync(id);
                if (category == null)
                    return NotFound(new ResponseAPI(404, $"Category with ID {id} not found."));

                await Work.CateroryRepository.DeleteAsync(category);
                return Ok(new ResponseAPI(200, "Item has been deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }


    }
}
