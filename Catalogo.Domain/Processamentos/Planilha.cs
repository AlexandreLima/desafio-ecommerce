using Catalogo.Domain.Extensoes;
using Catalogo.Domain.Produtos;
using Catalogo.Domain.Produtos.Fabrica;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Catalogo.Domain.Processamentos
{
    public class Planilha : Arquivo
    {
        public Planilha(Stream stream, string nome)
            : base(stream, nome)
        {

        }

        public override async Task<Result<IList<Produto>>> GerarProdutos()
        {
            var produtos = new List<Produto>();
            var total = 0;

            try
            {
                using (var streaReader = new StreamReader(this.stream))
                {
                    while (!streaReader.EndOfStream)
                    {
                        var linha = await streaReader.ReadLineAsync();

                        // Descartando o cabeçalho
                        if (total == 0)
                        {
                            linha = await streaReader.ReadLineAsync();
                            total++;
                        }

                        if (string.IsNullOrWhiteSpace(linha))
                            return Result.Failure<IList<Produto>>($"Linha {total} em branco.");

                        var colunas = linha.Split(';');

                        if (colunas.Length != MapeamentoColunas.TOTALCOLUNAS)
                            return Result.Failure<IList<Produto>>($"Linha {total} com total de colunas erradas.");

                        var produtoResult = ConverterColunasEmProduto(colunas, total);

                        if (produtoResult.IsFailure)
                        {
                            //TODO: Logar que não foi possível conversão do produto na linha {total}
                        }
                        else
                            produtos.Add(produtoResult.Value);

                        total++;
                    }
                }
            }
            catch (Exception e)
            {
                Result.Failure<IList<Produto>>($"Não foi possível ler o arquivo {this.Nome}. Erro: {e.Message}");
            }

            return Result.Success<IList<Produto>>(produtos);
        }

        private Result<Produto> ConverterColunasEmProduto(string[] colunas, int linhaAtual)
        {
            try
            {
                var codigoProdutoResult = MapeamentoColunas.ConverterColunaEm<int>(colunas[MapeamentoColunas.CODIGOPRODUTO], $"Não foi possível obter código do Produto na linha {linhaAtual}");
                var nome = colunas[MapeamentoColunas.NOME];
                var freteResult = MapeamentoColunas.ConverterColunaEm<bool>(colunas[MapeamentoColunas.TIPOFRETE], $"Não foi possível obter frete do Produto na linha {linhaAtual}");
                var descricao = colunas[MapeamentoColunas.DESCRICAO];
                var precoResult = MapeamentoColunas.ConverterColunaEm<double>(colunas[MapeamentoColunas.PRECO], $"Não foi possível obter o preço do Produto na linha {linhaAtual}");
                var categoria = colunas[MapeamentoColunas.CATEGORIA];

                var erroResult = Result.Combine(";", codigoProdutoResult, freteResult, precoResult);

                if (erroResult.IsFailure)
                   return Result.Failure<Produto>(erroResult.Error);

                return ProdutoFabrica.Criar(codigoProdutoResult.Value, nome, freteResult.Value, descricao, precoResult.Value, categoria);
            }
            catch (Exception e)
            {
                return Result.Failure<Produto>($"Linha {linhaAtual}: Não foi possível converter uma coluna.");
            }
        }

        internal class MapeamentoColunas
        {
            public static readonly int CODIGOPRODUTO = 0;
            public static readonly int NOME = 1;
            public static readonly int TIPOFRETE = 2;
            public static readonly int DESCRICAO = 3;
            public static readonly int PRECO = 4;
            public static readonly int CATEGORIA = 5;
            public static readonly int TOTALCOLUNAS = 6;

            public static Result<T> ConverterColunaEm<T>(string coluna, string mensagemErro)
            {
                try
                {
                    var resultado = coluna.Convert<T>();

                    if (resultado == null)
                        return Result.Failure<T>(mensagemErro);

                    return resultado;

                }
                catch (Exception e)
                {
                    return Result.Failure<T>(mensagemErro);
                }
            }
        }
    }
}
