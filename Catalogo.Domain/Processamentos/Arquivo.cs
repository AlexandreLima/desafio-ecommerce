using Catalogo.Domain.Produtos;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Catalogo.Domain.Processamentos
{
    public abstract class Arquivo
    {
        protected readonly Stream stream;

        public Arquivo(Stream stream, string nome)
        {
            this.stream = stream ?? throw new System.ArgumentNullException(nameof(stream));
            Nome = nome;
        }

        public string Nome { get; private set; }

        public abstract Task<Result<IList<Produto>>> GerarProdutos();
    }
}
