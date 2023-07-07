// Ignore Spelling: Automobili Polovni

namespace ProCode.PolovniAutomobili2.Crawler
{
    public class Crawler
    {
        public void Start()
        {
            Uri startUri = GetStartUri();
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