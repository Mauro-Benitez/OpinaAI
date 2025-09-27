using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Application.Interfaces
{
    public interface IUnitOfWork
    { 
        /// <summary>
      /// Salva todas as mudanças feitas no contexto para o banco de dados.
      /// </summary>
      /// <returns>O número de registros afetados no banco de dados.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
