﻿using System;
using System.Threading;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models;
using OnlineAuction.Buisness.Models.Account;
using OnlineAuction.Buisness.Models.Lot;

namespace OnlineAuction.Buisness
{
    public class Auction
    {
        private static DataAccess dataAccess = new DataAccess();
        public static void Start()
        {
            var t = new Thread(LotTimeOutChecker);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
        }

        static void LotTimeOutChecker()
        {
            /*var isAlive = true;
            while (isAlive)
            {
                Thread.Sleep(5000);
                foreach (var currentLot in dataAccess.GetCollectionToDelete())
                {
                    DeleteLot(dataAccess.ConvertToViewModel(currentLot));
                }
                
            }*/
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
        
        public static bool CreateLot(CreateLotModel model, string ownername, object image)
        {
           return dataAccess.CreateLot(ownername,model.Name, model.Description, model.ActualDate, model.Currency,model.LotType, image);
        }

        public static void MakeBet(LotModel model, string username)
        {
            if (model.LeaderName != null && username != model.LeaderName)
            {
                EmailSender.ToLeaderOnChangedRate(model, username);
            }
            dataAccess.MakeBet(model.ID, username, model.Currency);
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
                dataAccess.DeleteLot(currentLot.ID);
                
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}