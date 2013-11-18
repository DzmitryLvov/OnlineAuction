using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using OnlineAuction.Buisness.Models.Account;
using OnlineAuction.Buisness.Models.Lot;
using OnlineAuction.Buisness.Data;

namespace OnlineAuction.Buisness.Data
{
    public class DataAccess
    {
        private readonly MainDataBase _dataBase = new MainDataBase();
        private readonly string INITIAL_CATALOG = AppDomain.CurrentDomain.BaseDirectory;

        const string ETALON = @"qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";

        public DataAccess()
        {
            // .CommandTimeout = 5;
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
        public  IEnumerable<Lot> LotDataBase
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
                               Description = lot.Description,
                               Name = lot.LotName
                           }; 
            }
        }

        public LotModel ConvertToViewModel(Lot lot)
        {
            var model = new LotModel
                {
                    ID = lot.ID,
                    ActualDate = lot.ActualDate,
                    Currency = lot.StartCurrency,
                    Description = lot.Description,
                    Name = lot.LotName,
                    OwnerId = lot.OwnerId
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
               //TODO: Make bets
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
       public bool CreateLot(int ownerid, string name, string description, DateTime date, Int64 currency, string typeName, object image)
       {
           try
           {
               _dataBase.Lots.Add(new Lot
               {
                   LotName = name,
                   ActualDate = date,
                   StartCurrency  = currency,
                   Description = description,
                   IsDeleted = false,
                   OwnerId = ownerid,
                   LotType = _dataBase.LotTypes.FirstOrDefault(t => t.TypeName == typeName)
               });
               _dataBase.SaveChanges();
               var path = String.Format("{0}Content\\Image\\Lots\\{1}",INITIAL_CATALOG,
                                        LotDataBase.FirstOrDefault(
                                            t => t.OwnerId == ownerid &&
                                                t.LotName == name) // TODO: Leaderame is not null
                                                  .ID);
               var img = image as HttpPostedFileBase;
               if (!Directory.Exists(path) && img.ContentLength > 0 )
               {
                   Directory.CreateDirectory(path);
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
               var dbmodel = _dataBase.Lots.FirstOrDefault(m => m.ID == id);
               if (dbmodel != null)
                   dbmodel.IsDeleted = true;
               if(_dataBase.Database.Connection.State == System.Data.ConnectionState.Closed)
               _dataBase.SaveChanges();
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
                    var index = rnd.Next(LotDataBase.Count(t => !t.IsDeleted && t.LotType.TypeName == typeName));

                    yield return
                        ConvertToViewModel(LotDataBase.Where(t => !t.IsDeleted && t.LotType.TypeName == typeName).ToList()[index]);

                    _dataBase.Lots.Remove(LotDataBase.Where(t => !t.IsDeleted && t.LotType.TypeName == typeName).ToList()[index]);
                }
            }
        }

        internal IEnumerable<string> GetLotTypeList()
        {
            return _dataBase.LotTypes.Select(type => type.TypeName );
        }

        internal bool IndexImageExits(int ID, string targetFolder)
        {
            return File.Exists(INITIAL_CATALOG +@"Content\Image\"+ targetFolder + @"\" + ID.ToString() + @"\index.jpg");
        }

        

        public object GetUserProfileViewModel(string userName)
        {
            return _dataBase.Users.FirstOrDefault(t => t.Username == userName);

        }

        internal IEnumerable<LotModel> GetHotLots()
        {
            //индусский код но что поделать TODO: разобраться с этим методом
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

        internal IEnumerable<Bet> GetBetsOfCurrentLot(int lotid)
        {
            return null;
        }

        internal  MembershipCreateStatus AddNewUser(RegisterModel model)
        {
            var hash = new HMACSHA1();
            var encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(model.Password)));

            return _dataBase.AddNewUser(model.UserName,
                encodedPassword,
                model.Question,
                model.Answer,
                model.Email,
                getBaseRoleId(),
                model.Phone,
                model.FirstName,
                model.LastName,
                model.BirthDate,
                model.PhotoLink,
                model.LocationId) == 1 ? MembershipCreateStatus.Success : MembershipCreateStatus.ProviderError;
        }

        private int? getBaseRoleId()
        {
            return _dataBase.Roles.FirstOrDefault(t => t.Rolename == "Base").ID;
        }
    }
}