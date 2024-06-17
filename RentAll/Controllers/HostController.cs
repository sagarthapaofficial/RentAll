using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RentAll.Controllers
{
    //onl;y authorize user has access to this controller
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HostController : ControllerBase
    {
        [HttpGet("getHosts")]
        public IActionResult Hosts()
        {
            return Ok( new JsonResult( new {message ="Only authorize users can view hosts"}) );
        }
    }
}
