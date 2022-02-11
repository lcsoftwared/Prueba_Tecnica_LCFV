using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Prueba_Tecnica_LCFV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {

        private static List<Article> articulos = new List<Article>
        {
            new Article {
            Id = 1,
            Title = "Prueba",
            Contenido = "Contenido",
            PublishDate = DateTime.Parse("01/01/2022"),
            MemberId = 1
            }
        };
        private readonly DataContext _context;

        public ArticlesController(DataContext context)
        {
           _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Article>>> Get()
        { 
            return Ok(await _context.Articulos.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> Get(int Id)
        {
            var article = await _context.Articulos.FindAsync(Id);
            if (article == null)
                return BadRequest("Articulo no Encontrado");
            return Ok(await _context.Articulos.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<List<Article>>> AddArticle(Article articulo)
        {
            _context.Articulos.Add(articulo);
            await _context.SaveChangesAsync();  
            return Ok(await _context.Articulos.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<Article>>> UptdateArticle(Article request)
        {
            var article = await _context.Articulos.FindAsync(request.Id);
            if (article == null)
                return BadRequest("Articulo no Encontrado");

            article.Title = request.Title;
            article.Contenido = request.Contenido;  
            article.PublishDate = request.PublishDate;
            article.MemberId = request.MemberId;

            return Ok(await _context.Articulos.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Article>>> Delete(int Id)
        {
            var article = await _context.Articulos.FindAsync(Id); 
            if (article == null)
                return BadRequest("Articulo no Encontrado");

            articulos.Remove(article);
            return Ok(await _context.Articulos.ToListAsync());
        }

    }
}
