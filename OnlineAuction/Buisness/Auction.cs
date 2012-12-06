using System;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models.Account;
using OnlineAuction.Buisness.Models.Item;

namespace OnlineAuction.Buisness
{
    public class Auction
    {
        public static void Start()
        {
            bool isAlive = true;

            while (isAlive)
            {
                
            }
        }

        internal static bool RestorePassword(RestorePasswordModel model)
        {
            var newpass = "";
            if (!String.IsNullOrEmpty( newpass = DataAccess.RestorePassword(model)))
            {
                //sendEmail
                return true;
            }
            return false;
        }
        static int CalculateNextInterval()
        {
            var s = DataAccess.GetDateOfCloserDeleteon();
            var r = (DateTime.Now - s).Milliseconds;
            return r;
        }
        public static bool CreateLot(CreateLotModel model, string ownername)
        {

           return DataAccess.CreateLot(ownername,model.Name, model.Description, model.ActualDate, model.Currency);
        }

        public static void MakeBet(ViewLotModel model, string username)
        {
            DataAccess.MakeBet(model.ID, username, model.Currency);
        }

        public static bool DeleteLot(ViewLotModel model)
        {
            try
            {
                DataAccess.DeleteLot(model.ID);
                
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