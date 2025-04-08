using AutoMapper;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using System;

namespace Ecom.infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICateroryRepository CateroryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IPhotoRepository PhotoRepository { get; }

        // تعديل المُنشئ ليقبل IMapper و IImageMangmentService
        public UnitOfWork(AppDbContext context, IMapper mapper, IImageMangmentService imageMangmentService)
        {
            _context = context;
            CateroryRepository = new CateroryRepository(_context);
            ProductRepository = new ProductRepository(_context, mapper, imageMangmentService);
            PhotoRepository = new PhotoRepository(_context);
        }
    }
}
