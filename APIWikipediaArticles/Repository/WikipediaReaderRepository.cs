using Genbox.Wikipedia;
using Genbox.Wikipedia.Objects;
using Newtonsoft.Json;
using APIWikipediaArticles.Model;
using APIWikipediaArticles.Repository.Contract;

namespace APIWikipediaArticles.Repository;

public class WikipediaReaderRepository : IWikipediaReaderRepository
{
    private readonly HttpClient _client;

    public WikipediaReaderRepository(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient();
    }

    public async Task<(string Extract, int PageId, int NamespaceId, SearchResult SpecificPage)>
        ReadWikipediaArticle(string input)
    {
        var client = new WikipediaClient();
        var request = new WikiSearchRequest(input);
        var searchResponse = await client.SearchAsync(request);

        var specificPage = searchResponse.QueryResult?.SearchResults.OrderBy(x => Guid.NewGuid()).First();
        //Fetch the details of the random page
        object response = await GetDetailsFromPage(specificPage ?? throw new InvalidOperationException());

        var article = await GetPageValuesInTuple(specificPage, response.ToString() ?? string.Empty);

        return article;
    }

    private async Task<(string Extract, int PageId, int NamespaceId, SearchResult SpecificPage)> GetPageValuesInTuple(
        SearchResult specificPage, string response)
    {
        var responseJson = await DeserializeJsonAsync(response);
        var firstKey = responseJson.Query.Pages.First().Key;
        var extract = responseJson.Query.Pages[firstKey].Extract;
        var pageid = responseJson.Query.Pages[firstKey].PageId;
        var namespaceid = responseJson.Query.Pages[firstKey].NamespaceId;

        var articleValues = (Extract: extract, PageId: pageid, NamespaceId: namespaceid, SpecificPage: specificPage);

        return articleValues;
    }

    private async Task<RootObject> DeserializeJsonAsync(string json)
    {
        var stringReader = new StringReader(json);
        var jsonTextReader = new JsonTextReader(stringReader);
        var serializer = new JsonSerializer();
        return await Task.Run(() => serializer.Deserialize<RootObject>(jsonTextReader)) ??
               throw new InvalidOperationException();
    }

    private async Task<object> GetDetailsFromPage(SearchResult specificPage)
    {
        var response = await _client.GetStringAsync("https://en.wikipedia.org/w/api.php?format=json&" +
                                                    "action=query&prop=extracts&" +
                                                    "exlimit=max&" +
                                                    "explaintext&" +
                                                    "exintro&" +
                                                    "titles=" + Uri.EscapeDataString(specificPage.Title) +
                                                    "&redirects=");

        return response;
    }
}