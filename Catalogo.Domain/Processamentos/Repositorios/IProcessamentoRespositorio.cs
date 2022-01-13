using CSharpFunctionalExtensions;
using System.Threading;
using System.Threading.Tasks;

namespace Catalogo.Domain.Processamentos.Repositorios
{
    public interface IProcessamentoRespositorio
    {
        Task<Result> Salvar(Processamento processamento, CancellationToken cancellationToken);

        Task<Result> Alterar(Processamento processamento, CancellationToken cancellationToken);
    }
}
