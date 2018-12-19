using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MowerHelper.Models.DAL.DataAccessLayer;
using MowerHelper.Models.DAL;
namespace MowerHelper.Models.BLL.Providers
{
    public class Provider_Sitestastics
    {
        public static IEnumerable<SelectListItem> Years
        {
            get;
            set;
        }
        public Provider_Sitestastics()
{
}
        public int Provider_ID
        {
            get;
            set;
        }
public string Year
{
    get;
    set;
}
public string statistics_name { get;set;}
public string January { get;set;}           
public string February { get;set;}           
public string March  { get;set;}           
public string April { get;set;}           
public string May   { get;set;}         
public string June  { get;set;}
public string July  { get;set;}
public string August  { get;set;}
public string September  { get;set;}
public string October  { get;set;}
public string November  { get;set;}
public string December { get; set; }
public string ShowStats_Ind
{
    get;
    set;
}
public int StatisticsSetting_ID
{
    get;
    set;
}
public int SiteStatistic_ID
{
    get;
    set;
}
public string Title
{
    get;
    set;
}
public int ViewedCount
{
    get;
    set;
}
public string IsDefault
{
    get;
    set;
}
public string CreatedOn
{
    get;
    set;
}
public static List<Provider_Sitestastics> GET_Statistics(Provider_Sitestastics objstatastics)
{
    SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
    return DBLayer.GET_Statistics(objstatastics);
}
public static Provider_Sitestastics Get_StartYearOfProvider(int? Provider_ID)
{
    SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
    return DBLayer.Get_StartYearOfProvider(Provider_ID);
}

    }
}