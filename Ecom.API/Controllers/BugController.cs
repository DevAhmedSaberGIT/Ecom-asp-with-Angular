using AutoMapper;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{

    public class BugController : BaseController
    {
        public BugController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }
        [HttpGet(template:"not-found")]
        public async Task<IActionResult> GetNotFound()
        {
          Category category = await Work.CateroryRepository.GetByIdAsync(100);
          if (category == null) return NotFound();
          return Ok(value: category);         
        }
        [HttpGet(template: "Server-error")]
        public async Task<IActionResult> GetServererror()
        {
            Category category = await Work.CateroryRepository.GetByIdAsync(100);
            if (category == null) return NotFound();
            return Ok(value: category);
        }
        [HttpGet("Bad-request/{id}")]
        public async Task<IActionResult> GetBadRequest(int id) => Ok();

        [HttpGet("Bad-request")]
        public async Task<IActionResult> GetBadRequest1() => Ok();

    }
}
