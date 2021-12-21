using DesafioPricefy.Dominio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesafioPricefy.Dominio.Interfaces.RepositorioEscrita
{
    public interface IUploadFileRepositorioEscrita
    {
        Task<string> InserirImportacaoArquivo(File file);
        Task<int> InserirRegistrosArquivo(List<FileLine> registros, string idImportacao);
        Task AtualizarQuantidadeDeRegistrosImportados(int quantidade, string idImportacao);
    }
}
