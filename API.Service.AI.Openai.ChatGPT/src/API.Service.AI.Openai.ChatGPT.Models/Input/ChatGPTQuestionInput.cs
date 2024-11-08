namespace API.Service.AI.Openai.ChatGPT.Models.Input
{
    public class ChatGPTQuestionInput
    {
        public string UrlImage { set; get; } = String.Empty;

        public string Question { get; set; }
    }
}
