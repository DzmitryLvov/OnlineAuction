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

namespace OnlineAuction.Buisness.Data
{
    public class DataAccess
    {
        private readonly MainDataBase _dataBase = new MainDataBase();
        private readonly string INITIAL_CATALOG = AppDomain.CurrentDomain.BaseDirectory;

        const string ETALON = @"qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890"; //для генерации пароля
        private const string ENCRYPTION_KEY = "AE09F72BA97CBBB5"; // для генерации хэша HMACSHA1


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
        public  IEnumerable<Lot> LotDataBase
        {
            get
            {
                    return _dataBase.Lots; 
                
            }
        }

        public  IEnumerable<LotPreviewModel> GetConvertedActualLotCollection()
        {
            foreach (var m in _dataBase.Lots)
            {
                if(!m.IsDeleted)
                yield return new LotPreviewModel()
                       {
                           ID = m.ID,
                           LotName = m.LotName,
                           Currency =  (GetmaxBetValue(m.ID) == -1) ? (int)m.StartCurrency : (GetmaxBetValue(m.ID)),
                           PhotoLink =  IndexPhotoLink(m.ID),
                           ActualDate = m.ActualDate
                       };
            }
                       
        }

        private string IndexPhotoLink(int id)
        {
           return IndexImageExits(id, "Lots")
                            ? @"Content/Image/Lots/" + id + @"/index.jpg"
                            : @"Content/Image/Lots/Default/index.jpg";
        }

        public  LotViewModel GetViewModelById(int id)
        {
            var lot = _dataBase.Lots.FirstOrDefault(t => t.ID == id);
            var comments = _dataBase.CommentInfos.Where(t => t.LotID == id);
            return lot != null ? new LotViewModel() 
            {
                Model = new LotInfo()
                {
                    LotName = lot.LotName,
                    ID = lot.ID,
                    ActualDate = lot.ActualDate,
                    Description = lot.Description,
                    IsDeleted = lot.IsDeleted,
                    MaxBet = GetmaxBetValue(lot.ID),
                    Username = lot.User.Username
                },
                Comments = comments.Select(
                t => new CommentViewModel
                {
                    CommentDate = t.CommentDate,
                    CommentText = t.CommentText,
                    ID = t.ID,
                    IsApproved = t.IsApproved,
                    LotID = t.LotID,
                    Userid = t.Userid,
                    Username = t.Username
                })}: null;
        }
       
       public bool CreateLot(string ownerName,
           string name, 
           string description, 
           DateTime date, 
           int currency, 
           int[] selectedSubCategories,
           object image)
       {
               var id  =  new ObjectParameter("UserId",typeof(int));
               _dataBase.GetUserIdByName(ownerName, id);
               var typeid = new ObjectParameter("TypeId", typeof (int));
               _dataBase.GetBasicLotTypeId(typeid);

           var inserted = new ObjectParameter("id", typeof (int));
               _dataBase.AddNewLot(name,
                   description,
                   currency,
                   date,
                   (int)id.Value,
                   (int)typeid.Value,
                   inserted);
           foreach (var selectedId in selectedSubCategories)
           {
               _dataBase.LotSubCategories.Add(new LotSubCategory
               {
                   LotID = (int) inserted.Value,
                   SubCategoryID = selectedId
               });
           }
               _dataBase.SaveChanges();
               var path = String.Format("{0}Content\\Image\\Lots\\{1}",INITIAL_CATALOG,
                                            (int) inserted.Value);
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


        internal IEnumerable<string> GetLotTypeList()
        {
            return _dataBase.LotTypes.Select(type => type.TypeName );
        }

