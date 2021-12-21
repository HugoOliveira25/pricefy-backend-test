namespace DesafioPricefy.Dominio.Models
{
    public class FileLine
    {
        public string Tconst { get; set; }
        public string TitleType { get; set; }
        public string PrimaryTitle { get; set; }
        public string OriginalTitle { get; set; }
        public bool IsAdult { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string RuntimeMinutes { get; set; }
        public string Genres { get; set; }
        public string IdArquivoImportacao { get; set; }
    }
}
