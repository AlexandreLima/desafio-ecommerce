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
        private Processamento()
        {
            Id = new Guid();
            FaseProcessamento = FaseProcessamentoEnum.NaoIniciado;
        }

        public Guid Id { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public FaseProcessamentoEnum FaseProcessamento { get; private set; }
        private Arquivo arquivo;
        public async Task<Result<IList<Produto>>> Iniciar(FileStream stream)
        {
            DataInicio = DateTime.Now;
            arquivo = new Planilha(stream, stream.Name);
            FaseProcessamento = FaseProcessamentoEnum.EmProcesso;

            return  await arquivo.GerarProdutos();
        }
        public Result Finalizar() 
        {
            if (FaseProcessamento == FaseProcessamentoEnum.Finalizado)
                return Result.Failure("Processamento do arquivo já foi finalizado anteriormente.");

            DataFim = DateTime.Now;
            FaseProcessamento = FaseProcessamentoEnum.Finalizado;

            return Result.Success();
        } 
    } 
}
