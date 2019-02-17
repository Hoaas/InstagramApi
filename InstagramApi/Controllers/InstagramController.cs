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
                .Select(a =>
                {
                    var message = $"{a.User.Fullname} ({a.User.Username}): ";

                    if (!string.IsNullOrWhiteSpace(a.Text)) message += $"{a.Text.Trim()} ";

                    if (a.Urls != null && a.Urls.Any()) message += string.Join(" ", a.Urls);

                    return message.Trim();
                });
        }
    }
}
