using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Services
{
    public interface IImageMangmentService
    {
        Task<List<string>> AddImageAsyns(IFormFileCollection files , string src );
        Task<string> DelteImageAsyns(string src);
    }
}
