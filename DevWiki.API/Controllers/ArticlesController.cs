using DevWiki.Application.Articles.Commands.CreateArticle;
using DevWiki.Application.Articles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DevWiki.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly ISender _sender;

        public ArticlesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetArticles([FromQuery] string? searchTerm = null)
        {
            var articles = await _sender.Send(new GetArticlesQuery(searchTerm));
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticle(Guid id)
        {
            var article = await _sender.Send(new DevWiki.Application.Articles.Queries.GetArticleById.GetArticleByIdQuery(id));
            if (article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> CreateArticle([FromBody] CreateArticleCommand command)
        {
            var articleId = await _sender.Send(command);
            return CreatedAtAction(nameof(GetArticle), new { id = articleId }, articleId);
        }

        [HttpPut("{id}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> UpdateArticle(Guid id, [FromBody] DevWiki.Application.Articles.Commands.UpdateArticle.UpdateArticleCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await _sender.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> DeleteArticle(Guid id)
        {
            await _sender.Send(new DevWiki.Application.Articles.Commands.DeleteArticle.DeleteArticleCommand(id));
            return NoContent();
        }
    }
}
