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
    public class AZBContext : BotGearContext
    {

        //    public class AZBContext :DbContext
        //{
        //    public AZBContext()
        //        : base("DefaultConnection")
        //    {
        //        try
        //        {
        //            //    this.Configuration.AutoDetectChangesEnabled = true;
        //            //    //this.Configuration.LazyLoadingEnabled = true;
        //            //    //this.Configuration.ValidateOnSaveEnabled = false;

        //            string path = AppDomain.CurrentDomain.BaseDirectory;

        //            //System.IO.Directory.GetCurrentDirectory();
        //            AppDomain.CurrentDomain.SetData("DataDirectory", path);
        //            this.Configuration.AutoDetectChangesEnabled = true;

        //        }
        //        catch (StackOverflowException ex)
        //        {

        //        }

    

        public IDbSet<FavoriteCharacter>  FavoriteCharacters { get; set; }
        public IDbSet<GiftHistory>  GiftHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

           
            modelBuilder.Properties<DateTime?>()
                  .Configure(c => c.HasColumnType("datetime2"));
           
            base.OnModelCreating(modelBuilder);

        }
        public static new AZBContext Create()
        {
            return new AZBContext();
        }
    }
}
