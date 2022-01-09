using CSharpFunctionalExtensions;

namespace Catalogo.Domain.Produtos
{
    public class Produto
    {
        private Produto(int codigo, string nome, bool frete, string descricao, Preco preco, Categoria categoria)
        {
            Codigo = codigo;
            Nome = nome;
            Frete = frete;
            Preco = preco;
            Descricao = descricao;
            Categoria = categoria;
        }

        public int Codigo { get; private set; }
        public string Nome { get; private set; }
        public bool Frete { get; private set; }
        public string Descricao { get; private set; }
        public Preco Preco { get; private set; }
        public Categoria Categoria { get; private set; }

        public static Result<Produto> Criar(int codigo, string nome, bool frete, string descricao, Preco preco, Categoria categoria)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return Result.Failure<Produto>("Produto com nome nulo.");

            if (string.IsNullOrWhiteSpace(descricao))
                return Result.Failure<Produto>("Produto com decrição nula.");

            return new Produto(codigo, nome, frete, descricao, preco, categoria);
        }
    }

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

    public class ProdutoFabrica
    {
        public static Result<Produto> Criar(int codigo, string nome, bool frete, string descricao, double preco, string categoria)
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