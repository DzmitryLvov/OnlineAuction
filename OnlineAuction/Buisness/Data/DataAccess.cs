using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects;
using System.IO;
using System.Linq;
using System.Net;
using OnlineAuction.Buisness.Models.Account;
using OnlineAuction.Buisness.Models.Item;

namespace OnlineAuction.Buisness.Data
{
    public class DataAccess
    {
        private static readonly MainDataBase _dataBase = new MainDataBase();
        private const string INITIAL_CATALOG = @"E:\Prog\OnlineAuction\OnlineAuction\Content";

        static DataAccess()
        {
            _dataBase.CommandTimeout = 5;
        }
        public static object DataBase
        {
            get
            {
                    return _dataBase;
            }
        }
        private static MainDataBase GetDataBase()
        {
            return _dataBase;
        }
        public static ObjectSet<Lot> LotDataBase
        {
            get
            {
                //lock (_dataBase)
                //{
                    return _dataBase.Lots; 
                //}
            }
        }

        public static IEnumerable<LotModel> ConvertedActualLotCollection
        {
            get
            {
                    return from lot in LotDataBase
                           where !lot.IsDeleted
                           orderby lot.ActualDate
                           select new LotModel
                           {
                               ID = lot.ID,
                               ActualDate = lot.ActualDate,
                               Currency = lot.Currency,
                               Description = lot.Description,
                               Name = lot.Lotname
                           }; 
            }
        }

        public static LotModel ConvertToViewModel(Lot lot)
        {
            var model = new LotModel
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

        public static LotModel GetViewModelById(int id)
        {
            var lot = LotDataBase.FirstOrDefault(l => l.ID == id);
            return lot != null ? ConvertToViewModel(lot) : null;
        }

        public static bool MakeBet(int lotid, string leadername, Int64 newcurrency)
        {
            try
            {
                var lot = LotDataBase.First(t => t.ID == lotid);
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
               LotDataBase.AddObject(new Lot
               {
                   Lotname = name,
                   ActualDate = date,
                   Currency = currency,
                   Description = description,
                   IsDeleted = false,
                   OwnerName = ownername
               });
               _dataBase.SaveChanges();
               var path = String.Format("{0}\\Lots\\{1}",INITIAL_CATALOG,
                                        LotDataBase.FirstOrDefault(
                                            t => t.OwnerName == ownername &&
                                                t.Lotname == name
                                                && t.LeaderName == null)
                                                  .ID);
               if (!Directory.Exists(path))
               {
                   Directory.CreateDirectory(path);
               }
               else
               {
                   foreach (var filePath in Directory.GetFiles(path))
                   {
                       File.Delete(filePath);
                   }
               }
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
               var dbmodel = LotDataBase.FirstOrDefault(m => m.ID == id);
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

            var size = rnd.Next(32, 36);

            var result = "";

            for (var i = 0; i < size; i++)
            {
                result += etalon[rnd.Next(61)];
            }
            return result;
        }

        public static ParallelQuery<Lot> GetCollectionToDelete()
        {
            var s = from lot in LotDataBase.AsParallel() where !lot.IsDeleted && lot.ActualDate < DateTime.Now select lot;
            
            return s;


            //return result;
        }
    }
}