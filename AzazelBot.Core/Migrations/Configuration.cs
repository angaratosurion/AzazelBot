namespace AzazelBot.Core.Migrations
{
    using AzazelBot.Core.Data.Models;
    using global::AzazelBot.Core.Data;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AZBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
            
        }

        protected override void Seed(AZBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            FavoriteCharacter fav = new FavoriteCharacter();
            fav.AddedAt = DateTime.Now;
            fav.CharacterName = "test";
            fav.ServerId = "test";
            fav.uId = "test";
            context.FavoriteCharacters.AddOrUpdate(fav);
            GiftHistory gift = new GiftHistory();
            gift.ServerId = "test";
            gift.uId = "test";
            gift.Year = 1;
            gift.GiftedAt = DateTime.Now;
            gift.CharacterName = "test";
            context.GiftHistory.AddOrUpdate(gift);
            context.SaveChanges();
            
            

        }
    }
}
