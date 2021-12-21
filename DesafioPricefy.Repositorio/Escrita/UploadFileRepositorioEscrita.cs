using Dapper;
using DesafioPricefy.Dominio.Interfaces.Infra;
using DesafioPricefy.Dominio.Interfaces.RepositorioEscrita;
using DesafioPricefy.Dominio.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesafioPricefy.Repositorio.Escrita
{
    public class UploadFileRepositorioEscrita : IUploadFileRepositorioEscrita
    {
        private readonly IConnectionHandler connection;

        public UploadFileRepositorioEscrita(IConnectionHandler connection) => this.connection = connection;

        public async Task<string> InserirImportacaoArquivo(File file)
        {
            using (var conexao = connection.Create())
            {
                return await conexao.QueryFirstOrDefaultAsync<string>(@"INSERT 
                                                                        INTO arquivo(id, 
                                                                                     quantidaderegistrosimportados, 
                                                                                     quantidaderegistrosarquivo,
                                                                                     dataimportacao) 
                                                                        VALUES(@Id, 
                                                                               @QuantidadeRegistrosImportados, 
                                                                               @QuantidadeRegistrosArquivo, 
                                                                               @DataImportacao) 
                                                                        RETURNING id", file);
            }
        }

        public async Task<int> InserirRegistrosArquivo(List<FileLine> registros, string idImportacao)
        {
            using (var conexao = connection.Create())
            {
                int quantidadeRegistroImportados = 0;

                foreach (var registro in registros)
                {
                    try
                    {
                        registro.IdArquivoImportacao = idImportacao;

                        await conexao.QueryFirstOrDefaultAsync(@"
                        INSERT INTO arquivoregistros(
                                tconst, 
                                titleType, 
                                primaryTitle, 
                                originalTitle, 
                                isAdult, 
                                startYear,
                                endYear,
                                runtimeMinutes,
                                genres,
                                idarquivoimportacao) 
                        VALUES (@Tconst, 
                                @TitleType, 
                                @PrimaryTitle,
                                @OriginalTitle, 
                                @IsAdult, 
                                @StartYear, 
                                @EndYear, 
                                @RuntimeMinutes,
                                @Genres,
                                @IdArquivoImportacao)", registro);

                        quantidadeRegistroImportados++;
                        
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }

                return quantidadeRegistroImportados;


            }
        }

        public async Task AtualizarQuantidadeDeRegistrosImportados(int quantidade, string idImportacao)
        {
            using (var conexao = connection.Create())
            {
                await conexao.ExecuteAsync(@"UPDATE arquivo
                                             SET quantidaderegistrosimportados = @Quantidade
                                             WHERE id = @IdImportacao", 
                                             new { Quantidade = quantidade, IdImportacao = idImportacao});
            }
        }
    }
}
