using Microsoft.VisualStudio.TestTools.UnitTesting; 
using SamuraiApp.Domain;
using SamuraiApp.Data;
using System;

namespace SamuraiApp.Tests
{
    [TestClass]
    public class DatabaseIntegrationTests
    {
        [TestMethod]
        public void CanInsertSamuraisIntoFullBlownDB()
        {
            using (var context = new SamuraiContext_OLD())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var samurai = new Samurai { Name = "li" };
                context.Samurais.Add(samurai);
                Console.WriteLine($"Before save is {samurai.Id}");
                context.SaveChanges();

                Console.WriteLine($"After save is {samurai.Id}");

                Assert.AreEqual(1, samurai.Id);

            }
        }



    }
}
