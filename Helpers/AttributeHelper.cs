using GtvApiHub.Helpers;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public static class AttributeHelper
{
    /// <summary>
    /// <c>DeserializeWebApiResponseAsync</c> is used to deserialize responses from the API
    /// <example>
    /// <code>
    /// [DeserializeResponse]
    /// CreatePLById(customerId);
    /// ...
    /// var response = await customerService.CreatePLById(customerId);
    /// var methodInfo = typeof(CustomerGT).GetMethod("CreatePLById");
    /// await AttributeHelper.DeserializeWebApiResponseAsync(methodInfo, response);
    /// </code>
    /// </example>
    /// </summary>
    /// <remarks>
    /// Useful for displaying process details and errors to the client
    /// </remarks>
    public static async Task DeserializeWebApiResponseAsync<T>(string methodName, HttpResponseMessage response)
    {
        var method = typeof(T).GetMethod(methodName);

        if (method.GetCustomAttribute<DeserializeWebApiResponseAttribute>() != null)
        {
            var processId = Guid.NewGuid().ToString("N").Substring(0, 8);

            var url = response.RequestMessage.RequestUri.ToString();

            Console.WriteLine($"INFO|{processId}|Check request method: {method.Name}");
            Console.WriteLine($"INFO|{processId}|URL: {url}");

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var validationError = JsonSerializer.Deserialize<ValidationErrorResponse>(content);

                Console.WriteLine($"ERROR|{processId}|Validation Error method: {method.Name}");

                foreach (var error in validationError.Errors)
                {
                    Console.WriteLine($"ERROR|{processId}|Validation Error key: {error.Key}");

                    foreach (var value in error.Value)
                    {
                        Console.WriteLine($"ERROR|{processId}|Validation Error value: {value}");
                    }
                }
            }
        }
    }

    public static async Task DeserializeWebApiResponseAsync<T>(string methodName, IEnumerable<HttpResponseMessage> response)
    {
        foreach (var httpResponseMessage in response)
        {
            await DeserializeWebApiResponseAsync<T>(methodName, httpResponseMessage);
        }
    }
}