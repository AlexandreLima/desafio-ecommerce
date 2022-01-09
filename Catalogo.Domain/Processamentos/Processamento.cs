using Catalogo.Domain.Produtos;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Catalogo.Domain.Processamentos
{
    public class Processamento
    {
        public Processamento()
        {

        }
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }

        private Arquivo arquivo;

        public async Task<IList<Produto>> Iniciar(FileStream stream)
        {
            //DataInicio = DateTime.Now;
            //arquivo = new Planilha(stream, stream.Name);

            throw new NotImplementedException();

            //return await arquivo.GerarProdutos();
        }
    }

    public enum ProcessamentoEnum
    {
        NaoIniciado,
        EmProcesso,
        Finalizado
    }

    public abstract class Arquivo
    {
        protected readonly Stream stream;

        public Arquivo(Stream stream, string nome)
        {
            this.stream = stream ?? throw new System.ArgumentNullException(nameof(stream));
        }

        public string Nome { get; private set; }

        public abstract Task<Result<IList<Produto>>> GerarProdutos();
    }

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

                        var produtoResult = ConverterColunasEmProduto(colunas);

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

        private Result<Produto> ConverterColunasEmProduto(string[] colunas)
        {
            throw new NotImplementedException();
            //var produtoResult = ProdutoFabrica.Criar();
            //colunas[MapeamentoColunas.CODIGOPRODUTO]
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

            public static Result<T> ConverterColunaEm<T>(string[] colunas, int indice, string mensagemErro)
            {
                try
                {
                    var resultado = colunas[indice].Convert<T>();

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

    public static class ConversionExtensions
    {
        public static object Convert(this object value, Type t)
        {
            Type underlyingType = Nullable.GetUnderlyingType(t);

            if (underlyingType != null && value == null)
            {
                return null;
            }
            Type basetype = underlyingType == null ? t : underlyingType;
            return System.Convert.ChangeType(value, basetype);
        }

        public static T Convert<T>(this object value)
        {
            return (T)value.Convert(typeof(T));
        }
    }
}
