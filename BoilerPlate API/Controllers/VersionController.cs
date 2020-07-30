using Microsoft.AspNetCore.Mvc;
namespace BoilerPlate_API.Controllers
{
    [Route("api/[controller]")]
    public class VersionController : ControllerBase
    {
        //GET Endpoint for checking the current version of the API
        [HttpGet]
        public ActionResult<string> GetVersion()
        {
            return "20200730.1";
        }
    }
}
