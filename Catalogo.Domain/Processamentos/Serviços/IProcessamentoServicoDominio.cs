using CSharpFunctionalExtensions;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Catalogo.Domain.Processamentos.Serviços
{
    public interface IProcessamentoServicoDominio
    {
        public Task<Result<Guid>> ProcessarArquivo(FileStream fileStream, CancellationToken cancellationToken);

    }
}
