using DesafioPricefy.Dominio.Interfaces.CommandHandler;
using DesafioPricefy.Dominio.Interfaces.RepositorioEscrita;
using DesafioPricefy.Dominio.Interfaces.RepositorioLeitura;
using DesafioPricefy.Dominio.Models;
using Microsoft.AspNetCore.Http;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using File = DesafioPricefy.Dominio.Models.File;

namespace DesafioPricefy.Handler
{
    public class UploadFileCommandHandler : IUploadFileCommandHandler
    {
        private readonly IUploadFileRepositorioEscrita uploadFileRepositorioEscrita;
        private readonly IUploadFileRepositorioLeitura uploadFileRepositorioLeitura;

        public UploadFileCommandHandler(IUploadFileRepositorioEscrita uploadFileRepositorioEscrita, IUploadFileRepositorioLeitura uploadFileRepositorioLeitura)
        {
            this.uploadFileRepositorioEscrita = uploadFileRepositorioEscrita;
            this.uploadFileRepositorioLeitura = uploadFileRepositorioLeitura;
        }

        public async Task<Mensagem> UploadFile(IFormFile file)
        {
            var arquivo = new List<FileLine>();

            StreamReader srTSVFile = new StreamReader(file.OpenReadStream());

            string sHeaderLine = srTSVFile.ReadLine();

            List<string[]> lstData = new List<string[]>();
            while (!srTSVFile.EndOfStream)
            {
                string sDataLine = srTSVFile.ReadLine();
                string[] sDataEntries = sDataLine.Split('\t');
                lstData.Add(sDataEntries);

                var linha = new FileLine
                {
                    Tconst = sDataEntries[0],
                    TitleType = sDataEntries[1],
                    PrimaryTitle = sDataEntries[2],
                    OriginalTitle = sDataEntries[3],
                    IsAdult = Boolean.TryParse(sDataEntries[4], out bool value),
                    StartYear = sDataEntries[5],
                    EndYear = sDataEntries[6],
                    RuntimeMinutes = sDataEntries[7],
                    Genres = sDataEntries[8],
                };

                arquivo.Add(linha);
            }
            srTSVFile.Close();

            try
            {
                var arquivoJaImportado = await this.uploadFileRepositorioLeitura.ValidarArquivoJaImportado(file.FileName.Substring(0, file.FileName.IndexOf(".")));

                if (arquivoJaImportado != null)
                {
                    return await Task.FromResult(new Mensagem()
                    {
                        Titulo = "Arquivo não importado!",
                        Texto = "Este arquivo já foi importado"
                    });
                }

                var arquivoImportacao = new File()
                {
                    Id = file.FileName.Substring(0, file.FileName.IndexOf(".")),
                    QuantidadeRegistrosArquivo = arquivo.Count,
                    QuantidadeRegistrosImportados = 0,
                    DataImportacao = DateTime.Now
                };

                var idArquivoImportado = await this.uploadFileRepositorioEscrita.InserirImportacaoArquivo(arquivoImportacao);
                var quantidadeRegistrosImportados = await uploadFileRepositorioEscrita.InserirRegistrosArquivo(arquivo, idArquivoImportado);
                await uploadFileRepositorioEscrita.AtualizarQuantidadeDeRegistrosImportados(quantidadeRegistrosImportados, arquivoImportacao.Id);

                return new Mensagem()
                {
                    Titulo = "Arquivo importado com sucesso!",
                    Texto = $"Foram inseridos {quantidadeRegistrosImportados} registros no banco."
                    
                };
            }
            catch (Exception ex)
            {

                if(ex.GetBaseException() is PostgresException pgException)
                {
                    switch (pgException.SqlState)
                    {
                        case "23505":
                            return new Mensagem()
                            {
                                Titulo = "Erro ao realizar importação!",
                                Texto = "Existem registros duplicados no arquivo, por favor verifique."

                            };
                        default:
                            return new Mensagem()
                            {
                                Titulo = "Erro ao realizar importação!",
                                Texto = ex.Message

                            };
                    }
                }
                return new Mensagem()
                {
                    Titulo = "Erro ao realizar importação!",
                    Texto = ex.Message

                };


            }
        }

        public async Task<IEnumerable<File>> ObterArquivosImportados()
        {
            return await uploadFileRepositorioLeitura.ObterArquivosImportados();
        }

        public async Task<IEnumerable<File>> ObterArquivosImportadosPorPeriodo(FilterModel model)
        {
            return await uploadFileRepositorioLeitura.ObterArquivosImportadosPorPeriodo(model);
        }
    }
}
