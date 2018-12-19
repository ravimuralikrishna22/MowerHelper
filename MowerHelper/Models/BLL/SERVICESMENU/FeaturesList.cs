using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;
namespace MowerHelper.Models.BLL.SERVICESMENU
{
    public class FeaturesList
    {
        //private string _Billingservice_ID;
        //private string _Title;
        //private string _Description;
        //private string _ListingFeature_ID;
        //private string _IsActive;
        //static int _NoOfSites;
        //static string _TotalNoRecords;
        //static int _NoofRecords;
        //private string _BillChargeTran_ID;
        //private string _ListingI;
        //private string _LisitngII;
        //private string _ListingIII;
        //private int _Provider_ChildWebsite_ID;
        //private int _Childwebsite_ID;
        //private string _WebsiteTitle;
        //private int _No_of_Sites;
        SQLDataAccessLayer ObjSql = new SQLDataAccessLayer();
        //private int? _Provider_ID;
        //private int? _ProviderLogin_ID;
        //private int? _Practice_ID;
        //private string _FilePath;
        //private string _ImagePath;
        //private string _EncryptedFilepath;
        //private string _IsAudio;
        //private string _IsVideo;
        //private int _Provider_Directory_ID;
        //private int _Directory_ID;
        //private string _EncryptedImagePath;
        //private int? _Provider_Greeting_ID;
        //private string _ImagePathSuffix;
        //private string _ConcateString;
        //private int _BSChargeTemplate_ID;
        //private string _Status_Ind;
        //private int _BillingChargeTran_ID;
        //private int? _ViewedCount;
        //private double? _Document_Size;
        //private string _Embed_Text;

//        public static int NoofRecords {
//    get { return _NoofRecords; }
//    set { _NoofRecords = value; }
//}
        public static string TotalNoRecords
        {
            get;
            set;
        }
//public int BillingChargeTran_ID {
//    get { return _BillingChargeTran_ID; }
//    set { _BillingChargeTran_ID = value; }
//}
//public string BillChargeTransaction_ID {
//    get { return _BillChargeTran_ID; }
//    set { _BillChargeTran_ID = value; }
//}


public string Status_Ind {
    get;
    set;
}

//public string ConcateString {
//    get { return _ConcateString; }
//    set { _ConcateString = value; }
//}

//public string ImagePathSuffix {
//    get { return _ImagePathSuffix; }
//    set { _ImagePathSuffix = value; }
//}

public int? Provider_Greeting_ID {
    get;
    set;
}

//public string EncryptedImagePath {
//    get { return _EncryptedImagePath; }
//    set { _EncryptedImagePath = value; }
//}

//public int Provider_Directory_ID {
//    get { return _Provider_Directory_ID; }
//    set { _Provider_Directory_ID = value; }
//}

//public int Directory_ID {
//    get { return _Directory_ID; }
//    set { _Directory_ID = value; }
//}

public int? Provider_ID {
    get;
    set;
}

public int? ProviderLogin_ID {
    get;
    set;
}

public int? Practice_ID {
    get;
    set;
}

//public string FilePath {
//    get { return _FilePath; }
//    set { _FilePath = value; }
//}

//public string ImagePath {
//    get { return _ImagePath; }
//    set { _ImagePath = value; }
//}

//public string EncryptedFilepath {
//    get { return _EncryptedFilepath; }
//    set { _EncryptedFilepath = value; }
//}

//public string IsAudio {
//    get { return _IsAudio; }
//    set { _IsAudio = value; }
//}

//public string IsVideo {
//    get { return _IsVideo; }
//    set { _IsVideo = value; }
//}

//public string ListingI {
//    get { return _ListingI; }
//    set { _ListingI = value; }
//}

//public string LisitngII {
//    get { return _LisitngII; }
//    set { _LisitngII = value; }
//}

//public string ListingIII {
//    get { return _ListingIII; }
//    set { _ListingIII = value; }
//}

//public int Provider_ChildWebsite_ID {
//    get { return _Provider_ChildWebsite_ID; }
//    set { _Provider_ChildWebsite_ID = value; }
//}

//public int Childwebsite_ID {
//    get { return _Childwebsite_ID; }
//    set { _Childwebsite_ID = value; }
//}

//public string WebsiteTitle {
//    get { return _WebsiteTitle; }
//    set { _WebsiteTitle = value; }
//}

//public int No_of_Sites {
//    get { return _No_of_Sites; }
//    set { _No_of_Sites = value; }
//}

//public FeaturesList(string Billingservice_ID)
//{
//    _Billingservice_ID = Billingservice_ID;

//}


public FeaturesList()
{
}

//public string IsActive {
//    get;
//    set;
//}

//public static int NoOfSites {
//    get { return _NoOfSites; }
//    set { _NoOfSites = value; }
//}

public string Title {
    get;
    set;
}

public string Description {
    get;
    set;
}

//public string ListingFeature_ID {
//    get;
//    set;
//}

//public string Billingservice_ID {
//    get { return _Billingservice_ID; }
//    set { _Billingservice_ID = value; }
//}
//public int BSChargeTemplate_ID {
//    get { return _BSChargeTemplate_ID; }
//    set { _BSChargeTemplate_ID = value; }
//}
public int? ViewedCount {
    get;
    set;
}
//public double? Document_Size {
//    get { return _Document_Size; }
//    set { _Document_Size = value; }
//}
public string Embed_Text {
    get;
    set;
}

public List<FeaturesList> GetProviderBasedGreetings(int? ProviderID,  int NoOfRecords, int Pageno)
{
    return ObjSql.GetProviderBasedGreetings(ProviderID,  NoOfRecords, Pageno);
}
public bool Insert_Provider_Greetings(FeaturesList objData)
{
    return ObjSql.Insert_Provider_Greetings(objData);
}

public void ApplyStatus(int ProviderGreetingID, int ProviderID, string Status)
{
     ObjSql.ApplyStatus(ProviderGreetingID, ProviderID, Status);
}
    }
}