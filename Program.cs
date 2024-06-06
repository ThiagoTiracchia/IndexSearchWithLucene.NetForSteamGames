using IndexSearch;
using Newtonsoft.Json.Linq;
using SteamSearch;
using System.Text.Json;


namespace SteamSearch
{
    public class SteamGame
    {
        public string name { get; set; }

    }
}
class Program
{
    static void Main()
    {
        Console.WriteLine("Waiting for the json to be parsed");
        var SteamFileText = File.ReadAllText("C:\\Users\\thiag\\source\\repos\\IndexSearch\\steamGames.json");
        var values = JObject.Parse(SteamFileText).SelectToken("applist").SelectToken("apps");
        var averdevuelta = values.ToString();
        var steamgames = JsonSerializer.Deserialize<List<SteamGame>>(averdevuelta);


        var engine = new SteamSearchEngine();
        engine.AddSteamGamesToIndex(steamgames);
        RunConsole(engine);

    }

    private static void RunConsole(SteamSearchEngine engine)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Enter a search query: ");
            var query = Console.ReadLine();
            if (string.IsNullOrEmpty(query))
                continue;

            var results = engine.SearchSteamGames(query);
            if (!results.Any())
            {
                Console.WriteLine("No results Found");
                continue;

            }

            foreach (var result in results)
            {
                Console.WriteLine($"({result.name})");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();


        }
    }
}


/* var values = JObject.Parse(SteamFileText).SelectToken("applist").SelectToken("apps").ToArray();
       List<string> listOfNames = new List<string>();
       foreach (var item in values)
       {

           var nameOfGame =  item.Value<string>("name");
           if (nameOfGame == "" || nameOfGame.Contains("test")  ) {


           } else
           {
               listOfNames.Add(nameOfGame);

           }


       }
       */
