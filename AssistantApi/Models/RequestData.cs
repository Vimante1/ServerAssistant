using Newtonsoft.Json;
using System.Text.Json;

namespace AssistantApi.Models
{
    public class RequestData
    {
        public string Handler { get; set; }

        public string Time { get; set; }

        public RequestData(JsonElement request)
        {
            Time = request.GetProperty("user").GetProperty("lastSeenTime").GetString()!;
            Handler = request.GetProperty("handler").GetProperty("name").GetString()!;
        }
        public override string ToString()
        {
            return $"Command: {Handler}\nId: {Time}";
        }
    }
}
