using System.Collections.Generic;
using MowerHelper.Models.DAL;
using MowerHelper.Models.BLL.Common;
using System.Data;
namespace MowerHelper.Models.BLL.Providers
{
    public class ProviderAdvertising
    {
       
        public int DocCount { get; set; }
public string StateNme1
{
    get;
    set;
}
public string ProviderSchedule
{
    get;
    set;
}
public int? Practice_ID
{
    get;
    set;
}
public string firstfee
{
    get;
    set;
}
public string Siteurl
{
    get;
    set;
}
public string secondaryaddress {
    get;
    set;
}
public string twitterurl {
    get;
    set;
}
public string facebookurl {
    get;
    set;
}
public int? distance {
    get;
    set;
}
public string CountryCode {
    get;
    set;
}
public string Latitude {
    get;
    set;
}
public string Longitude {
    get;
    set;
}
public string IPAddress {
    get;
    set;
}
public string StateAbb {
    get;
    set;
}

public string FirstName {
    get;
    set;
}

public string PublicURL {
    get;
    set;
}

public string WPExtension {
    get;
    set;
}
public int? NoOfRecords {
    get;
    set;
}

public int? PageNo {
    get;
    set;
}

public int VideoCount {
    get;
    set;
}

public int AudioCount {
    get;
    set;
}

public string LatitudeValue {
    get;
    set;
}

public string LongitudeValue {
    get;
    set;
}

public string LastName {
    get;
    set;
}

public int? Radius {
    get;
    set;
}

public string Gender {
    get;
    set;
}

public string IsLicenseVerified {
    get;
    set;
}

public int? City_ID {
    get;
    set;
}

public string l_GoogleMapPath {
    get;
    set;
}



public string Address1 {
    get;
    set;
}

public string Address2 {
    get;
    set;
}

public string OfficeHours {
    get;
    set;
}

public string Payments {
    get;
    set;
}

public string Description {
    get;
    set;
}

public int therprofile_id {
    get;
    set;
}

public int Country_ID {
    get;
    set;
}

public int? State_ID {
    get;
    set;
}

public string Email {
    get;
    set;
}

public string Country {
    get;
    set;
}

public int ProviderID {
    get;
    set;
}

public string ProviderFullName {
    get;
    set;
}

public string Businessname {
    get;
    set;
}

public string Address {
    get;
    set;
}

public string CityNm {
    get;
    set;
}

public string State_Name {
    get;
    set;
}

public string ZIPValue {
    get;
    set;
}

public string Vmoffice {
    get;
    set;
}
public string cellphone
{
    get;
    set;
}
public string Fax {
    get;
    set;
}

public string Webaddress {
    get;
    set;
}

public string License {
    get;
    set;
}

public int Yearsinpractice {
    get;
    set;
}

public string Degree1 {
    get;
    set;
}

public string Degree2 {
    get;
    set;
}

public string Degree3 {
    get;
    set;
}


public string SiteStatic_ID {
    get;
    set;
}

public string Memberships {
    get;
    set;
}

public string Availability {
    get;
    set;
}

public string Avgcostepersessionfrom {
    get;
    set;
}

public string Avgcosepersessionto {
    get;
    set;
}

public string Paragraph {
    get;
    set;
}

public string Picture {
    get;
    set;
}

public string State {
    get;
    set;
}
public int Reference_ID {
    get;
    set;
}
public string UserAgent {
    get;
    set;
}
public string certifications {
    get;
    set;
}

public string City {
    get;
    set;
}
public string Provider_ID
{
    get;
    set;
}
public string Display_officehours
{
    get;
    set;
}
public string IsAcceptNewPatient
{
    get;
    set;
}
public string Is_Contact_Info
{
    get;
    set;
}
public string fbcomments
{
    get;
    set;
}
public string show_schedule
{
    get;
    set;
}

public string Random_number { get; set; }

public static string fulladdress
{
    get;
    set;
}

public List<ProviderAdvertising> GetTopcitisList()
{
    var objSql = new SQLDataAccessLayer();
    return objSql.GetTopcitisList();
}

public List<ProviderAdvertising> GetCountriesBasedOnDirectoryID(int? Directory_ID)
{
    var objSql = new SQLDataAccessLayer();
    return objSql.GetCountriesBasedOnDirectoryID(Directory_ID);
}
//public string CountingProviderVisitToHisProfile(ProviderAdvertising objStatics)
//{
//    var objSql = new SQLDataAccessLayer();
//    return objSql.CountingProviderVisitToHisProfile(objStatics);
//}
public long InsertVisitorTracking(ProviderAdvertising obj)
{
    var objSql = new SQLDataAccessLayer();
    return objSql.InsertVisitorTracking(obj);
}
public List<ProviderAdvertising> GetProvidersListForAdvanceSearch(ProviderAdvertising obj, ref int TotalNoofRecords)
{
    var objSql = new SQLDataAccessLayer();
    return objSql.GetProvidersListForAdvanceSearch(obj,ref TotalNoofRecords);
}
public int CountingStateswiseSearch(string ProviderIDS, char? SiteStats_ind, char listingstats_ind, string State_Name = null, string CityNm = null)
{
    var objSql = new SQLDataAccessLayer();
    return objSql.CountingStateswiseSearch(ProviderIDS, SiteStats_ind, listingstats_ind, State_Name, CityNm);
}
public List<ProviderAdvertising> GetProviders_ListWithOutRadius_ForAdvanceSearch(ProviderAdvertising obj, ref int TotalNoofRecords)
{
    var objSql = new SQLDataAccessLayer();
    return objSql.GetProviders_ListWithOutRadius_ForAdvanceSearch(obj,ref TotalNoofRecords);
}
public List<ProviderAdvertising> GeneralListProviders(string ZipValue, int? Distance,string LastName, int CityID, int PageNo, ref int TotalNoofRecords, ref string Outmsg)
{
    var objSql = new SQLDataAccessLayer();
    return objSql.GeneralListProviders(ZipValue, Distance, LastName, CityID, PageNo,ref TotalNoofRecords,ref Outmsg);
}
public List<ProviderAdvertising> BindStatesList(string city)
{
    var objSql = new SQLDataAccessLayer();
    return objSql.BindStatesList(city);
}

public List<ProviderAdvertising> ListProvidersBasedOnStateOrCountryOrCityIDs(string CityNm, string StateID, int CountryID, int PageNo, ref int TotalNoofRecords, ref string StrOutmsg)
{
    var objSql = new SQLDataAccessLayer();
    return objSql.ListProvidersBasedOnStateOrCountryOrCityIDs(CityNm, StateID, CountryID, PageNo,ref TotalNoofRecords,ref StrOutmsg);
}
public clsCountry GetStatecitywithZip(string strzip)
{
    var objSql = new SQLDataAccessLayer();
    return objSql.GetStatecitywithZip(strzip);
}
public List<clsCountry> GetCitiesofProvidersBasedonStateID(int? StateID, string ZIP, int? Distance, int? CountryID)
{
    var obj = new DAL.CDAL.SQLDataAccessLayer();
    return obj.GetCitiesofProvidersBasedonStateID(StateID, ZIP, Distance, CountryID);
}
public List<clsCountry> GetZipcodesbasedonStatecities(int? StateID, int? CityID, string Directory_ID, string ZIP, int? Distance, string Cityname)
{
    var objSql = new SQLDataAccessLayer();
    return objSql.GetZipcodesbasedonStatecities(StateID, CityID, Directory_ID, ZIP, Distance, Cityname);
}
public ProviderAdvertising GetTheProviderProfile(string randomnumber)
{
    var objSql = new SQLDataAccessLayer();
    return objSql.GetTheProviderProfile(randomnumber);
}
public DataSet GetPreviousNextRecords(int? State_Id, int? City_Id)
{
    var objSql = new SQLDataAccessLayer();
    return objSql.GetPreviousNextRecords(State_Id, City_Id);
}
public string UpdAudioVideoStatistics(int userReference_ID, int ReferenceTypeId, int ReferenceID, int SiteStatic_ID, string State_Name = null, string CityNm = null)
{
    var objSql = new SQLDataAccessLayer();
    return objSql.UpdAudioVideoStatistics(userReference_ID, ReferenceTypeId, ReferenceID, SiteStatic_ID, State_Name, CityNm);
}
public string CountingProviderVisitToHisProfileDocument(int? Provider_ID, int Sitestatistics_ID, int Reference_ID, string State_Name = null, string CityNm = null)
{
    var objSql = new SQLDataAccessLayer();  
    return objSql.CountingProviderVisitToHisProfileDocument(Provider_ID, Sitestatistics_ID, Reference_ID, State_Name, CityNm);
}
public clsCountry GetProviderStateID(string StateNm)
{
    var objSql = new SQLDataAccessLayer();  
    return objSql.GetProviderStateID(StateNm);
}
public string GetProviderStateab(string StateNm)
{
    var objSql = new SQLDataAccessLayer();  
    return objSql.GetProviderStateab(StateNm);
}
public object GetProviderCityID(string cityname, int? stateid)
{
    var objSql = new SQLDataAccessLayer();  
    return objSql.GetProviderCityID(cityname, stateid);
}

public string UpdArticlesStatistics(int Article_ID, int SiteStatistic_ID, string statename, string cityname)
{
    var objSql = new SQLDataAccessLayer();  
    return objSql.UpdArticlesStatistics(Article_ID, SiteStatistic_ID, statename, cityname);
}

    }
    public class ProviderListPaging
    {

        

