using Genbox.Wikipedia.Objects;

namespace APIWikipediaArticles.Repository.Contract;

public interface IWikipediaReaderRepository
{
    Task<(string Extract, int PageId, int NamespaceId, SearchResult SpecificPage)> ReadWikipediaArticle(string input);
}