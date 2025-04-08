using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecom.API.Controllers
{
  
    public class ProductController : BaseController
    {
        public ProductController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }
        [HttpGet(template: "GetAll")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                // استدعاء الدالة لتنفيذ جلب جميع المنتجات مع تضمين Category والصور.
                IReadOnlyList<Product> products = await Work.ProductRepository.GetAllAsync(x => x.Category, x => x.photos);

          
                // استخدام Mapper لتحويل الكيانات إلى DTO إذا لزم الأمر.
                var result = Mapper.Map<List<ProductDTO>>(products);
                // التأكد من عدم كون النتيجة فارغة
                if (products == null)
                {
                    // يمكن استخدام NotFound بدلاً من BadRequest
                    return NotFound(new ResponseAPI(404, "No products found."));
                }

                return Ok(new
                {
                    Response = new ResponseAPI(200, "Products retrieved successfully."),
                    Data = result // أو Data = products إذا كنت تريد إرجاع الكيانات الأصلية
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI(500, $"Internal Server Error: {ex.Message}"));
            }
        }
        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                // جلب المنتج بناءً على ID وتضمين الصور والتصنيف
                var product = await Work.ProductRepository.GetByIdAsync(id, x => x.Category, x => x.photos);

                // التحقق من عدم وجود المنتج
                if (product == null)
                    return NotFound(new ResponseAPI(404, $"Product with ID {id} not found."));

                // تحويل المنتج إلى DTO
                var result = Mapper.Map<ProductDTO>(product);

                return Ok(new
                {
                    Response = new ResponseAPI(200, "Product retrieved successfully."),
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI(500, $"Internal Server Error: {ex.Message}"));
            }
        }

        [HttpPost("Add-Product")]
        public async Task<IActionResult> AddProduct(AddProductDTO productDTO)
        {
            try
            {
                // محاولة إضافة المنتج باستخدام Repository
                var result = await Work.ProductRepository.AddAsync(productDTO);

                // إذا نجحت العملية نُرجع استجابة Ok، وإلا BadRequest
                if (result)
                {
                    return Ok(new ResponseAPI(200, "Product added successfully."));
                }
                else
                {
                    return BadRequest(new ResponseAPI(400, "Failed to add product."));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI(500, $"Internal Server Error: {ex.Message}"));
            }
        }


        [HttpPut("Update-Product")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO productDTO)
        {
            try
            {
                // محاولة تحديث المنتج باستخدام Repository
                var result = await Work.ProductRepository.UpdateAsync(productDTO);
                // إذا نجحت العملية نُرجع استجابة Ok، وإلا BadRequest
                if (result)
                {
                    return Ok(new ResponseAPI(200, "Product updated successfully."));
                }
                else
                {
                    return BadRequest(new ResponseAPI(400, "Failed to update product."));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI(500, $"Internal Server Error: {ex.Message}"));
            }
        }

        [HttpDelete("Delete-Product/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                // البحث عن المنتج أولا
                var product = await Work.ProductRepository.GetByIdAsync(id);

                if (product == null)
                {
                    return NotFound(new ResponseAPI(404, "Product not found."));
                }

                // محاولة حذف المنتج باستخدام Repository
                var result = await Work.ProductRepository.DeleteAsync(product);

                // إذا نجحت العملية نُرجع استجابة Ok، وإلا BadRequest
                if (result)
                {
                    return Ok(new ResponseAPI(200, "Product deleted successfully."));
                }
                else
                {
                    return BadRequest(new ResponseAPI(400, "Failed to delete product."));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI(500, $"Internal Server Error: {ex.Message}"));
            }
        }
    }
}
