using DesafioPricefy.Dominio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesafioPricefy.Dominio.Interfaces.RepositorioLeitura
{
    public interface IUploadFileRepositorioLeitura
    {
        Task<File> ValidarArquivoJaImportado(string nomeArquivo);
        Task<FileLine> ValidarRegistroJaImportado(string idRegistro);
        Task<IEnumerable<File>> ObterArquivosImportados();
        Task<IEnumerable<File>> ObterArquivosImportadosPorPeriodo(FilterModel model);
    }
}
