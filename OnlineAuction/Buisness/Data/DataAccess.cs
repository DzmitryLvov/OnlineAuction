using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models.Home;

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


        //internal  List<Lot> GetLastActualLots()
        //{
        //    return LotDataBase;
        //}

    }
}