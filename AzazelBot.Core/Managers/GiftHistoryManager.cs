using AzazelBot.Core.Data;
using AzazelBot.Core.Data.Models;
using BotGear.Managers;
using BotGear.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzazelBot.Core.Managers
{
    public class GiftHistoryManager
    {
        AZBContext db = new AZBContext();
        UserManager usermngr = new UserManager();
        public async Task AddGift(string uid, string fchar, string sid)
        {
            try
            {
                var user = await usermngr.GetUserbyId(uid);
                
                


                if (fchar != null && user != null && sid != null)
                {
                   
                        if (fchar != null)
                        {

                            
                                var exchar = await this.ExistGift(uid,fchar, sid);
                                if (exchar == null)
                                {
                                   GiftHistory gift = new GiftHistory();
                                    gift.GiftedAt = DateTime.Now;
                                    gift.ServerId = sid;
                                    gift.Year = gift.GiftedAt.Year;

                                   gift.CharacterName = fchar;
                                    gift.uId = user.Id;
                                    db.GiftHistory.Add(gift);
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

        public async Task<GiftHistory> ExistGift(string uid, string fchar, string sid)
        {
            try
            {
                var ap = db.GiftHistory.FirstOrDefault(x => x.uId == uid && x.CharacterName == fchar && x.ServerId == sid);

                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<Boolean> isGiftGiven(string uid, string sid,int year)
        {
            try
            {
                Boolean ap = false;
                var gift = db.GiftHistory.FirstOrDefault(x => x.uId == uid  && x.ServerId == sid &&x.Year==year);
                if ( gift !=null)
                {
                    ap = true;
                }

                return ap;

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return false;
            }
        }
        public async Task <List<GiftHistory> >GetUsersGiftsByCharacterName(string uid, string charname)
        {
            try
            {
                List<GiftHistory> ap = null;
                if (charname != null && uid != null)
                {
                    var gifts = await this.GetUserGift(uid);
                    if (gifts != null)
                    {

                        ap =  gifts.Where(x => x.CharacterName == charname).ToList();
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
        public async Task<GiftHistory> GetUsersGiftById(string uid, int id)
        {
            try
            {
               GiftHistory ap = null;

                var chars =await  this.GetUserGift(uid);
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
        public async Task<List<GiftHistory>> GetUserGift(string uid)
        {
            try
            {
                return db.GiftHistory.Where(x => x.uId == uid).ToList();

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task<List<GiftHistory> >GetGiftHistory()
        {
            try
            {
                return db.GiftHistory.ToList();

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public async Task DelGift(string uid, string fchar, string sid)
        {
            try
            {
                var user = await usermngr.GetUserbyId(uid);
              

                if (fchar != null && user != null && sid != null)
                {
                   
                    
                        var exchar = await this.ExistGift(uid, fchar, sid);
                        if (exchar != null)
                        {
                            db.GiftHistory.Remove(exchar);
                            await db.SaveChangesAsync();
                        }
                    }

                }

            
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        public async Task DelGiftsByUserId(string uid)
        {
            try
            {
                var user = await usermngr.GetUserbyId(uid);




                if (user != null)
                {


                    var exchar = await this.GetUserGift(uid);
                    if (exchar == null)
                    {



                        if (exchar != null)
                        {

                            foreach (var e in exchar)
                            {

                                db.GiftHistory.Remove(e);
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
        public async Task DelAllGifts()
        {
            try
            {




                var gifts = await this.GetGiftHistory();
                if (gifts != null)

                {
                    foreach(var g in gifts)
                    {
                       await  this.DelGiftByid(g.Id);
                    }
                }




            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }

        public async Task DelGiftByid( int id)
        {
            try
            {







                var exchar = await this.GetGiftHistory() ;
                
                    if (exchar != null)
                    {


                    var gift = exchar.FirstOrDefault(x => x.Id == id);
                    if (gift != null)
                    {
                        await db.SaveChangesAsync();

                    }
                    }


                

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        public async Task DelGiftsByServerid(string  sid)
        {
            try
            {







                var gifts= await this.GetUsersGiftByServerId(sid);

                if (gifts != null)
                {


                   
                   foreach(var g in gifts)
                    {
                        await this.DelGiftByid(g.Id);
                    }
                }




            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        public async Task<List<GiftHistory>> GetUsersGiftByServerId( string sid)
        {
            try
            {
                List<GiftHistory>  ap = null;

                var chars = await this.GetGiftHistory();
                if (chars != null)
                {

                    ap = chars.Where(x => x.ServerId == sid).ToList();
                }



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
