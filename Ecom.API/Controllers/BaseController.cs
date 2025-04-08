using AutoMapper;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IUnitOfWork Work;
        protected readonly IMapper Mapper;
        public BaseController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.Work = unitOfWork;
            Mapper = mapper;
        }


    }
}
