using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        static void Main(string[] args)
        {
            _context.Database.EnsureCreated();
            if (args.Length > 1)
            {
                if (args[0] == "1")
                {
                    //  GetSamurais("Before Add");
                    //  AddSamurai();
                    //  GetSamurais("After Add");
                    GetLastSamurai();
                }
                else if (args[0] == "2")
                { 
                //4 or more object adding will be batched
                  //  AddSamurai("s1");
                  //DeleteSamurai(2);
                  //DeleteSamuraiDirect(3);

                }
                else if(args[0]=="3"){
                    QueryAndUpdateBattle_Disconnected();
                }
            }
           


            Console.ReadKey();
        }

        private static void QueryAndUpdateBattle_Disconnected()
        {
            var battle = _context.Battles.AsNoTracking().FirstOrDefault();
            battle.EndDate = new DateTime(1986, 08, 05);

             using(var newContextInstance= new SamuraiContext()){//context and update as no tracking
                newContextInstance.Battles.Update(battle);
                newContextInstance.SaveChanges();
            }
        }

        private static void DeleteSamurai(int id)
        {
            var samurai = _context.Samurais.Find(id);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
        }
        private static void DeleteSamuraiDirect(int id)
        {
            var samurai = _context.Samurais.Find(id);
            _context.Remove(samurai);
            _context.SaveChanges();
        }

        private static void GetLastSamurai()
        {
            var lastSamuari = _context.Samurais.OrderBy(a => a.Id).LastOrDefault();
            Console.WriteLine($"{lastSamuari.Name} is the last Samurai with {lastSamuari.Quotes.Count} Quotes");
            foreach (var q in lastSamuari.Quotes)
            {
                Console.WriteLine($"Quote: {q.Text}");
                Console.WriteLine($"Translations: {string.Join(',',q.Translations.Select(t=> string.Concat(t.LanguageCode," - ",t.Description)).ToArray())}");
            }
        }

        private static void AddSamurai(string name="sam")
        {
            var q1 = new Quote
            {
                Text = "Because you just never know what's behind the freaking sky",
                Translations = new List<Translation> {
                    new Translation {LanguageCode="ja",Description="おかしな空の後ろに何があるのか​​わからないから" },
                    new Translation {LanguageCode="ar",Description="لأنك لا تعرف أبدًا ما وراء السماء المرعبة" },
                    new Translation {LanguageCode="it",Description="Perché non sai mai cosa c'è dietro quel dannato cielo" }
                        }
            };
            var q2= new Quote
            {
                Text = "You can always die. It's living that takes real courage",
                Translations = new List<Translation> {
                    new Translation {LanguageCode="ja",Description="あなたはいつでも死ぬことができます。本当の勇気が必要なのは生きている" },
                    new Translation {LanguageCode="ar",Description="يمكنك دائما الموت. إنها الحياة التي تتطلب شجاعة حقيقية" },
                    new Translation {LanguageCode="it",Description="Puoi sempre morire. È vivere che richiede vero coraggio" }
                        }
            };
            var qs = new List<Quote> { q1,q2 };

            var s = new Samurai { Name = name, Quotes = qs };
            _context.Samurais.Add(s);
            _context.SaveChanges();
        }
        private static void GetSamurais(string text)
        {
            var samurais = _context.Samurais.ToList();
            Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
            foreach (var s in samurais)
            {
                Console.WriteLine($"Samurai Name: {s.Name}");
            }
        }
    }
}
