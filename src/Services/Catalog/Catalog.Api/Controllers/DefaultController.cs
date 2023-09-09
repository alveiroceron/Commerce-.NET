using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [Route("/")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        public string Index()
        {
            return "Product API is Running..";
        }
    }
}
