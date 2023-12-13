using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class LoggingInterceptor : DelegatingHandler
{
    private readonly ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Log information about the incoming request
        _logger.LogInformation($"Incoming request: {request.Method} {request.RequestUri}");

        // Call the inner handler to process the request
        var response = await base.SendAsync(request, cancellationToken);

        // Log information about the outgoing response
        _logger.LogInformation($"Outgoing response: {response.StatusCode}");

        return response;
    }
}