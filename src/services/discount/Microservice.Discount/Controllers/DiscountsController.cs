using Microservice.Discount.Services;
using Microservice.Shared;
using Microservice.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservice.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : CustomBaseController
    {
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public DiscountsController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            _discountService = discountService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => CreateActionResultInstance(await _discountService.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => CreateActionResultInstance(await _discountService.GetById(id));

        [HttpPost]
        public async Task<IActionResult> Save(Models.Discount discount) => CreateActionResultInstance(await _discountService.Save(discount));
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => CreateActionResultInstance(await _discountService.Delete(id));
        [HttpGet]
        [Route("/api/[controller]/GetByCodeAndUserId/{code}")]
        public async Task<IActionResult> GetByCodeAndUserId(string code) => CreateActionResultInstance(await _discountService.GetByCodeAndUserId(code, _sharedIdentityService.GetUserId));
        [HttpPut]
        public async Task<IActionResult> Update(Models.Discount discount) => CreateActionResultInstance(await _discountService.Update(discount));
    }
}
