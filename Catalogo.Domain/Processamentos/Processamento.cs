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

        private Arquivo arquivo;
        private readonly FileStream stream;

        public Processamento(FileStream stream)
        {
            Id = new Guid();
            FaseProcessamento = FaseProcessamentoEnum.NaoIniciado;
            this.stream = stream;
        }

        public Guid Id { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public FaseProcessamentoEnum FaseProcessamento { get; private set; }

        public async Task<Result<IList<Produto>>> Iniciar()
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
