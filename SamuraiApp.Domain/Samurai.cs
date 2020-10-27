using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Domain
{
    public class Samurai
    {
        public Samurai()
        {
            this.Quotes = new List<Quote>();
            this.SamuraiBattles = new List<SamuraiBattle>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Quote> Quotes{ get; set; }
        public Clan Clan { get; set; }
        public Horse Horse{ get; set; }


        public List<SamuraiBattle> SamuraiBattles { get; set; }
    }
}
