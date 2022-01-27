using Catalogo.Testes.Utilitarios;
using Xunit;
using Catalogo.Domain.Processamentos;
using System.IO;
using FluentAssertions;

namespace Catalogo.TestesUnitarios.Dominio.Processamentos
{
    public class ProcessamentoTeste
    {
        Stream arquivo;

        public ProcessamentoTeste()
        {
            arquivo = ManipuladorArquivo.MontarArquivo();
        }

        [Fact(DisplayName ="Cria Processamento com sucesso")]
        public void CriarProcessamento() 
        {
            var processamento = new Processamento((FileStream)arquivo);
            processamento.FaseProcessamento.Should().Be(FaseProcessamentoEnum.NaoIniciado);
        }

    }
}
