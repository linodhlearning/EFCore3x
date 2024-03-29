﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;
namespace SamuraiApp.Data
{
    // can rename to SamuariContext and use with ConsoleAPP
    public class SamuraiContext_OLD : DbContext
    {
        public SamuraiContext_OLD() { }// just to allow for defaults

        //public SamuraiContext_OLD(DbContextOptions<SamuraiContext_OLD> options) : base(options)
        //{
        //}
        public SamuraiContext_OLD(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Translation> Translations { get; set; }

        public DbSet<SamuraiBattleStat> SamuraiBattleStats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SamuraiApp_OldData";
                optionsBuilder.UseSqlServer(connectionString);
            }

            /*//for verobse out
                     optionsBuilder.UseLoggerFactory(ConsoleLoggerFactory)
            .EnableSensitiveDataLogging()
            .UseSqlServer(connectionString);
             */

            //   optionsBuilder.EnableSensitiveDataLogging(true);

            // optionsBuilder.UseSqlServer(connectionString,options=>options.MaxBatchSize(150));
        }
        // temp code to allow logging sql commands depatched by 
        public static readonly ILoggerFactory ConsoleLoggerFactory = LoggerFactory.Create(b =>
        {
            b.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
            .AddConsole();
        });
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>().HasKey(s => new { s.SamuraiId, s.BattleId });
            modelBuilder.Entity<Horse>().ToTable("Horses");
            modelBuilder.Entity<SamuraiBattleStat>().HasNoKey().ToView("SamuariBattleStats");
        }


    }
}
