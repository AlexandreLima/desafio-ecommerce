using CSharpFunctionalExtensions;

namespace Catalogo.Domain.Produtos
{
    public class Categoria
    {
        private Categoria(string nome)
        {
            Nome = nome;
        }
        public string Nome { get; private set; }

        public static Result<Categoria> Criar(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return Result.Failure<Categoria>("Produto sem nome de categoria.");

            return new Categoria(nome);
        }
    }

}
