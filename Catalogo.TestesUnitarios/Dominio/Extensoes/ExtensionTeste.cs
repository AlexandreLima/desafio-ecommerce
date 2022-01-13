using System;
using Xunit;
using Catalogo.Domain.Extensoes;
using FluentAssertions;

namespace Catalogo.TestesUnitarios.Dominio.Extensoes
{
    public class ExtensionTeste
    {
        [Theory(DisplayName = "Deve alterar tipo de objeto")]
        [InlineData("1", typeof(int))]
        [InlineData("1.90", typeof(double))]
        public void TestarConversaoDeTipo(object objetoASerAlterado, Type type) 
        {
            objetoASerAlterado.Convert(type).Should().BeOfType(type);
        }

        [Theory(DisplayName = "Deve dar erro ao alterar uma string em boleano")]
        [InlineData("1", typeof(bool))]
        [InlineData("Ç", typeof(int))]
        public void ErroTestarConversaoDeTipo(object objetoASerAlterado, Type type)
        {
            Assert.Throws<FormatException>(() => objetoASerAlterado.Convert(type));
        }
    }
}
