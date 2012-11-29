using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<LotModel> ConvertedActualLotCollection
        {
            get
            {
                return from lot in LotDataBase where !lot.IsDeleted orderby lot.ActualDate select new LotModel
                    {

                        ID = lot.ID,
                        ActualDate = lot.ActualDate,
                        Currency = lot.Currency,
                        Description = lot.Description,
                        Name = lot.Lotname
                    }; 
                
            }
        }

        public static LotModel ConvertModel(Lot lot)
        {
            return new LotModel
                {

                    ID = lot.ID,
                    ActualDate = lot.ActualDate,
                    Currency = lot.Currency,
                    Description = lot.Description,
                    Name = lot.Lotname
                };
        }

        public static LotModel GetLotById(int? id)
        {
            var lot = _dataBase.Lots.FirstOrDefault(l => l.ID == id);
            return lot != null ? ConvertModel(lot) : null;
        }

        //internal  List<Lot> GetLastActualLots()
        //{
        //    return LotDataBase;
        //}

    }
}