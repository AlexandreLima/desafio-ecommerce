using Catalogo.Domain.Processamentos.Repositorios;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Catalogo.Domain.Processamentos.Serviços
{
    public class ProcessamentoServicoDominio : IProcessamentoServicoDominio
    {
        private readonly ConcurrentQueue<Processamento> processamentos;
        private readonly SemaphoreSlim semaphoreSlim;
        private readonly IProcessamentoRespositorio processamentoRepositorio;

        public ProcessamentoServicoDominio(ConcurrentQueue<Processamento> processamentos, SemaphoreSlim semaphoreSlim, IProcessamentoRespositorio processamentoRepositorio)
        {
            this.processamentos = processamentos;
            this.semaphoreSlim = semaphoreSlim;
            this.processamentoRepositorio = processamentoRepositorio;
        }

        public async Task<Result<Guid>> ProcessarArquivo(FileStream fileStream)
        {
            Processamento processamento = new Processamento(fileStream);
            await processamentoRepositorio.Salvar(processamento);

            Task.Run(async () => 
            {
                processamentos.Enqueue(processamento);
                await ExecutarEnfileiramento();
            });

            return processamento.Id;
        }

        private async Task ExecutarEnfileiramento() 
        {
            await semaphoreSlim.WaitAsync();

            try
            {
                while (!processamentos.IsEmpty)
                {
                    Processamento processamento;
                    var resultado = processamentos.TryDequeue(out processamento);

                    if (resultado)
                    {
                        var produtos = await processamento.Iniciar();
                        //TODO: Salvar os produtos
                    }
                }
            }
            catch (Exception e) 
            {
                //TODO: Fazer o log em caso de problema
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public async Task<Result> SalvarProcessamento(Processamento processamento) 
        {
            return await processamentoRepositorio.Salvar(processamento);
        }

        public async Task<Result> AlterarProcessamento(Processamento processamento)
        {
            return await processamentoRepositorio.Alterar(processamento);
        }
    }
}
