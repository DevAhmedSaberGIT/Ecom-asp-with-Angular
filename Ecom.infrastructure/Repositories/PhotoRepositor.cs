using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;
using Ecom.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories
{
    public class PhotoRepositor : GenericRepository<Photo>, IPhotoRepository
    {
        public PhotoRepositor(AppDbContext context) : base(context)
        {
        }
    }
}
