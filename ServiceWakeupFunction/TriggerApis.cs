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

        //string apiUrl = "https://your-api-url.com/trigger"; // Replace with your actual API URL
        List<string> apiUrlList = new List<string>()
        {
            "https://fastreadingapi.azurewebsites.net/api/Test/WakeupCall?from=AzureFunctions",
            "https://cornyapi.azurewebsites.net/api/Test/WakeupCall?from=AzureFunctions"
            
        };
        try
        {
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            if (DateTime.UtcNow.Hour == 16 && DateTime.UtcNow.Minute == 0)
            {
                HttpResponseMessage response = await httpClient.GetAsync("https://okutioapi.azurewebsites.net/api/Notification/SendType1Notifications?timezone=tr");

                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation($"API triggered successfully. Status Code: {response.StatusCode}");
                }
                else
                {
                    logger.LogWarning($"API trigger failed. Status Code: {response.StatusCode}");
                }
            }
            /*else if (DateTime.UtcNow.Hour == 11 && DateTime.UtcNow.Minute == 0)
            {
                HttpResponseMessage response = await httpClient.GetAsync("https://okutioapi.azurewebsites.net/api/Notification/SendType2Notifications?timezone=tr");

                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation($"API triggered successfully. Status Code: {response.StatusCode}");
                }
                else
                {
                    logger.LogWarning($"API trigger failed. Status Code: {response.StatusCode}");
                }
            }*/
            else if (easternTime.Hour == 19 && easternTime.Minute == 0)
            {
                HttpResponseMessage response = await httpClient.GetAsync("https://okutioapi.azurewebsites.net/api/Notification/SendType1Notifications?timezone=us");

                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation($"API triggered successfully. Status Code: {response.StatusCode}");
                }
                else
                {
                    logger.LogWarning($"API trigger failed. Status Code: {response.StatusCode}");
                }
            }
            /*else if (easternTime.Hour == 14 && easternTime.Minute == 0)
            {
                HttpResponseMessage response = await httpClient.GetAsync("https://okutioapi.azurewebsites.net/api/Notification/SendType2Notifications?timezone=us");

                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation($"API triggered successfully. Status Code: {response.StatusCode}");
                }
                else
                {
                    logger.LogWarning($"API trigger failed. Status Code: {response.StatusCode}");
                }
            }*/
            foreach (var apiUrl in apiUrlList)
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
        }
        catch (Exception ex)
        {
            logger.LogError($"Error triggering API: {ex.Message}");
        }
    }
    
    [Function("TriggerApiEveryTwoMinutes")]
    public async Task RunEveryTwoMinutes([TimerTrigger("0 */2 * * * *")] TimerInfo timer, FunctionContext context)
    {
        var logger = context.GetLogger("TriggerApiEveryTwoMinutes");
        logger.LogInformation($"Function executed at: {DateTime.Now}");

        string apiUrl = "https://fastreadingapi.azurewebsites.net/api/Activity/PeriodicTeacherActivityCheck"; // Replace with your actual API URL

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
