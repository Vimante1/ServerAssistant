using AssistantApi.Hubs;
using AssistantApi.Models;
using AssistantApi.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace AssistantApi.Controllers
{
    [ApiController]
    [Route("/")]
    public class AssistantController : ControllerBase
    {
        IHubContext<AutentificationHub> hubContext;

        public AssistantController(IHubContext<AutentificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] JsonElement Request)
        {

            DateTimeOffset now = DateTimeOffset.UtcNow;
            string Time = now.ToString("yyyy-MM-ddTHH:mm:ssZ");
            RequestData Data = new RequestData(Request);
            Console.WriteLine(Data);
            Console.WriteLine(Time);

            DBMongo dBMongo = new DBMongo();

            if (Data.Handler == "Autentification")
            {
                var ResponseForAssistant = "{\r\n\t\"prompt\": {\r\n\t\t\"override\": false,\r\n\t\t\"firstSimple\": {\r\n\t\t\t\"speech\": \"" + Time + ".\",\r\n\t\t\t\"text\": \"\"\r\n\t\t}\r\n\t}\r\n}";
                return Ok(ResponseForAssistant);
            }
            else
            {
                var DesktopId = dBMongo.GetDesktopIdByLastSeenTime(Data.Time, Time);
                if (DesktopId != null)
                {
                    try
                    {
                        hubContext.Clients.Client(DesktopId).SendAsync("Replace", Time);
                        hubContext.Clients.Client(DesktopId).SendAsync("Send", Data.Handler);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Помилка авторизації чі якась хуйня\n" + ex);
                    }
                }
            }
            return Ok();
        }
    }
}