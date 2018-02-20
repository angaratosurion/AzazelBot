using BotGear.Data;
using AzazelBot.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzazelBot.Core.Data
{
   public class AZBContext :BotGearContext
    {
       

        public IDbSet<FavoriteCharacter>  FavoriteCharacters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Properties<DateTime?>()
                  .Configure(c => c.HasColumnType("datetime2"));

        }
        public static new AZBContext Create()
        {
            return new AZBContext();
        }
    }
}
