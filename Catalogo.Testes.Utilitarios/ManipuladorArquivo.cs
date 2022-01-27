using System;
using System.IO;

namespace Catalogo.Testes.Utilitarios
{
    public class ManipuladorArquivo
    {
        public static Stream MontarArquivo() =>
         File.OpenRead($"{Environment.CurrentDirectory}/products.csv");
        
    }
}
