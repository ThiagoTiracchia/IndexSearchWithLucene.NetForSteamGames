using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using LuceneDirectory = Lucene.Net.Store.Directory;
using Document = Lucene.Net.Documents.Document;
using Lucene.Net.QueryParsers.Classic;
using SteamSearch;
using Lucene.Net.Search.Suggest.Analyzing;




namespace IndexSearch
{

    public class SteamSearchEngine
    {
        private readonly AnalyzingInfixSuggester suggester;
        private readonly Analyzer _analyzer;
        private readonly LuceneDirectory _directory;
        private readonly IndexWriter _writer;

        private const LuceneVersion version = LuceneVersion.LUCENE_48;

        public SteamSearchEngine()
        {
           
            _analyzer = new StandardAnalyzer(version);
            _directory = new RAMDirectory();
            var config = new IndexWriterConfig(version, _analyzer);
            _writer = new IndexWriter(_directory, config);
            AnalyzingInfixSuggester suggester = new AnalyzingInfixSuggester(
               version, _directory, _analyzer);


        }

        public void AddSteamGamesToIndex(IEnumerable<SteamGame> steamGames)
        {
            foreach (SteamGame steam in steamGames) { 
                var document = new Document();
                document.Add(new TextField("name", steam.name, Field.Store.YES));
            
                _writer.AddDocument(document);
            }
            _writer.Commit();


        }

        public IEnumerable<SteamGame> SearchSteamGames(string searchterm) { 
        var directoryReader =  DirectoryReader.Open(_directory);
        var indexSearcher = new IndexSearcher(directoryReader);

            string  field = "name";
            QueryParser parser = new QueryParser(version, field, _analyzer);
            Query query = parser.Parse(searchterm);
            var hits = indexSearcher.Search(query, n: 10).ScoreDocs;
            var games = new List<SteamGame>();
            foreach(var hit in hits)
            {
                var document = indexSearcher.Doc(hit.Doc);
                games.Add(new SteamGame()
                {
                    name = document.Get("name")
                }); ;

            }
            return games;


        }

    }
}
