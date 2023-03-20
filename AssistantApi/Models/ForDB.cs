namespace AssistantApi.Models
{
    public class ForDB
    {
        public int _id{ get; set; } 
        public string ?desktopId{ get; set; }
        public string ?lastSeenTime{ get; set; }

        public ForDB(string? lastSeenTime, string? desktopId)
        {
            Random rand = new Random(1000);
            _id = rand.Next(100);
            this.lastSeenTime = lastSeenTime;
            this.desktopId = desktopId;
        }
    }
}
