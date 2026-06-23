using CompanyName.DDD.API.Extensions;
using CompanyName.DDD.Domain.SaleAggregate.Application.DTO;
using CompanyName.DDD.Domain.SaleAggregate.Application.Interface;
using Microsoft.AspNetCore.Mvc;
namespace CompanyName.DDD.API.Controllers.Sale
{
    [ApiController]
    [Route("[controller]")]
    public class SaleController(ISaleAggregate sale) : Controller
    {
        [HttpPost]
        public IActionResult Create(SaleCommand command)
        {
            var userData = new UserDataRetriever(HttpContext.Request.Headers.Authorization.ToString());
            var response = sale.Create(command, userData.UserCode);
            return Ok(response);
        }
    }
}