using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("/")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        public string Index()
        {
            return "Identity API is Running..";
        }
    }
}
