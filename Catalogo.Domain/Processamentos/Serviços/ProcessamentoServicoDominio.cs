using Catalogo.Domain.Processamentos.Repositorios;
using Catalogo.Domain.Produtos.Respositorios;
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
        private readonly IProdutoRespositorio produtoRespositorio;

        public ProcessamentoServicoDominio(ConcurrentQueue<Processamento> processamentos, SemaphoreSlim semaphoreSlim, IProcessamentoRespositorio processamentoRepositorio, IProdutoRespositorio produtoRespositorio)
        {
            this.processamentos = processamentos;
            this.semaphoreSlim = semaphoreSlim;
            this.processamentoRepositorio = processamentoRepositorio;
            this.produtoRespositorio = produtoRespositorio;
        }

        public async Task<Result<Guid>> ProcessarArquivo(FileStream fileStream, CancellationToken cancellationToken)
        {
            Processamento processamento = new Processamento(fileStream);
            await processamentoRepositorio.Salvar(processamento, cancellationToken);

            Task.Run(async () => 
            {
                processamentos.Enqueue(processamento);
                await ExecutarEnfileiramento(cancellationToken);
            });

            return processamento.Id;
        }

        private async Task ExecutarEnfileiramento(CancellationToken cancellationToken) 
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
                        var produtosResult = await processamento.Iniciar();
                        await processamentoRepositorio.Salvar(processamento, cancellationToken);

                        if (!produtosResult.IsFailure)
                        {
                            ParallelOptions parallelOptions = 
                                new ParallelOptions 
                                { 
                                    CancellationToken = new CancellationTokenSource(new TimeSpan(0, 3, 0)).Token,
                                    MaxDegreeOfParallelism = 4
                                };
                            
                            Parallel.ForEach(produtosResult.Value, parallelOptions, async (prod) =>
                            {
                                try
                                {
                                    await produtoRespositorio.Salvar(prod, parallelOptions.CancellationToken);
                                }
                                catch (Exception e)
                                {
                                    // TODO: Log caso dê algo errado no paralelismo
                                }
                            });

                            processamento.Finalizar();
                            await processamentoRepositorio.Salvar(processamento, cancellationToken);
                        }
                        else 
                        { }
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

        public async Task<Result> SalvarProcessamento(Processamento processamento, CancellationToken cancellationToken) 
        {
            return await processamentoRepositorio.Salvar(processamento, cancellationToken);
        }

        public async Task<Result> AlterarProcessamento(Processamento processamento, CancellationToken cancellationToken)
        {
            return await processamentoRepositorio.Alterar(processamento, cancellationToken);
        }
    }
}
