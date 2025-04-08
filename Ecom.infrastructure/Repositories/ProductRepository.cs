using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repositories.Services;
using Ecom.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore; // Add this for Include extension method
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageMangmentService _ImageMangmentService;

        public ProductRepository(AppDbContext context, IMapper mapper, IImageMangmentService imageMangmentService)
            : base(context)
        {
            _context = context;
            _mapper = mapper;
            _ImageMangmentService = imageMangmentService;
        }

        public async Task<bool> AddAsync(AddProductDTO productDTO)
        {
            // التحقق من أن بيانات المنتج غير فارغة
            if (productDTO == null) return false;

            // تحويل الـ DTO إلى كيان Product باستخدام AutoMapper
            var product = _mapper.Map<Product>(productDTO);

            // إضافة المنتج إلى قاعدة البيانات
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // استدعاء خدمة إدارة الصور لحفظ الصور على السيرفر أو الدليل المحدد.
            // باستخدام المتغير _ImageMangmentService بدلاً من اسم الكلاس مباشرةً.
            var imagePaths = await _ImageMangmentService.AddImageAsyns(productDTO.Photo, productDTO.Name);

            // بناء كائنات Photo من المسارات التي أُعيدت
            var photos = imagePaths.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id
            }).ToList();

            // حفظ الصور في قاعدة البيانات
            _context.Photos.AddRange(photos);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO)
        {
            if (updateProductDTO is null)
            {
                return false;
            }

            Product findProduct = await _context.Products
                .Include(m => m.Category)
                .Include(m => m.photos) // Note: Changed from "photos" to "Photos" to match property name
                .FirstOrDefaultAsync(m => m.Id == updateProductDTO.Id);

            if (findProduct is null)
            {
                return false;
            }

            // Map the update DTO to the existing product
            _mapper.Map(updateProductDTO, findProduct);

            // Handle image updates if there are new photos
            if (updateProductDTO.Photo != null && updateProductDTO.Photo.Any())
            {
                var imagePaths = await _ImageMangmentService.AddImageAsyns(updateProductDTO.Photo, updateProductDTO.Name);

                // Create photo entities from the returned paths
                var photos = imagePaths.Select(path => new Photo
                {
                    ImageName = path,
                    ProductId = findProduct.Id
                }).ToList();

                // Add the new photos to the database
                _context.Photos.AddRange(photos);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return true;
        }

        Task<bool> IProductRepository.DeleteAsync(Product product)
        {
            if (product == null)
                return Task.FromResult(false);

            try
            {
                // First remove any associated photos
                var photos = _context.Photos.Where(p => p.ProductId == product.Id).ToList();
                if (photos.Any())
                {
                    _context.Photos.RemoveRange(photos);
                }

                // Then remove the product itself
                _context.Products.Remove(product);

                // Save changes
                var result = _context.SaveChangesAsync();

                // Return true if at least one entity was affected
                return result.ContinueWith(t => t.Result > 0);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }
    }
}