        public int PageNo { get; set; }

        public List<ProviderListPaging> GetPagingforDataList(ref int TotalNoofRecords)
        {
            var objSql = new SQLDataAccessLayer();
            return objSql.GetPagingforDataList(ref TotalNoofRecords);
        }
       
       

    }
    public class StatesList
    {
        public string State_Name { get; set; }
        public int? State_ID { get; set; }
        public int count { get; set; }
        public string ind { get; set; }
        public List<StatesList> GetStatesBasedOnDirectoryID()
        {
            var objSql = new SQLDataAccessLayer();
            return objSql.GetStatesBasedOnDirectoryID();
        }
    }
    public class Ziplist
    {
        public string City_Name { get; set; }
        public int count { get; set; }
        public static DataSet GetZipBasedOnLatLon(string latitude, string longitude)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.GetZipBasedOnLatLon(latitude, longitude);
        }

       
    }
    public class ProviderWebSites
    {
        public string File_Path { get; set; }

        public int GreetingID { get; set; }

        public string Title { get; set; }

        public string ImagePathSuffix { get; set; }


        public string CreatedOn { get; set; }

        public string DocDescription { get; set; }

        public int Document_ID { get; set; }

        public string FileName { get; set; }

        public static int TotalNoRecords { get; set; }
        public List<ProviderWebSites> GetProviderDocumentsForDifferentWebSites(int? ProviderID, int? NoofRecords, int? PageNo)
        {
            var objSql = new SQLDataAccessLayer();
            return objSql.GetProviderDocumentsForDifferentWebSites(ProviderID, NoofRecords, PageNo);
        }
        public string GetproviderWebsiteCount(int? ProviderID, string State_Name = null, string CityNm = null)
        {
            var objSql = new SQLDataAccessLayer();
            return objSql.GetproviderWebsiteCount(ProviderID, State_Name, CityNm);
        }

    }
    public class clsNearestZIPCodes
    {

        public string ZIPCode { get; set; }
    }
    public class clsNearestCities
    {
        public List<clsNearestCities> citylist { get; set; }
        public string State_Name { get; set; }

        public string CityName { get; set; }

        public string StateName { get; set; }

        public string CITY_ID { get; set; }
        public int count { get; set; }
        public string ind { get; set; }
        public void GetNearestZIPCodesandNearestCities(string ZIPCode, ref List<clsNearestCities> objCitiesList, ref List<clsNearestZIPCodes> objZIPCodesList)
        {
            var objSql = new SQLDataAccessLayer();
            objSql.GetNearestZIPCodesandNearestCities(ZIPCode,ref objCitiesList,ref objZIPCodesList);
        }

    }

}