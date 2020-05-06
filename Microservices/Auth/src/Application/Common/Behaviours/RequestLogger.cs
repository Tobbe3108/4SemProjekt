using System.Threading;
using System.Threading.Tasks;
using Auth.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Common.Behaviours
{
    public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger _logger;

        public RequestLogger(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userName = _currentUserService.Username ?? string.Empty;

            _logger.LogInformation("Resource Request: {Name} {@UserName} {@Request}",
                requestName, userName, request);

            await Task.CompletedTask;
        }
    }
}