        internal bool IndexImageExits(int ID, string targetFolder)
        {
            return File.Exists(INITIAL_CATALOG +@"Content\Image\"+ targetFolder + @"\" + ID.ToString() + @"\index.jpg");
        }

        

        public UserFullInfo GetUserProfileViewModel(string userName)
        {
            var result = new UserFullInfo(); 
            var model =  _dataBase.Users.FirstOrDefault(t => t.Username == userName);
            if (model != null)
            {
                result.ID = model.ID;
                result.Username = model.Username;
                result.IsDeleted = model.IsDeleted;
                result.Rolename = model.Role.Rolename;

                var userData = _dataBase.UserDatas.FirstOrDefault(t => t.User_ID == model.ID);
                if (userData != null)
                {
                    result.FirstName = userData.FirstName;
                    result.LastName = userData.LastName;
                    result.Phone = userData.Phone;
                    result.PhotoLink = userData.PhotoLink;
                    result.LocationName = userData.Location.LocationName;
                }
                return result;
            }
            return null;
        }

        internal IEnumerable<LotViewModel> GetHotLots()
        {
            //TODO: разобраться с этим методом
            var tempdate = DateTime.Now + new TimeSpan(1,0,0,0);
            return _dataBase.LotInfos.Where(t => t.ActualDate > tempdate).Select(lot => new LotViewModel(){Model = lot});
        }

        public bool UserIsAdmin(string username)
        {
           return _dataBase.Users.FirstOrDefault(t => t.Username == username).Role.Rolename == "admin";
        }

        internal void SetAdmin(string username)
        {
            try
            {
                var id = new ObjectParameter("id", typeof(int));
                _dataBase.GetAdministratorRoleId(id);

                _dataBase.TrySetUserRole(username, (int)id.Value);
            }
            catch (Exception e)
            {

            }
        }

        internal IEnumerable<Bet> GetBetsOfCurrentLot(int lotid)
        {
            return null;
        }

        internal  MembershipCreateStatus AddNewUser(RegisterModel model)
        {
            try
            {
                var hash = new HMACSHA1()
                {
                    Key = HexToByte(ENCRYPTION_KEY)
                };
                var encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(model.Password)));
                var count = _dataBase.AddNewUser(model.UserName,
                    encodedPassword,
                    model.Question,
                    model.Answer,
                    model.Email,
                    getBaseRoleId(),
                    model.Phone,
                    model.FirstName,
                    model.LastName,
                    null, //model.BirthDate,
                    model.PhotoLink,
                    model.LocationId);
                return count == 2 ? MembershipCreateStatus.Success : MembershipCreateStatus.ProviderError;
            }
            catch (Exception ex)
            {
                return MembershipCreateStatus.DuplicateUserName;
            }
        }
        private static byte[] HexToByte(string hexString)
        {
            var returnBytes = new byte[hexString.Length / 2];
            for (var i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return returnBytes;
        }

        private int? getBaseRoleId()
        {
            return _dataBase.Roles.FirstOrDefault(t => t.Rolename == "Simpleuser").ID;
        }

        internal IEnumerable<Location> GetLocations()
        {
            foreach (var location in _dataBase.Locations) // TODO:не превращать в linq, отвалится dropdown binding
            {
                yield return new Location()
                {
                    ID = location.ID,
                    LocationName = location.LocationName
                };
            }
        }



        internal IEnumerable<SubCategory> GetSubCategoriesList()
        {
            foreach (var subCategory in _dataBase.SubCategories)
            {
                yield return new SubCategory()
                {
                    ID = subCategory.ID,
                    SubCategoryName = subCategory.SubCategoryName
                };
            }
        }

        internal int StartCompletingLots()
        {

            var i = _dataBase.CursorCompleteLots();
            return i;
        }

        public IEnumerable<LotViewModel> GetLotTypePreviewCollection(string typeName)
        {
            return null;
        }

        internal IEnumerable<BetInfo> GetBetCollection(int id)
        {
            foreach (var bet in _dataBase.BetInfos.OrderBy(t => t.BetDate))
            {
                if(bet.LotID == id)
                yield return new BetInfo()
                {
                    BetDate = bet.BetDate,
                    BetValue = bet.BetValue,
                    ID = bet.ID,
                    LotID = bet.LotID,
                    UserID = bet.UserID,
                    Username = bet.Username
                };
            }
        }

        internal int GetmaxBetValue(int id)
        {
            int tempValue;
            var betValue = new ObjectParameter("Value", typeof (int));
            _dataBase.GetMaxBet(id, betValue);
            if (int.TryParse(betValue.Value.ToString(), out tempValue))
                return (int) betValue.Value;
            else return -1;
        }

        internal bool MakeBet(string userName, int lotId, int betValue)
        {
            try
            {
                var userid = new ObjectParameter("UserId", typeof (int));
                _dataBase.GetUserIdByName(userName, userid);
                _dataBase.MakeBet((int) userid.Value, lotId, betValue, DateTime.Now);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        internal string LeaveComment(int lotid, string userName, string commentText)
        {
            try
            {
                var userid = new ObjectParameter("UserId", typeof (int));
                _dataBase.GetUserIdByName(userName, userid);


                _dataBase.LeaveComment((int) userid.Value, lotid, commentText.Trim());
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public IEnumerable<UserUnapprovedComment> GetUnApprovedComments()
        {
            foreach (var m in _dataBase.UserUnapprovedComments)
            {
                yield return new UserUnapprovedComment()
                {
                    CommentId = m.CommentId,
                    CommentText = m.CommentText,
                    CommentDate = m.CommentDate,
                    ID = m.ID,
                    Username = m.Username
                };
            }
        }

        internal void BanUser(string username)
        {
            _dataBase.Users.FirstOrDefault(t => t.Username == username).IsDeleted = true;
            _dataBase.SaveChanges();
        }

        internal void UnBanUser(string username)
        {
            _dataBase.Users.FirstOrDefault(t => t.Username == username).IsDeleted = false;
            _dataBase.SaveChanges();
        }

        internal IEnumerable<UserRoleInfo> GetUsersRolesInfo()
        {
            return _dataBase.UserRoleInfos;
        }

        internal IEnumerable<LotByLocation> GetLotsLocations()
        {
            return _dataBase.LotByLocations;
        }

        internal void BanComment(int id)
        {
            _dataBase.Comments.First(t => t.ID == id).IsApproved = false;
            _dataBase.SaveChanges();
        }

        internal IEnumerable<Category> GetCategoriesCollection()
        {
            return _dataBase.Categories;
        }

        internal IEnumerable<LotPreviewModel> SearchLotBuCategoryId(int id)
        {
            return null;
        }
    }
}