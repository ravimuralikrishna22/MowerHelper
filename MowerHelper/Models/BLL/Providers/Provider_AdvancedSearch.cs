using System.Collections.Generic;
using MowerHelper.Models.DAL;
using System.Web.Mvc;
using System.Data;
namespace MowerHelper.Models.BLL.Providers
{
    public class Provider_AdvancedSearch
    {
        SQLDataAccessLayer objSql = new SQLDataAccessLayer();
        public IEnumerable<SelectListItem> CityList { get; set; }
        public IEnumerable<SelectListItem> ServiceList { get; set; }
public Provider_AdvancedSearch()
{
}
        public string freesession { get; set; }


        public string FeeRange_From { get; set; }

        public string FeeRange_To { get; set; }

        public int? TherputicApproach_ID { get; set; }

public int? Provider_ID { get; set; }

        public int Payer_ID { get; set; }

        public string PayerName { get; set; }

        public string session_IDs { get; set; }

        public string IssueIds { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Paragraph { get; set; }

        public string URL { get; set; }

        public string ClientProfile { get; set; }

        public string Payments { get; set; }

       public string otherservices { get; set; }
        public string Out_msg { get; set; }
        public string DisplayFee_ind { get; set; }
        public string Yearinpractice
        {
            get;
            set;
        }
        public string Degree1
        {
            get;
            set;
        }
        public string School1
        {
            get;
            set;
        }
        public string Degree2
        {
            get;
            set;
        }
        public string School2
        {
            get;
            set;
        }
        public string Degree3
        {
            get;
            set;
        }
        public string School3
        {
            get;
            set;
        }
        public string Payer_IDs { get; set; }
        public string Is_Contact_Info { get; set; }
        public string UpdatedBy { get; set; }



        public string Longitude { get; set; }
        public string Lattitude { get; set; }
        public string Picture { get; set; }
        public string StateName
        {
            get;
            set;
        }

        public string ZIPValue
        {
            get;
            set;
        }
        //public string Fbcomments
        //{
        //    get;
        //    set;
        //}
public int? City_ID
{
    get;
    set;
}

public int? Country_ID
{
    get;
    set;
}

public int? Distance
{
    get;
    set;
}

public string CityName
{
    get;
    set;
}

public string IsSearch
{
    get;
    set;
}

public string NoAdvertising {
    get;
    set;
}

public string Address1 {
    get;
    set;
}

public string ProviderName {
    get;
    set;
}

public string Zip
{
    get;
    set;
}

public int? State_ID {
    get;
    set;
}
public string showschedule { get; set; }
public string facebookurl { get; set; }

public string twitterurl { get; set; }
public string fbcomments
{
    get;
    set;
}
public string Website { get; set; }
public static string Insert_AdvancedSearchInfo(Provider_AdvancedSearch ObjInsAdvancedSearch)
{
    SQLDataAccessLayer DALLayer = new SQLDataAccessLayer();
    return DALLayer.Insert_AdvancedSearchInfo(ObjInsAdvancedSearch);
}

public static DataSet list_Featuredtherapists(Provider_AdvancedSearch objData)
{
    SQLDataAccessLayer dblayer = new SQLDataAccessLayer();
    return dblayer.list_Featuredtherapists(objData);
}

    }
}