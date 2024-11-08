using API.Service.AI.Openai.ChatGPT.Domain.Implementations.Interfaces;
using API.Service.AI.Openai.ChatGPT.Models.Input;
using API.Service.AI.Openai.ChatGPT.Models.Output;
using ChatGPT.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace API.Service.AI.Openai.ChatGPT.Domain.Implementations.Services
{
    public class ChatGPTService : IChatGPTService
    {
        private readonly ILogger<ChatGPTService> _logger;

        private readonly IConfiguration _configuration;

        public ChatGPTService(
                ILogger<ChatGPTService> logger,
                IConfiguration configuration
            )
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<EntitySelectOutput<T>> Question<T>(ChatGPTQuestionInput Obj)
        {
            var Validation = new EntitySelectOutput<T>();
            var result = new List<QuestionOutput>();

            var openApiKey = _configuration["ApiKeyChatGPT"] ?? "";

            if (String.IsNullOrEmpty(openApiKey))
            {
                _logger.LogError("key not found");

                result.Add(new QuestionOutput()
                {
                    Message = "key not found"
                });

                Validation.StatusCode = HttpStatusCode.NotFound;
                Validation.Items = (List<T>)Convert.ChangeType(result, typeof(List<T>));

                return Validation;
            }

            if (String.IsNullOrEmpty(Obj.Question))
            {
                _logger.LogError("fill in the question");

                result.Add(new QuestionOutput()
                {
                    Message = "fill in the question"
                });

                Validation.StatusCode = HttpStatusCode.NotFound;
                Validation.Items = (List<T>)Convert.ChangeType(result, typeof(List<T>));

                return Validation;
            }

            var answer = "";

            if (String.IsNullOrEmpty(Obj.UrlImage))
            {
                var openai = new ChatGpt(openApiKey);
                answer = await openai.Ask(Obj.Question);
            }
            else
            {
                answer = SendMessage(openApiKey, Obj.UrlImage, Obj.Question);
            }

            if (answer == null)
            {
                _logger.LogError("unable to call chat gpt");

                result.Add(new QuestionOutput()
                {
                    Message = "unable to call chat gpt"
                });

                Validation.StatusCode = HttpStatusCode.NotFound;
                Validation.Items = (List<T>)Convert.ChangeType(result, typeof(List<T>));

                return Validation;
            }

            result.Add(new QuestionOutput()
            {
                Message = "",
                Answer = answer
            });

            Validation.StatusCode = HttpStatusCode.OK;
            Validation.Items = (List<T>)Convert.ChangeType(result, typeof(List<T>));

            return Validation;
        }

        private string SendMessage(string OpenApiKey, string UrlImage, string Question)
        {
            var _client = new RestClient("https://api.openai.com/v1/chat/completions");

            var request = new RestRequest("", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {OpenApiKey}");

            var requestBody = "{ \"model\": \"gpt-4o-mini\", \"messages\": [{ \"role\": \"user\", \"content\": [ { \"type\": \"text\", \"text\": \"" + Question + "\" }, { \"type\": \"image_url\", \"image_url\": { \"url\": \"" + UrlImage + "\" } }] }], \"max_tokens\": 300 }";

            request.AddJsonBody(requestBody);

            var response = _client.Execute(request);

            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response.Content ?? string.Empty);

            return jsonResponse?.choices[0]?.message?.content?.ToString()?.Trim() ?? string.Empty;
        }
    }
}