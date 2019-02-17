using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using InstagramService;
using Microsoft.AspNetCore.Mvc;

namespace InstagramApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InstagramController : ControllerBase
    {
        private readonly InstaService _instaService;

        public InstagramController(InstaService instaService)
        {
            _instaService = instaService;
        }

        [HttpGet]
        public async Task<List<InstaActivityDto>> Get()
        {
            var activities = await _instaService.GetAllNewActivity();
            return activities;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Irc()
        {
            var activities = await _instaService.GetAllNewActivity();
            return activities
                .OrderBy(a => a.Timestamp)
                .Select(a => $"{a.User.Fullname} ({a.User.Username}): {a.Text} {string.Join(" ", a.Urls)}");
        }
    }
}
