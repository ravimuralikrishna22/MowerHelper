using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.BLL.Common;
using MowerHelper.Models.DAL.DataAccessLayer;
using MowerHelper.Models.DAL;
namespace MowerHelper.Models.BLL.Common
{
    public class clsCountry
    {
        public string Language
        {
            get;
            set;
        }
        public int LanguageID
        {
            get;
            set;
        }
        //public string LATITUDE
        //{
        //    get;
        //    set;
        //}
        //public string LONGITUDE
        //{
        //    get;
        //    set;
        //}
        public string Displaybold
        {
            get;
            set;
        }
        //public string StateAbb
        //{
        //    get;
        //    set;
        //}
        public int IssueID
        {
            get;
            set;
        }
        public string Issue
        {
            get;
            set;
        }
        //public int SpecializationID
        //{
        //    get;
        //    set;
        //}
        //public string Specialization
        //{
        //    get;
        //    set;
        //}
        //public string Directory
        //{
        //    get;
        //    set;
        //}
        public string Zipcode
        {
            get;
            set;
        }
        //public int County_ID
        //{
        //    get;
        //    set;
        //}

        //public string CountyName
        //{
        //    get;
        //    set;
        //}

        //public int CountInCounty
        //{
        //    get;
        //    set;
        //}

        //public int CountryId
        //{
        //    get;
        //    set;
        //}

        public int CountInCity
        {
            get;
            set;
        }

        //public string CountryName
        //{
        //    get;
        //    set;
        //}

        public int StateId
        {
            get;
            set;
        }

        public string StateName
        {
            get;
            set;
        }

        public string StateFullName
        {
            get;
            set;
        }

        public string CityName
        {
            get;
            set;
        }

        public int CityId
        {
            get;
            set;
        }

        //public string Abbrevation
        //{
        //    get;
        //    set;
        //}
        public clsCountry()
{
}

//public clsCountry(string _CityName, int _CityId)
//{
//    CityName = _CityName;
//    CityId = _CityId;
//}

//public clsCountry(int _CountryId, string _CountryName)
//{
//    CountryId = _CountryId;
//    CountryName = _CountryName;
//}

//public clsCountry(int _StateId, string _StateName, string _StateFullName)
//{
//    StateId = _StateId;
//    StateName = _StateName;
//    StateFullName = _StateFullName;
//}

//public clsCountry(int _StateId, string _StateName, string _StateFullName, string _Abbrevation)
//{
//    StateId = _StateId;
//    StateName = _StateName;
//    StateFullName = _StateFullName;
//    Abbrevation = _Abbrevation;
//}
public static List<clsCountry> GetStatesByCountryId(int CountryId)
{

    SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
    return DBLayer.GetStatesByCountryId(CountryId);

}
       

public static List<clsCountry> GetCityByStateId(int StateId)
{
    SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
    return DBLayer.GetCityByStateId(StateId);
}
public static List<clsCountry> Reg_GetStatesByZipCode(string ZipCode)
{
    SQLDataAccessLayer objDAL = new SQLDataAccessLayer();
    return objDAL.Reg_GetStatesByZipCode(ZipCode);
}
public static List<clsCountry> Reg_GetCityByZipCode(string ZipCode, int? State_ID)
{
    SQLDataAccessLayer objDAL = new SQLDataAccessLayer();
    return objDAL.Reg_GetCityByZipCode(ZipCode, State_ID);
}


    }
}