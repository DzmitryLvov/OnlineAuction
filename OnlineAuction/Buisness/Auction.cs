using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models;
using OnlineAuction.Buisness.Models.Item;
using OnlineAuction.Buisness.Security;

namespace OnlineAuction.Buisness
{
    public class Auction
    {
        void Start()
        {
            var isAlive = true;

        }



        internal static void RestorePassword(string email)
        {
            
        }

        public static bool CreateLot(CreateLotModel model, string ownername)
        {

           return DataAccess.CreateLot(ownername,model.Name, model.Description, model.ActualDate, model.Currency);
        }

        public static void MakeBet(ViewLotModel model, string username)
        {
            DataAccess.MakeBet(model.ID, username, model.Currency);
        }

        public static bool DeleteLot(int? id, string leadername)
        {
            try
            {
                DataAccess.DeleteLot(id);
                
                    //EmailSender.SendEmailToLeader(model);
                
                return true;
            }
            catch (ArgumentException e)
            {
                return false;
            }
        }
    }
}