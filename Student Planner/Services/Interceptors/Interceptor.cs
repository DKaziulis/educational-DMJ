using Serilog;

namespace Student_Planner.Services.Interceptors
{
    public class Interceptor : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Perform interception logic before sending the request
            Log.Information("Intercepting request...");

            // Call the inner handler to process the request
            var response = await base.SendAsync(request, cancellationToken);

            // Perform interception logic after receiving the response
            Log.Information("Intercepted response...");

            return response;
        }
    }
}
