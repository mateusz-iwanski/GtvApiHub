using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GtvApiHub.Helpers
{
    /// <summary>
    /// <c>ValidationErrorResponse</c> class.
    /// </summary>
    /// <remarks>
    /// Used to deserialize data from HTTP content (as a string) with validation errors.
    /// </remarks>
    public class ValidationErrorResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("errors")]
        public Dictionary<string, string[]> Errors { get; set; }

        [JsonPropertyName("traceId")]
        public string TraceId { get; set; }
    }
}
