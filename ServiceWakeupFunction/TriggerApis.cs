using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ServiceWakeupFunction;

public class TriggerApis
{
    private static readonly HttpClient httpClient = new HttpClient();

    [Function("TriggerApiEveryMinute")]
    public async Task Run([TimerTrigger("0 * * * * *")] TimerInfo timer, FunctionContext context)
    {
        var logger = context.GetLogger("TriggerApiEveryMinute");
        logger.LogInformation($"Function executed at: {DateTime.Now}");

        string apiUrl = "https://your-api-url.com/trigger"; // Replace with your actual API URL

        try
        {
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation($"API triggered successfully. Status Code: {response.StatusCode}");
            }
            else
            {
                logger.LogWarning($"API trigger failed. Status Code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Error triggering API: {ex.Message}");
        }
    }
}