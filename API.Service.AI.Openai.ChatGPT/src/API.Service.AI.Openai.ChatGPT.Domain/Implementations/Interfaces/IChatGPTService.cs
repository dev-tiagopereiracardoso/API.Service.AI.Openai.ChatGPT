using API.Service.AI.Openai.ChatGPT.Models.Input;
using API.Service.AI.Openai.ChatGPT.Models.Output;

namespace API.Service.AI.Openai.ChatGPT.Domain.Implementations.Interfaces
{
    public interface IChatGPTService
    {
        Task<EntitySelectOutput<T>> Question<T>(ChatGPTQuestionInput Obj);
    }
}