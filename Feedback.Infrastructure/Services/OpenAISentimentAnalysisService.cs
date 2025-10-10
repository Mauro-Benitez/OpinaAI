using Feedback.Application.Interfaces.Services;
using Feedback.Application.Models;
using Feedback.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using System.Text.Json;

namespace Feedback.Infrastructure.Services
{
    public class OpenAISentimentAnalysisService : ISentimentAnalysisService
    {
        private readonly ChatClient _chatClient;
        private readonly ILogger<OpenAISentimentAnalysisService> _logger;
        const string INSTRUCTIONS = @"
                Você é um especialista em análise de feedback de clientes.
                Sua tarefa é analisar o comentário de um cliente e retornar um JSON com os campos 'sentiment' e 'topics'.
                - 'sentiment': classifique estritamente como 'Positivo', 'Neutro' ou 'Negativo'.
                - 'topics': extraia de 1 a 3 conceitos ou assuntos principais mencionados. Os tópicos devem ser concisos e representar a causa raiz do comentário, não apenas palavras-chave genéricas.

                Aqui está um exemplo: 
                O usuário enviou o seguinte feedback : A interface do app é linda e fácil de usar, mas a entrega do meu pedido demorou demais. 
                Você responderia deste jeito: { \""sentiment\"": \""Neutro\"", \""topics\"": [\""Interface do App\"", \""Tempo de Entrega\""] }";
                    

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

        public async Task<FeedbackAnalysisResult> AnalizeTextAsync(string text)
        {

            var defaulResult = new FeedbackAnalysisResult { Sentiment = Sentiment.NotAnalyzed };

            if(string.IsNullOrWhiteSpace(text))
            {
                return defaulResult;

            }          

            var messages = new ChatMessage[]
            {
                new SystemChatMessage(INSTRUCTIONS),
                new UserChatMessage(text)
            };

            try
            {
                var responseOptions = new ChatCompletionOptions { ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat() };

                ChatCompletion chatCompletion = await _chatClient.CompleteChatAsync(messages, responseOptions);

                string jsonResponse = chatCompletion.Content[0].Text;

                _logger.LogInformation("Resposta JSON da OpenAI: {JsonResponse}", jsonResponse);

                return ParseOpenAIJsonResponse(jsonResponse);
               
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Falha ao analisar o sentimento com o texto:{Text}", text);
                return defaulResult;
            }
        }

        private FeedbackAnalysisResult ParseOpenAIJsonResponse(string jsonResponse)
        {
            var result = new FeedbackAnalysisResult();

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var jsonDoc = JsonDocument.Parse(jsonResponse);

                // Extrai o sentimento e converte para o Enum
                var sentimentStr = jsonDoc.RootElement.GetProperty("sentiment").GetString();
                result.Sentiment = sentimentStr?.ToLower() switch
                {
                    "positivo" => Sentiment.Positive,
                    "negativo" => Sentiment.Negative,
                     _ => Sentiment.Neutral,
                };

                // Extrai os tópicos
                if (jsonDoc.RootElement.TryGetProperty("topics", out var topicsElement))
                {
                    result.Topics = topicsElement.EnumerateArray().Select(t => t.GetString()).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao parsear o JSON da OpenAI: {JsonResponse}", jsonResponse);
                // Retorna um resultado padrão em caso de falha no parse
                return new FeedbackAnalysisResult { Sentiment = Sentiment.NotAnalyzed };
            }

            return result;
        }


    }
}
