 
using Microsoft.VisualStudio.TestTools.UnitTesting; 
using SamuraiApp.Domain;
using SamuraiApp.Data;
using System;
using Microsoft.EntityFrameworkCore;

namespace SamuraiApp.Tests
{
    [TestClass]
    public class InMemoryTests
    {
        [TestMethod]
        public void CanInsertSamuraisIntoMemDB1()
        {
            var builder =new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("CanInsertSamuariInstance1");
            using (var context = new SamuraiContext_OLD())  
            { 
                var samurai = new Samurai();
                context.Samurais.Add(samurai); 
                context.SaveChanges();
                Assert.AreEqual(EntityState.Added, context.Entry(samurai).State);

            }
        }
        [TestMethod]
        public void CanInsertSamuraisIntoMemDB2()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("CanInsertSamuariInstance2");
            using (var context = new SamuraiContext_OLD(builder.Options)) // options passed in to constructor to avoid calling actual db
            {
                var samurai = new Samurai();
                context.Samurais.Add(samurai);
                context.SaveChanges();
                Assert.AreEqual(1, samurai.Id);

            }
        }
         

    }
}
