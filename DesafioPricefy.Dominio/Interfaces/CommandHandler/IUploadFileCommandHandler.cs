using DesafioPricefy.Dominio.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesafioPricefy.Dominio.Interfaces.CommandHandler
{
    public interface IUploadFileCommandHandler
    {
        Task<Mensagem> UploadFile(IFormFile file);
        Task<IEnumerable<File>> ObterArquivosImportados();
        Task<IEnumerable<File>> ObterArquivosImportadosPorPeriodo(FilterModel model);
    }
}
