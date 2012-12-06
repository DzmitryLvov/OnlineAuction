using System;
using System.Collections.Generic;
using System.Linq;
using OnlineAuction.Buisness.Models.Account;
using OnlineAuction.Buisness.Models.Item;

namespace OnlineAuction.Buisness.Data
{
    public class DataAccess
    {
        private static MainDataBase _dataBase = new MainDataBase();

        public static object DataBase
        {
            get { return _dataBase; }
        }

        public static IQueryable<Lot> LotDataBase
        {
            get { return _dataBase.Lots; }
        }

        public static IEnumerable<ViewLotModel> ConvertedActualLotCollection
        {
            get
            {
                return from lot in LotDataBase where !lot.IsDeleted orderby lot.ActualDate select new ViewLotModel
                    {
                        ID = lot.ID,
                        ActualDate = lot.ActualDate,
                        Currency = lot.Currency,
                        Description = lot.Description,
                        Name = lot.Lotname
                    }; 
                
            }
        }

        public static ViewLotModel ConvertToViewModel(Lot lot)
        {
            var model = new ViewLotModel
                {
                    ID = lot.ID,
                    ActualDate = lot.ActualDate,
                    Currency = lot.Currency,
                    Description = lot.Description,
                    Name = lot.Lotname,
                    LeaderName = lot.LeaderName,
                    OwnerName = lot.OwnerName
                };
            return model;
        }

        public static ViewLotModel GetViewModelById(int? id)
        {
            var lot = _dataBase.Lots.FirstOrDefault(l => l.ID == id);
            return lot != null ? ConvertToViewModel(lot) : null;
        }



        public static int[] GetMinVal(int id)
        {
            throw new System.NotImplementedException();
        }
        public static bool MakeBet(int lotid, string leadername, Int64 newcurrency)
        {
            try
            {
                var lot = _dataBase.Lots.First(t => t.ID == lotid);
                lot.Currency = newcurrency;
                lot.LeaderName = leadername;

                _dataBase.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
       public static bool CreateLot(string ownername, string name, string description, DateTime date, Int64 currency)
       {
           try
           {
               _dataBase.Lots.AddObject(new Lot
               {
                   Lotname = name,
                   ActualDate = date,
                   Currency = currency,
                   Description = description,
                   IsDeleted = false,
                   OwnerName = ownername
               });
               _dataBase.SaveChanges();
               return true;
           }
           catch (Exception)
           {
               return false;
           }
           
       }

       internal static bool DeleteLot(int id)
       {
           try
           {
               var dbmodel = _dataBase.Lots.FirstOrDefault(m => m.ID == id);
               if (dbmodel != null)
                   dbmodel.IsDeleted = true;
               _dataBase.SaveChanges();
               return true;
           }
           catch
           {
               return false;
           }
       }

        public static string RestorePassword(RestorePasswordModel model)
        {
            var result = "";
            var etalon = _dataBase.Users.FirstOrDefault(u => u.Username == model.UserName);
            if (etalon != null && etalon.Email == model.Email)
            {
                result = GenerateRandomPass();
                etalon.Password = result;
                _dataBase.SaveChanges();
            }
            return result;
        }

        private static string GenerateRandomPass()
        {
            var etalon = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";
            var rnd = new Random();

            var size = rnd.Next(8, 16);

            var result = "";

            for (var i = 0; i < size; i++)
            {
                result += etalon[rnd.Next(61)];
            }
            return result;
        }
        public static DateTime GetDateOfCloserDeleteon()
        {
            return _dataBase.Lots.OrderBy(m => m.ActualDate).First().ActualDate;
        }
    }
}