
using CSharpFunctionalExtensions;

namespace Catalogo.Domain.Produtos.Fabrica
{
    public class ProdutoFabrica
    {
        public static Result<Produto> Criar(int codigo, string nome, bool frete, string descricao, decimal preco, string categoria)
        {
            var categoriaResult = Categoria.Criar(categoria);
            var precoResult = Preco.Criar(preco);

            var resultados = Result.Combine(categoriaResult, precoResult);

            if (resultados.IsFailure)
                return Result.Failure<Produto>(resultados.Error);

            return Produto.Criar(codigo, nome, frete, descricao, precoResult.Value, categoriaResult.Value);
        }
    }
}
