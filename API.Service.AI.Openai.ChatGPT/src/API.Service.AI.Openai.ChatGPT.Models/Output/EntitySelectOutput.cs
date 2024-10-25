using System.Net;

namespace API.Service.AI.Openai.ChatGPT.Models.Output
{
    public class EntitySelectOutput<T>
    {
        public HttpStatusCode StatusCode { get; set; }

        public List<T>? Items { get; set; }
    }
}
