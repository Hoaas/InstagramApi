using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Common;
using Common.Models;
using InstagramApi.Models;
using InstagramService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace InstagramApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InstagramController : ControllerBase
    {
        private readonly InstaService _instaService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<InstagramController> _logger;
        private readonly IrcConfig _config;

        public InstagramController(
            InstaService instaService,
            IHttpClientFactory httpClientFactory,
            IOptions<IrcConfig> ircConfig,
            ILogger<InstagramController> logger)
        {
            _instaService = instaService;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _config = ircConfig.Value;
        }

        [HttpGet]
        public async Task<List<InstaActivityDto>> Get()
        {
            var activities = await _instaService.GetAllNewActivity();
            return activities;
        }

        [HttpGet]
        public async Task<IActionResult> Irc()
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync(_config.Url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(412, $"Url '{_config.Url}' not available.");
            }

            var activities = await _instaService.GetAllNewActivity();

            foreach (var activity in activities)
            {
                var message = $"{activity.User.Fullname} ({activity.User.Username}): ";

                if (!string.IsNullOrWhiteSpace(activity.Text)) message += $"{activity.Text.Trim()} ";

                if (activity.Urls != null && activity.Urls.Any()) message += string.Join(" ", activity.Urls);

                message = message.Trim();

                var ircDto = new IrcDto
                {
                    Channel = _config.Channel,
                    Message = $"[IG] {message}"
                };

                var json = JsonConvert.SerializeObject(ircDto);

                var postResponse = await client.PostAsync(_config.Url, new StringContent(json));
                if (!postResponse.IsSuccessStatusCode)
                {
                    _logger.LogError($"Post to IRC failed. Got code '{postResponse.StatusCode}' and message: '{await postResponse.Content.ReadAsStringAsync()}'.");
                }
            }

            return Ok();
        }
    }
}
