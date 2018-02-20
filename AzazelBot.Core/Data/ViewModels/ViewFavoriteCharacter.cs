using BotGear.Managers;
using BotGear.Tools;
using AzazelBot.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzazelBot.Core.Data.ViewModels
{
    public class ViewFavoriteCharacter
    {
      
        public int Id { get; set; }
       
        public   string  uId { get; set; }
       
        public string CharacterName { get; set; }

        public DateTime AddedAt { get; set; }
        
        public string ServerId { get; set; }
        public string ServerName { get; set; }
        public async void ImportModel(FavoriteCharacter model)
        {
            try
            {
                if (model != null)
                {
                    this.AddedAt = model.AddedAt;
                    this.CharacterName = model.CharacterName;
                    this.Id = model.Id;
                    this.ServerId = model.ServerId;
                    this.uId = model.uId;
                    ServerManager mngr = new ServerManager();
                    var srv = await mngr.getServerbyId(this.ServerId);
                    if ( srv !=null)
                    {
                        this.ServerName = srv.Name;
                    }

                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        public FavoriteCharacter ExportModel()
        {
            try
            {
                FavoriteCharacter ap = new FavoriteCharacter();
                
                    ap.AddedAt = this.AddedAt;
                    ap.CharacterName = this.CharacterName;
                    ap.Id = this.Id;
                    ap.ServerId = this.ServerId;
                    ap.uId = this.uId;
                   

                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
    }
}
