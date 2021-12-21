using DesafioPricefy.Dominio.Interfaces.CommandHandler;
using DesafioPricefy.Dominio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DesafioPricefy.Controllers
{
    [ApiController]
    [Route("v1/upload")]
    public class UploadFileController : ControllerBase
    {
        private readonly IUploadFileCommandHandler uploadFileCommandHandler;

        public UploadFileController(IUploadFileCommandHandler uploadFileCommandHandler)
        {
            this.uploadFileCommandHandler = uploadFileCommandHandler;
        }

        [HttpPost]
        [Route("uploadfile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                var mensagem = await uploadFileCommandHandler.UploadFile(file);
                return Ok(mensagem);
            }
            catch (Exception ex)
            {
                return BadRequest(new Mensagem { Titulo = "Erro ao realizar importação!", Texto = ex.Message});
            }
        }

        [HttpGet]
        [Route("obterarquivosimportados")]
        public async Task<IActionResult> ObterArquivosImportados()
        {
            try
            {
                var arquivos = await uploadFileCommandHandler.ObterArquivosImportados();
                return Ok(arquivos);
            }
            catch (Exception ex)
            {
                return BadRequest(new Mensagem { Titulo = "Erro ao obter arquivos importados!", Texto = ex.Message });
            }
        }

        [HttpGet]
        [Route("obterarquivosimportadosporperiodo")]
        public async Task<IActionResult> ObterArquivosImportados([FromBody] FilterModel model)
        {
            try
            {
                var arquivos = await uploadFileCommandHandler.ObterArquivosImportadosPorPeriodo(model);
                return Ok(arquivos);
            }
            catch (Exception ex)
            {
                return BadRequest(new Mensagem { Titulo = "Erro ao obter arquivos importados!", Texto = ex.Message });
            }
        }
    }
}
