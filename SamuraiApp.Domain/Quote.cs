using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Domain
{
    public class Quote
    {
        public Quote()
        {
            this.Translations = new List<Translation>();
        }
        public int Id { get; set; }
    
        public string Text { get; set; }
        public Samurai Samurai { get; set; }
        public int SamuraiId { get; set; }

        public List<Translation> Translations { get; set; }
    }
}
