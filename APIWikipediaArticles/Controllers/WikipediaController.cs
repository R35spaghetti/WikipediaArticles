using Microsoft.AspNetCore.Mvc;
using APIWikipediaArticles.Repository.Contract;

namespace APIWikipediaArticles.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WikipediaController : ControllerBase
{
    private readonly IWikipediaReaderRepository _wikipediaReaderRepository;

    public WikipediaController(IWikipediaReaderRepository wikipediaReaderRepository)
    {
        _wikipediaReaderRepository = wikipediaReaderRepository;
    }

    [HttpGet]
    public async Task<ActionResult> GetRandomPage(string input)
    {
        var article = await _wikipediaReaderRepository.ReadWikipediaArticle(input);

        return Ok(new { article.SpecificPage.Title, article.Extract, article.PageId, article.NamespaceId });
    }
}