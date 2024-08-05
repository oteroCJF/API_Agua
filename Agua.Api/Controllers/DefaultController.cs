using Microsoft.AspNetCore.Mvc;

namespace Agua.Api.Controllers
{
    [ApiController]
    [Route("/")]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        public string Index()
        {
            return "APi Agua en línea";
        }
    }
}
