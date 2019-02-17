using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;
using InstagramService;
using Microsoft.AspNetCore.Mvc;

namespace InstagramApi.Controllers
{
    [Route("api/[controller]")]
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
    }
}
