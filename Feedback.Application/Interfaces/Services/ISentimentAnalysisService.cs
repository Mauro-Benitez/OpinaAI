using Feedback.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Application.Interfaces.Services
{
    public interface ISentimentAnalysisService
    {

        /// <summary>
        /// Analisa um texto e retorna a classificação de sentimento.
        /// </summary>
        /// <param name="text">O texto a ser analisado.</param>
        /// <returns>O enum Sentiment correspondente.</returns>
        Task<Sentiment> AnalizeTextAsync(string text);


    }
}
