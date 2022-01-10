using CSharpFunctionalExtensions;

namespace Catalogo.Domain.Produtos
{
    public class Preco
    {
        private Preco(double valor) { }

        public double Valor { get; set; }

        public static Result<Preco> Criar(double preco)
        {
            if (double.IsPositiveInfinity(preco))
                return Result.Failure<Preco>("Preço do produto deve ser positivo.");

            return new Preco(preco);
        }
    }
}
