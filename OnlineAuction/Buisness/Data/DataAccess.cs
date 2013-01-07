using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.IO;
using System.Linq;
using System.Web;
using OnlineAuction.Buisness.Models.Account;
using OnlineAuction.Buisness.Models.Lot;

namespace OnlineAuction.Buisness.Data
{
    public class DataAccess
    {
        private  readonly MainDataBase _dataBase = new MainDataBase();
        private readonly string INITIAL_CATALOG = AppDomain.CurrentDomain.BaseDirectory;

        const string ETALON = @"qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";

        public DataAccess()
        {
            _dataBase.CommandTimeout = 5;
        }
        public  object DataBase
        {
            get
            {
                lock (_dataBase)
                {
                    return _dataBase;
                }   
            }
        }
        private  MainDataBase GetDataBase()
        {
            return _dataBase;
        }
        public  ObjectSet<Lot> LotDataBase
        {
            get
            {
                    return _dataBase.Lots; 
                
            }
        }

        public  IEnumerable<LotModel> ConvertedActualLotCollection
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

        public LotModel ConvertToViewModel(Lot lot)
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

        public  LotModel GetViewModelById(int id)
        {
            var lot = LotDataBase.FirstOrDefault(l => l.ID == id);
            return lot != null ? ConvertToViewModel(lot) : null;
        }

        public bool MakeBet(int lotid, string leadername, Int64 newcurrency)
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
       public bool CreateLot(string ownername, string name, string description, DateTime date, Int64 currency, string typeName, object image)
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
                   OwnerName = ownername,
                   LotType = _dataBase.LotTypes.FirstOrDefault(t => t.TypeName == typeName)
               });
               _dataBase.SaveChanges();
               var path = String.Format("{0}\\Image\\Lots\\{1}",INITIAL_CATALOG,
                                        LotDataBase.FirstOrDefault(
                                            t => t.OwnerName == ownername &&
                                                t.Lotname == name
                                                && t.LeaderName == null)
                                                  .ID);
               if (!Directory.Exists(path))
               {
                   Directory.CreateDirectory(path);
                   var img = image as HttpPostedFileBase;
                   if (img.ContentLength > 0)
                       img.SaveAs(path + @"\index.jpg");
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

       internal bool DeleteLot(int id)
       {
           try
           {
               var db = new MainDataBase();
               var dbmodel = db.Lots.FirstOrDefault(m => m.ID == id);
               if (dbmodel != null)
                   dbmodel.IsDeleted = true;
               db.SaveChanges();
               return true;
           }
           catch(Exception e)
           {
               throw;
           }
       }

        public string RestorePassword(RestorePasswordModel model)
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

        private string GenerateRandomPass()
        {
            var rnd = new Random();

            var size = rnd.Next(32, 36);

            var result = "";

            for (var i = 0; i < size; i++)
            {
                result += ETALON[rnd.Next(61)];
            }
            return result;
        }

        public  IQueryable<Lot> GetCollectionToDelete()
        {
            var s = from lot in LotDataBase where !lot.IsDeleted && lot.ActualDate < DateTime.Now select lot;
            
            return s.AsQueryable();
        }

        internal IEnumerable<LotModel> GetLotTypePreviewCollection(string typeName)
        {
            var rnd  = new Random();
            int count;
            if ((count = (LotDataBase.Count(t => !t.IsDeleted && t.LotType.TypeName == typeName ) >= 3)?3:LotDataBase.Count(t =>!t.IsDeleted && t.LotType.TypeName == typeName)) > 0)
            {
                for (var i = 0; i < count; i++)
                {
                    var index = rnd.Next(LotDataBase.Count(t => t.LotType.TypeName == typeName));

                    yield return
                        ConvertToViewModel(LotDataBase.Where(t => t.LotType.TypeName == typeName).ToList()[index]);

                    LotDataBase.DeleteObject(LotDataBase.Where(t => t.LotType.TypeName == typeName).ToList()[index]);
                }
            }
        }

        internal IEnumerable<string> GetLotTypeList()
        {
            return _dataBase.LotTypes.Select(type => type.TypeName );
        }

        internal bool IndexImageExits(int ID, string targetFolder)
        {
            return File.Exists(INITIAL_CATALOG +@"\Image\"+ targetFolder + @"\" + ID.ToString() + @"\index.jpg");
        }

        internal void CreateUserData(string location, string phone, string firstname, string lastname, string username)
        {
            var user = _dataBase.Users.FirstOrDefault(t => t.Username == username);
            if (user != null)
            {
                user.Phone = phone;
                user.LastName = lastname;
                user.FirstName = firstname;
                user.Location = location;
            }

            _dataBase.SaveChanges();
        }

        public object GetUserProfileViewModel(string userName)
        {
            return _dataBase.Users.FirstOrDefault(t => t.Username == userName);

        }

        internal IEnumerable<LotModel> GetHotLots()
        {
            //индусский код но что поделать
            var tempdate = DateTime.Now + new TimeSpan(1,0,0,0);
            foreach (var lot in _dataBase.Lots.Where(t => t.ActualDate > tempdate))
            {
                yield return new DataAccess().ConvertToViewModel(lot);
            }

        }

        public bool UserIsAdmin(string username)
        {
           return _dataBase.Users.FirstOrDefault(t => t.Username == username).Role.Rolename == "admin";
        }

        internal void SetAdmin(string username)
        {
            _dataBase.Users.FirstOrDefault(t => t.Username == username).Role =
                _dataBase.Roles.FirstOrDefault(t => t.Rolename == "amdin");
        }
    }
}