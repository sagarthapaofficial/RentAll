using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RentAll.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string>Index()
        {
            return "Server started";
        }
    }
}
