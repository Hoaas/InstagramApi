using System.Threading.Tasks;
using InstagramService;
using Microsoft.AspNetCore.Mvc;

namespace InstagramApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly InstaService _instaService;

        public ValuesController(InstaService instaService)
        {
            _instaService = instaService;
        }

        // GET api/values
        [HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        public async Task<IActionResult> Get()
        {
            var activities = await _instaService.GetAllNewActivity();

            return Ok(activities);
        }
    }
}
