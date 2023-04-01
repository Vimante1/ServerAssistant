using System.Text.Json;

namespace AssistantApi.Models
{
    public class RequestData
    {
        public string HandlerCommand { get; set; }
        public string Params { get; set; } = null;

        public RequestData(JsonElement Request)
        {
            HandlerCommand = Request.GetProperty("handler").GetProperty("name").ToString();
            Params = Request.GetProperty("intent").GetProperty("params").ToString();
        }

        public override string ToString()
        {
            return $"Command: {HandlerCommand}\nParams: {Params}";
        }
    }
}
