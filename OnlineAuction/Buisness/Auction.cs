using System;
using System.Threading;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models;
using OnlineAuction.Buisness.Models.Account;
using OnlineAuction.Buisness.Models.Lot;
using System.Collections.Generic;
using System.Data.Objects;

namespace OnlineAuction.Buisness
{
    public class Auction
    {
        private static DataAccess dataAccess = new DataAccess();
        private static bool _isAlive = true;
        public static void Start()
        {
            var t = new Thread(LotTimeOutChecker) {Priority = ThreadPriority.Lowest};
            t.Start();
        }

        static void LotTimeOutChecker()
        {
            while (_isAlive)
            {
                Thread.Sleep(7000);
                dataAccess.StartCompletingLots();

            }
        }

        internal static bool RestorePassword(RestorePasswordModel model)
        {
            var newpass = "";
            if (!String.IsNullOrEmpty( newpass = dataAccess.RestorePassword(model)))
            {
                EmailSender.SendResetEmail(model.Email, model.UserName, newpass);
                return true;
            }
            return false;
        }
        
        public static bool CreateLot(CreateLotModel model, string ownername)
        {
           return dataAccess.CreateLot(ownername, model.Name,model.Description,model.ActualDate,model.StartCurrency, model.SubCategoryId);
           
        }

        public static void MakeBet(LotViewModel model, string username)
        { 
        }

        public static bool DeleteLot(LotViewModel currentLot)
        {
           try
            {
                
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}