namespace Astor.Logging.Tests;

public record Hobby(string Name, string Fav)
{
    public const string Json = """
                               {
                                "Name" : "Games",
                                "fav" : "Ticket to ride"
                               }
                               """;
    
    public static readonly object Anonymous = new
    {
        Name = "Board Games",
        fav = "Resistance"
    };

    public static readonly Hobby Example1 = new("Trails", "Mtirala");
}