using CSharpFunctionalExtensions;

namespace Catalogo.Domain.Produtos
{
    public class Preco
    {
        private Preco(decimal valor) 
        {
            Valor = valor;
        }

        public decimal Valor { get; set; }

        public static Result<Preco> Criar(decimal preco)
        {
            if (decimal.Zero > preco)
                return Result.Failure<Preco>("Preço do produto deve ser positivo.");

            return new Preco(preco);
        }
    }
}
