using System;

namespace DesafioPricefy.Dominio.Models
{
    public class File
    {
        public string Id { get; set; }
        public int QuantidadeRegistrosImportados { get; set; }
        public int QuantidadeRegistrosArquivo { get; set; }
        public DateTime DataImportacao { get; set; }
    }
}
