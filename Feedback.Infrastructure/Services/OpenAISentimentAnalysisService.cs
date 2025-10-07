using Feedback.Application.Interfaces.Services;
using Feedback.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;

namespace Feedback.Infrastructure.Services
{
    public class OpenAISentimentAnalysisService : ISentimentAnalysisService
    {
        private readonly ChatClient _chatClient;
        private readonly ILogger<OpenAISentimentAnalysisService> _logger;
        const string INSTRUCTIONS = "Você é um especialista em análise de sentimento. Analise o comentário de um cliente e classifique-o estritamente como Positivo, Neutro ou Negativo. Responda com apenas uma dessas três palavras.";
        public OpenAISentimentAnalysisService(IConfiguration configuration, ILogger<OpenAISentimentAnalysisService> logger)
        {
            var apiKey = configuration["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException(nameof(apiKey), "A chave da API da OpenAI não foi configurada.");
            }

            _chatClient = new ChatClient("gpt-3.5-turbo", apiKey);
            _logger = logger;
            
        }

        public async Task<Sentiment> AnalizeTextAsync(string text)
        {
            if(string.IsNullOrWhiteSpace(text))
            {
                return Sentiment.Neutral;
            }          

            var messages = new ChatMessage[]
            {
                new SystemChatMessage(INSTRUCTIONS),
                new UserChatMessage(text)
            };

            try
            {

                ChatCompletion chatCompletion = await _chatClient.CompleteChatAsync(messages);
                String response = chatCompletion.Content[0].Text.Trim().ToLower();

                _logger.LogInformation("Comentário '{Text}' analisado com o sentimento : {Response}", text, response);
                
                switch (response)
                {
                    case "positivo":
                        return Sentiment.Positive;
                    case "negativo":
                        return Sentiment.Negative;
                    case "neutro":
                    default:
                        return Sentiment.Neutral;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Falha ao analisar o sentimento com o texto:{Text}", text);
                return Sentiment.NotAnalyzed;
            }
        }

       
    }
}
