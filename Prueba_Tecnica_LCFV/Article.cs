namespace Prueba_Tecnica_LCFV
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; }
        public int MemberId { get; set; }

    }
}
