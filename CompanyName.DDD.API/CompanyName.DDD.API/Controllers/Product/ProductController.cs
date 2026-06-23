using CompanyName.DDD.Domain.ProductAggregate.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CompanyName.DDD.API.Controllers.Product
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController(IProductAggregate product) : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("GetBySaleFilter/{productName}/{taxDescriprion}")]
        public IActionResult GetBySaleFilter(string productName, string taxDescriprion)
        {
            var list = product.GetBySaleFilter(productName, taxDescriprion);
            return Ok(list);
        }
    }
}