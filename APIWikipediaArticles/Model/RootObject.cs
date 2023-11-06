namespace APIWikipediaArticles.Model;

//Root object of the JSON response from the Wikipedia API
public class RootObject
{
    public string BatchComplete { get; set; }
    public Query Query { get; set; }
}