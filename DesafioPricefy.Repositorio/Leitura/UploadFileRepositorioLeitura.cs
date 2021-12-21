using Dapper;
using DesafioPricefy.Dominio.Interfaces.Infra;
using DesafioPricefy.Dominio.Interfaces.RepositorioLeitura;
using DesafioPricefy.Dominio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesafioPricefy.Repositorio.Leitura
{
    public class UploadFileRepositorioLeitura : IUploadFileRepositorioLeitura
    {
        private readonly IConnectionHandler connection;

        public UploadFileRepositorioLeitura(IConnectionHandler connection) => this.connection = connection;

        public async Task<File> ValidarArquivoJaImportado(string nomeArquivo)
        {
            using (var conexao = connection.Create())
            {
                return await conexao.QueryFirstOrDefaultAsync<File>(@$"SELECT * FROM arquivo WHERE id = @NomeArquivo;", new { NomeArquivo = nomeArquivo});
            }
        }

        public async Task<FileLine> ValidarRegistroJaImportado(string idRegistro)
        {
            using (var conexao = connection.Create())
            {
                return await conexao.QueryFirstOrDefaultAsync<FileLine>("SELECT * FROM registrosarquivoImportado WHERE id = @Id", new { Id = idRegistro });
            }
        }

        public async Task<IEnumerable<File>> ObterArquivosImportados()
        {
            using (var conexao = connection.Create())
            {
                return await conexao.QueryAsync<File>("SELECT * FROM arquivo");
            }
        }

        public async Task<IEnumerable<File>> ObterArquivosImportadosPorPeriodo(FilterModel model)
        {
            using (var conexao = connection.Create())
            {
                return await conexao.QueryAsync<File>("SELECT * FROM arquivo WHERE dataimportacao BETWEEN @DataInicial AND @DataFinal", model);
            }
        }
    }
}
