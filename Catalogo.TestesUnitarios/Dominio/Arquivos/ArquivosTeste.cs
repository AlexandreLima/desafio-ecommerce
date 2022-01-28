using Catalogo.Domain.Processamentos;
using Catalogo.Testes.Utilitarios;
using FluentAssertions;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Catalogo.TestesUnitarios.Dominio.Arquivos
{
    public class ArquivosTeste
    {
        FileStream arquivoCsv;

        public ArquivosTeste()
        {
            arquivoCsv = (FileStream)ManipuladorArquivo.MontarArquivo();
        }

        [Fact(DisplayName = "Cria Arquivo com sucesso")]
        public void CriarArquivoComSucesso() 
        {
            var arquivo = new Planilha(arquivoCsv, arquivoCsv.Name);
            arquivo.Nome.Should().Be(arquivoCsv.Name);
        }

        [Fact(DisplayName = "Gera Produtos com sucesso")]
        public async Task GerarArquivos()
        {
            var arquivo = new Planilha(arquivoCsv, arquivoCsv.Name);
            var resultado = await arquivo.GerarProdutos();

            resultado.IsSuccess.Should().BeTrue();
            resultado.Value.Count.Should().Be(6);
        }

        [Fact(DisplayName = "Arquivo não pode ser lido.")]
        public async Task GerarArquivosVazio()
        {
            var arquivo = new Planilha(FileStream.Null, arquivoCsv.Name);
            var resultado = await arquivo.GerarProdutos();

            resultado.IsSuccess.Should().BeFalse();
            resultado.Error.Should().Be("Arquivo não pode ser lido.");
        }
    }
}
