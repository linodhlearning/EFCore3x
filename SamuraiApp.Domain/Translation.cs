 
namespace SamuraiApp.Domain
{
    public class Translation
    {
        public int Id { get; set; }
        public string LanguageCode { get; set; }
        public string Description { get; set; }
        public Quote Quote { get; set; }
        public int QuoteId { get; set; }
    }
}
