namespace APIWikipediaArticles.Model;

//Represents a Wikipedia page
public class Page
{
    public int PageId { get; set; }
    public int NamespaceId { get; set; }
    public string Title { get; set; }
    public string Extract { get; set; }
}