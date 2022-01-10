using CSharpFunctionalExtensions;
using System;

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

        public Guid Id { get; private set; }
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

   
}