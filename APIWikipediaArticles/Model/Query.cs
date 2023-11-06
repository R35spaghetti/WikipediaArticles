namespace APIWikipediaArticles.Model;

//A query to the Wikipedia API
public class Query
{
    public Dictionary<string, Page> Pages { get; set; }
}