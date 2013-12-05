using System;
using System.Threading;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models.Lot;

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

        
        public static string CreateLot(CreateLotModel model, string ownername)
        {
            try
            {
                if (dataAccess.CreateLot(ownername, model.Name, model.Description, model.ActualDate, model.StartCurrency, model.SelectedIds
                    )) ;
                return null;
            }
            catch (Exception e)
            {
                return e.InnerException.Message;
            }
           
           
        }


        public static bool DeleteLot(int id)
        {
           try
           {
               new MainDataBase().DeleteLot(id);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}