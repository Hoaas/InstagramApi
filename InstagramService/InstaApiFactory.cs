using Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;

namespace InstagramService
{
    public class InstaApiFactory
    {
        private readonly ILogger _logger;
        private readonly InstagramApiConfig _config;
        private IInstaApi _api;

        public InstaApiFactory(
            ILogger<InstaApiFactory> logger,
            IOptions<InstagramApiConfig> options)
        {
            _logger = logger;
            _config = options.Value;
        }

        public async Task<IInstaApi> Create()
        {
            if (_api != null) return _api;

            var api = InstaApiBuilder.CreateBuilder()
                .SetUser(new UserSessionData
                {
                    UserName = _config.Username,
                    Password = _config.Password
                })
                 //.UseLogger(_logger)
                 .SetRequestDelay(RequestDelay.FromSeconds(2, 2))
                 .Build();

            var status = await api.LoginAsync();
             if (!status.Succeeded) throw new InstaException("Login failed.");

            _api = api;
            return _api;
        }
    }
}
