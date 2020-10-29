using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    /*
     * add-migration -Context "SamuraiContext_OLD" "NewStoredProcs"
     Update-Database -Context "SamuraiContext_OLD"
     */
    class Program
    {
        private static SamuraiContext_OLD _context = new SamuraiContext_OLD();
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
                else if (args[0] == "3")
                {
                    QueryAndUpdateBattle_Disconnected();
                }
                else if (args[0] == "4")
                {
                    NoTrackingQueryContext();
                }
                else if (args[0] == "5")
                { // attach
                    AddQuoteToExistingSamuraiNotTracking(4);
                    AddQuoteToExistingSamuraiNotTracking_2(5);
                }
                else if (args[0] == "6")
                {
                    EagerLoad_SamuraiQuotes();
                }
                else if (args[0] == "7")
                {
                    Project_SamuraiQuotes();
                }
                else if (args[0] == "8")
                {
                    // explicit loading
                    ExplicitLoad_SamuraiQuotes();
                }
                else if (args[0] == "9")
                {
                    // Lazy loading
                    /**
                     * Every Nav property must be virtual
                     * Microsoft.EntiyFramework.Proxies Package needed
                     * ModelBuilder.UseLazyLoadingProxies()
                     * **/
                }
                else if (args[0] == "10")
                {
                    ModifyRelatedData_WhenNotTracked();
                }
                else if (args[0] == "11")
                {
                    JoinBattleAndSamurai();
                    EnlistSamuraiToABattle();
                    RemoveSamuraiBattle_1();
                }
                else if (args[0] == "12")
                {
                    GetSamuraisWithBattles();

                }
                else if (args[0] == "13")
                {
                    GetSamuraisWithBattles();
                    GetSamuraiWithClan();
                    GetClanWithSamurai();
                }
                else if (args[0] == "14")
                {
                    //KEYLESS entities 
                    GetSamuariBattleStatistics(1);
                }
                else if (args[0] == "15")
                {
                    QueryUsingRaw_1(1);
                }
                else if (args[0] == "16")
                {
                    QueryUsingRawSQLStoredProcedure();
                }
                else if (args[0] == "17")
                {
                    RunRawSQLCommands();
                }
            }

            Console.ReadKey();
        }

        private static void RunRawSQLCommands()
        {
            string text = "ha";
            var s1 = _context.Database.ExecuteSqlRaw("Exec dbo.SamuarisWithQuote {0}", text);
        }

        private static void QueryUsingRawSQLStoredProcedure()
        {
            var text = "ha";
            var samurais1 = _context.Samurais.FromSqlRaw("Exec dbo.SamuarisWithQuote {0}", text).ToList();
            //or use interpolation
            var samurais2 = _context.Samurais.FromSqlInterpolated($"Exec dbo.SamuarisWithQuote {text}").ToList();
        }

        private static void QueryUsingRaw_1(int samuariId)
        {
            var samuaris = _context.Samurais.FromSqlRaw("select * from Samurais").ToList();
        }

        private static void GetSamuariBattleStatistics(int v)
        {
            var stat = _context.SamuraiBattleStats.FirstOrDefault(s => s.Name == "CDC");
            //but we cant use DBSET methods like Find as there is no key on this view
            var errorGenerated = _context.SamuraiBattleStats.Find(1);//compiler happy runtime surprise :)
        }

        private static void GetClanWithSamurai()
        {
            var clan = _context.Clans.Find(1);
            var samuraisForClan = _context.Samurais.Where(s => s.Clan.Id == 1).ToList();

        }
        private static void GetSamuraiWithClan()
        {
            var samurai = _context.Samurais.Include(s => s.Clan).ToList();
        }

        private static void GetSamuraisWithBattles()
        {
            var sbs = _context.Samurais
            .Include(s => s.SamuraiBattles)
            .ThenInclude(sb => sb.Battle)
            .FirstOrDefault(s => s.Id == 4);

            //OR

            var sbs2 = _context.Samurais.Where(s => s.Id == 2)
            .Select(s => new
            {
                Samurai = s,
                Battles = s.SamuraiBattles.Select(sb => sb.Battle)
            }).FirstOrDefault();
        }

        private static void RemoveSamuraiBattle_1()
        {
            var samuraiBattle = new SamuraiBattle { SamuraiId = 2, BattleId = 1 };
            _context.Remove(samuraiBattle);
            _context.SaveChanges();
        }

        private static void EnlistSamuraiToABattle()
        {
            var battle = _context.Battles.Find(1);
            battle.SamuraiBattles.Add(new SamuraiBattle { SamuraiId = 4 });
            _context.SaveChanges();
        }

        private static void JoinBattleAndSamurai()
        {
            var sb = new SamuraiBattle { BattleId = 1, SamuraiId = 4 };
            _context.Add(sb);// see no db set exposed for samurai battle
            _context.SaveChanges();
        }

        private static void ModifyRelatedData_WhenNotTracked()
        {
            var samuari = _context.Samurais.Include(s => s.Quotes).FirstOrDefault(s => s.Id == 4);
            var quote = samuari.Quotes[0];
            quote.Text = "changed Text";
            using (var newContext = new SamuraiContext_OLD())
            {
                // newContext.Quotes.Update(quote);// EF Core would have updated all the quotes instead of just the changed one
                newContext.Entry(quote).State = EntityState.Modified; //only going to track the changed one
                newContext.SaveChanges();
            }
        }

        private static void ExplicitLoad_SamuraiQuotes()
        {
            //only for one specific samurai object
            var samurai = _context.Samurais.FirstOrDefault();
            _context.Entry(samurai).Collection(s => s.Quotes).Load();
            _context.Entry(samurai).Reference(s => s.Horse).Load();

        }

        private static void Project_SamuraiQuotes()
        {
            // anonymous type to project data (entities are still tracked by EF Core)
            var s = _context.Samurais.Select(s => new { s.Id, s.Name, s.Quotes }).ToList();
        }

        private static void EagerLoad_SamuraiQuotes()
        {

            var quotes = _context.Samurais
            .Include(s => s.Quotes)
            .ThenInclude(t => t.Translations).ToList();
            Console.WriteLine(quotes);
        }

        private static void AddQuoteToExistingSamuraiNotTracking(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote { Text = "test no track " });
            using (var newContext = new SamuraiContext_OLD())
            {
                newContext.Samurais.Attach(samurai);
                newContext.SaveChanges();
            }
        }
        private static void AddQuoteToExistingSamuraiNotTracking_2(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote { Text = "test no track ", SamuraiId = samuraiId });
            using (var newContext = new SamuraiContext_OLD())
            {
                newContext.Samurais.Add(samurai);
                newContext.SaveChanges();
            }
        }

        private static void NoTrackingQueryContext()
        {
            using (var newContextInstance = new SamuraiContextNoTrack())
            {
                var lastSamuari = newContextInstance.Samurais.OrderBy(a => a.Id).LastOrDefault();
                Console.WriteLine(lastSamuari.Name);
            }
        }
        private static void QueryAndUpdateBattle_Disconnected()
        {
            var battle = _context.Battles.AsNoTracking().FirstOrDefault();
            battle.EndDate = new DateTime(1986, 08, 05);

            using (var newContextInstance = new SamuraiContext_OLD())
            {//context and update as no tracking
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
                Console.WriteLine($"Translations: {string.Join(',', q.Translations.Select(t => string.Concat(t.LanguageCode, " - ", t.Description)).ToArray())}");
            }
        }

        private static void AddSamurai(string name = "sam")
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
            var q2 = new Quote
            {
                Text = "You can always die. It's living that takes real courage",
                Translations = new List<Translation> {
                    new Translation {LanguageCode="ja",Description="あなたはいつでも死ぬことができます。本当の勇気が必要なのは生きている" },
                    new Translation {LanguageCode="ar",Description="يمكنك دائما الموت. إنها الحياة التي تتطلب شجاعة حقيقية" },
                    new Translation {LanguageCode="it",Description="Puoi sempre morire. È vivere che richiede vero coraggio" }
                        }
            };
            var qs = new List<Quote> { q1, q2 };

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
