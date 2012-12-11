using System;
using System.Threading;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models;
using OnlineAuction.Buisness.Models.Account;
using OnlineAuction.Buisness.Models.Item;

namespace OnlineAuction.Buisness
{
    public class Auction
    {
        public static void Start()
        {
            /*var t = new Thread(LotTimeOutChecker) {IsBackground = true};
            t.Priority = ThreadPriority.Lowest;
            t.Start();*/
        }

        static void LotTimeOutChecker()
        {
            var isAlive = true;
            while (isAlive)
            {
                Thread.Sleep(5000);
                foreach (var currentLot in DataAccess.GetCollectionToDelete())
                {
                    DeleteLot(DataAccess.ConvertToViewModel(currentLot));
                }
                
            }
        }

        internal static bool RestorePassword(RestorePasswordModel model)
        {
            var newpass = "";
            if (!String.IsNullOrEmpty( newpass = DataAccess.RestorePassword(model)))
            {
                EmailSender.SendResetEmail(model.Email, model.UserName, newpass);
                return true;
            }
            return false;
        }
        
        public static bool CreateLot(CreateLotModel model, string ownername)
        {
           return DataAccess.CreateLot(ownername,model.Name, model.Description, model.ActualDate, model.Currency);
           
        }

        public static void MakeBet(LotModel model, string username)
        {
            if (model.LeaderName != null && username != model.LeaderName)
            {
                EmailSender.ToLeaderOnChangedRate(model, username);
            }
            DataAccess.MakeBet(model.ID, username, model.Currency);
        }

        public static bool DeleteLot(LotModel currentLot)
        {
            try
            {
                if (currentLot.LeaderName != null)
                {
                    EmailSender.ToOwnerOnComplete(currentLot, currentLot.LeaderName);
                    EmailSender.ToLeaderOnComplete(currentLot);
                }
                else
                {
                    EmailSender.ToOwnerOnComplete(currentLot);
                }
                DataAccess.DeleteLot(currentLot.ID);
                
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}