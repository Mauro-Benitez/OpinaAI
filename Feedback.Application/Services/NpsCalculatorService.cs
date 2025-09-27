using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Application.Services
{
    public class NpsCalculatorService
    {
      
        public double CalculateNps(List<int> scores)
        {
            if(scores == null || !scores.Any())
            {
                return 0;
            }

            double totalScores = scores.Count;

            // Conta quantas notas são de Promotores (maior ou igual a 9)
            double promoterCount = scores.Count(s => s >= 9);

            // Conta quantas notas são de Detratores (menor ou igual a 6)
            double detractorCount = scores.Count(s => s <= 6);

            // Conta quantas notas são de Promotores (maior ou igual a 9)


            // Converte a contagem de promotores em um percentual do total.
            double promoterPecentage = (promoterCount / totalScores) * 100;

            // Converte a contagem de detratores em um percentual do total.
            double detractorPercentage = (detractorCount / totalScores) * 100;

           
            //Cálculo final do NPS
            //Subtrai o percentual de detratores do percentual de promotores.
            return promoterPecentage - detractorPercentage;
        }


    }
}
