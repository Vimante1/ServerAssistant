using AssistantApi.Hubs;
using AssistantApi.Models;
using AssistantApi.services;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson.IO;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AssistantApi.Controllers
{
    [ApiController]
    [Route("/assistant/endpoint")]
    public class AssistantController : ControllerBase
    {

        private string ResponseForAssistant = "{\r\n\t\"prompt\": {\r\n\t\t\"override\": false,\r\n\t\t\"firstSimple\": {\r\n\t\t\t\"speech\": \"" + /*Time*/ ".\",\r\n\t\t\t\"text\": \"\"\r\n\t\t}\r\n\t}\r\n}";

        private DBMongo dBMongo = new DBMongo();
        IHubContext<AutentificationHub> hubContext;

        public AssistantController(IHubContext<AutentificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromHeader(Name = "Authorization")] string Token, [FromBody] JsonElement Request)
        {

            RequestData data = new RequestData(Request);
            var Email = new JwtObject(Token).Email;

            if (data.HandlerCommand == "CreateUser")
            {

                ForDB forDB = new ForDB(Email);
                dBMongo.Add(forDB);
            }
            else
            {
                if (dBMongo.ClientIsConnected(Email, out string desktopId))
                {
                    try
                    {
                        hubContext.Clients.Client(desktopId).SendAsync("Send", data.HandlerCommand);
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine("Помилка авторизації чі якась хуйня\n" + ex); 
                    }
                }
            }
            return Ok();
        }


        public class JwtObject
        {
            public string Email { get; set; }

            public JwtObject(string Token)
            {
                string[] tokenParts = Token.Split('.');
                var body = tokenParts[1];
                var json = Encoding.UTF8.GetString(Base64Url.Decode(body));
                Email = Newtonsoft.Json.JsonConvert.DeserializeObject<em>(json).email;

            }
            private class em
            {
                public string? email { get; set; }
            }
        }

    }
}





//Console.WriteLine(Request);
//DateTimeOffset now = DateTimeOffset.UtcNow;
//string Time = now.ToString("yyyy-MM-ddTHH:mm:ssZ");
//RequestData Data = new RequestData(Request);
//Console.WriteLine(Data);
//Console.WriteLine(Time);

//DBMongo dBMongo = new DBMongo();

//if (Data.Handler == "Autentification")
//{
//    return Ok(ResponseForAssistant);
//}
//else
//{
//    var DesktopId = dBMongo.GetDesktopIdByLastSeenTime(Data.Time, Time);
//    if (DesktopId != null)
//    {
//        try
//        {
//            hubContext.Clients.Client(DesktopId).SendAsync("Replace", Time);
//            hubContext.Clients.Client(DesktopId).SendAsync("Send", Data.Handler);
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine("Помилка авторизації чі якась хуйня\n" + ex);
//        }
//    }
//}
//return Ok();