using CompanyName.DDD.Domain.TaxTypeAggregate.Application.Interface;
using Microsoft.AspNetCore.Mvc;
namespace CompanyName.DDD.API.Controllers.TaxType
{
    [ApiController]
    [Route("[controller]")]
    public class TaxTypeController(ITaxTypeAggregate taxType) : Controller
    {
        [HttpGet]
        public IActionResult GetSimpleResponse()
        {
            var rows = taxType.GetSimpleResponse();
            return Ok(rows);
        }
    }
}