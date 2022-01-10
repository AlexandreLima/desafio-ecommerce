using Catalogo.Domain.Produtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Catalogo.Domain.Processamentos
{
    public class Processamento
    {
        private Processamento()
        {
            Id = new Guid();
        }

        public Guid Id { get; private set; }
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
}
