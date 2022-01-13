using CSharpFunctionalExtensions;
using System.Threading;
using System.Threading.Tasks;

namespace Catalogo.Domain.Produtos.Respositorios
{
    public interface IProdutoRespositorio
    {
        Task<Result> Salvar(Produto produto, CancellationToken cancellationToken);
    }
}
