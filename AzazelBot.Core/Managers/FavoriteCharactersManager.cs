using BotGear.Managers;
using BotGear.Tools;
using AzazelBot.Core.Data;
using AzazelBot.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzazelBot.Core.Managers
{
    public class FavoriteCharactersManager
    {
        AZBContext db = new AZBContext( );
        UserManager usermngr = new UserManager();

        public async Task AddFavorite(string  uid, string fchar,string sid)
        {
            try
            {
                var user = await  usermngr.GetUserbyId(uid);
                string fchar2 = null;
                if (fchar.Contains(" ,") == true)
                {
                    fchar2 = fchar.Replace(" ,", ",");
                }
                 if (fchar.Contains(", ") == true)
                {
                    fchar2 = fchar.Replace(", ", ",");
                }
               if (fchar.Contains(" , ") == true)
                {
                    fchar2 = fchar.Replace(" , ", ",");
                }
               else
                {
                    fchar2 = fchar;
                }


                if (fchar2 != null && user != null &&sid!=null)
                {
                    if (fchar2.Contains(',') == true)
                    {
                        var chars = fchar2.Split(',');
                        if (chars != null)
                        {
                           
                            foreach (var c in chars)
                            {
                                var exchar = await this.ExistFavorite(uid, c,sid);
                                if (exchar == null)
                                {
                                    FavoriteCharacter ch = new FavoriteCharacter();
                                    ch.AddedAt = DateTime.Now;
                                    ch.ServerId = sid;
                                    string name = c.Replace(" ", "_");
                                    ch.CharacterName = c;
                                    ch.uId =user.Id;
                                    db.FavoriteCharacters.Add(ch);
                                    await db.SaveChangesAsync();
                                }
                            }

                        }
                    }

                    else
                    {
                        var exchar = await this.ExistFavorite(uid, fchar2,sid);
                        if (exchar == null)
                        {
                            FavoriteCharacter ch = new FavoriteCharacter();
                            ch.AddedAt = DateTime.Now;

                            string name = fchar2.Replace(" ", "_");
                            ch.CharacterName = fchar2;
                            ch.uId =  user.Id;
                            ch.ServerId = sid;
                            db.FavoriteCharacters.Add(ch);
                            await db.SaveChangesAsync();
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }

        public async Task<FavoriteCharacter> ExistFavorite(string  uid, string fchar,string sid)
        {
            try
            {
               var ap = db.FavoriteCharacters.FirstOrDefault(x => x.uId == uid && x.CharacterName == fchar && x.ServerId==sid);

                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public FavoriteCharacter GetUsersFavoriteByCharacterName(string  uid ,string charname)
        {
            try
            {
                FavoriteCharacter ap = null;
               if ( charname !=null && uid !=null)
                {
                    var chars = this.GetUserFavorite(uid);
                    if ( chars!=null)
                    {
                        foreach(var c in chars)
                        {
                            if ( c.CharacterName == charname)
                            {
                                ap = c;
                                break;

                            }
                        }
                    }
                }


                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public FavoriteCharacter GetUsersFavoriteById(string  uid,  int id)
        {
            try
            {
                FavoriteCharacter ap = null;
               
                    var chars = this.GetUserFavorite(uid);
                    if (chars != null)
                    {

                        ap = chars.Find(x => x.Id == id);
                    }
               


                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public List<FavoriteCharacter> GetUserFavorite(string  uid )
        {
            try
            {
                return db.FavoriteCharacters.Where(x => x.uId == uid).ToList();

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task DelFavorites(string uid, string fchar,string sid)
        {
            try
            {
                var user = await usermngr.GetUserbyId(uid);
                string fchar2 = null;
                if (fchar.Contains(" ,") == true)
                {
                    fchar2 = fchar.Replace(" ,", ",");
                }
                if (fchar.Contains(", ") == true)
                {
                    fchar2 = fchar.Replace(", ", ",");
                }
                if (fchar.Contains(" , ") == true)
                {
                    fchar2 = fchar.Replace(" , ", ",");
                }
                else
                {
                    fchar2 = fchar;
                }


                if (fchar2 != null && user != null && sid!=null)
                {
                    if (fchar2.Contains(',') == true)
                    {
                        var chars = fchar2.Split(',');
                        if (chars != null)
                        {

                            foreach (var c in chars)
                            {
                                var exchar = await this.ExistFavorite(uid, c,sid);
                                if (exchar != null)
                                {

                                    db.FavoriteCharacters.Remove(exchar);
                                    await db.SaveChangesAsync();
                                }
                            }

                        }
                    }
                
                else
                {
                    var exchar = await this.ExistFavorite(uid, fchar2,sid);
                    if (exchar != null)
                    {
                        db.FavoriteCharacters.Remove(exchar);
                        await db.SaveChangesAsync();
                    }
                }

                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        public async Task DelFavorites(string uid)
        {
            try
            {
                var user = await usermngr.GetUserbyId(uid);
                



                if ( user != null)
                {


                    var exchar =  this.GetUserFavorite(uid);
                        if (exchar == null)
                        {
                            


                        if (exchar != null)
                        {

                            foreach (var e in exchar)
                            {

                                db.FavoriteCharacters.Remove(e);
                                await db.SaveChangesAsync();
                            }

                        }
                        }
                    

                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }

        public async Task DelFavoritesByid(string uid,int id)
        {
            try
            {
                var user = await usermngr.GetUserbyId(uid);




                if (user != null)
                {


                    var exchar = this.GetUsersFavoriteById( user.Id, id);
                    if (exchar != null)
                    {



                        db.FavoriteCharacters.Remove(exchar);
                        await db.SaveChangesAsync();
                    }


                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        public async Task EditFavorite(string  uid, int id, string fchar2)
        {
            try
            {


                var user = await usermngr.GetUserbyId(uid);

                if (fchar2 != null && user != null && id>0  )
                {



                    var exchar =  this.GetUsersFavoriteById(uid,id);
                        if (exchar != null)
                    {
                        FavoriteCharacter chr = new FavoriteCharacter();
                        chr.AddedAt = DateTime.Now;
                        chr.CharacterName = fchar2;
                        chr.uId = exchar.uId;
                        chr.Id = exchar.Id;
                        chr.ServerId = exchar.ServerId;
                        this.db.Entry(exchar).CurrentValues.SetValues(chr);
                        

                          await db.SaveChangesAsync();
                        }
                    

                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }

    }
}
