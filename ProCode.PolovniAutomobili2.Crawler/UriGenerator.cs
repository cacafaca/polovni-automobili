namespace ProCode.PolovniAutomobili2.Crawler
{
    public class UriGenerator
    {
        public int Page { get; private set; }
        private Uri baseUri = new Uri("https://www.polovniautomobili.com", UriKind.Absolute);

        public UriGenerator()
        {
            Page = 0;
        }

        public Uri GetNext()
        {
            return new Uri(baseUri, $"auto-oglasi/pretraga?page={++Page}");
        }
        private Uri GetStartUri()
        {
            return new Uri("https://www.polovniautomobili.com/auto-oglasi/pretraga?brand=&price_to=&year_from=&year_to=&showOldNew=all&submit_1=&without_price=1");
        }
        private Uri GetNextUri()
        {
            return new Uri("https://www.polovniautomobili.com/auto-oglasi/pretraga?page=2&sort=basic&city_distance=0&showOldNew=all&without_price=1");
        }

    }
}
