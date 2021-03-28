using Articles.API.Models;
using Articles.API.Models.SearchQueries;
using Articles.API.Persistence;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.API.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {

        private ArticleDbContext _articleDbContext;

        public ArticlesController(ArticleDbContext articleDbContext)
        {
            _articleDbContext = articleDbContext;
        }

        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            var article = _articleDbContext.Articles.FirstOrDefault(x => x.Id == Id);

            if (article != null)
            {
                return Ok(article);
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult GetAll([FromQuery]ArticleSearchQuery query)
        {
            var articles = _articleDbContext.Articles.Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedAt).ToList();

            if(!string.IsNullOrEmpty(query.Title))
            {
                articles = articles.Where(x => x.Title.Contains(query.Title)).ToList();
            }

            if(articles != null && articles.Count > 0)
            {
                return Ok(articles);
            }
            return NotFound();

        }

        [HttpPut]
        public IActionResult Update(Article article)
        {
            var articleEntity = _articleDbContext.Articles.FirstOrDefault(x => x.Id == article.Id);

            if(articleEntity != null)
            {
                articleEntity.Title = article.Title;
                articleEntity.Description = article.Description;
                articleEntity.UpdatedAt = DateTime.Now;
                articleEntity.Phone = article.Phone;
                articleEntity.Image = article.Image;

                _articleDbContext.Articles.Update(articleEntity);
                _articleDbContext.SaveChanges();

                return NoContent();
            }

            return BadRequest();
        }

        [HttpPost]
        public IActionResult Post(Article article)
        {
            try
            {
                article.CreatedAt = DateTime.Now;
                article.UpdatedAt = null;

                _articleDbContext.Articles.Add(article);
                _articleDbContext.SaveChanges();

                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            var article = _articleDbContext.Articles.FirstOrDefault(x => !x.IsDeleted && x.Id == Id);

            if(article != null)
            {
                article.IsDeleted = true;
                article.UpdatedAt = DateTime.Now;

                _articleDbContext.Articles.Update(article);
                _articleDbContext.SaveChanges();

                return NoContent();
            }

            return BadRequest($"განცხადება აიდით {Id} არ არსებობს");
        }
    }
}
