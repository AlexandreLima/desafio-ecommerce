using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace Catalogo.Domain.Processamentos.Repositorios
{
    public interface IProcessamentoRespositorio
    {
        Task<Result> Salvar(Processamento processamento);

        Task<Result> Alterar(Processamento processamento);
    }
}
