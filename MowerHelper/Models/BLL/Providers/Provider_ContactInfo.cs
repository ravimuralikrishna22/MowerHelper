using System;
using MowerHelper.Models.BLL.Common;
using MowerHelper.Models.DAL;
namespace MowerHelper.Models.BLL.Providers
{
    public class Provider_ContactInfo:Addresses
    {
        private string _Provider_ID;

        public Provider_ContactInfo()
        {
        }

public Provider_ContactInfo(string in_Address1, string in_Address2, string in_Zip, int? in_State_ID, string in_StateName, int? in_City_ID, string in_CityName,
string in_CellPhone,  string in_WorkPhone,  string in_Fax,  string in_Email,   string in_Website, string facebookurl,
string twitterurl)
{
	Address1 = in_Address1;
	Address2 = in_Address2;
	Zip = in_Zip;
	State_ID = in_State_ID;
	StateName = in_StateName;
	City_ID = in_City_ID;
	CityName = in_CityName;
	CellPhone = in_CellPhone;
	WorkPhone = in_WorkPhone;
	Fax = in_Fax;
	Email = in_Email;
	Website = in_Website;
	this.twitterurl = twitterurl;
	this.facebookurl = facebookurl;
}
public Provider_ContactInfo(string Provider_ID)
{
	_Provider_ID = Provider_ID;
}

public string Firstname { get; set; }

public string MiddleInitial { get; set; }


public string LastName { get; set; }

public string PracticeName { get; set; }

        public string LicenseNo { get; set; }

        public string IsLicenseVerified { get; set; }

        public string licenseexpirydate { get; set; }

        public string ProviderImage { get; set; }

        public string ProviderImagePath { get; set; }

        public int Provider_ID{get; set; }

        public string facebookurl { get; set; }

        public string twitterurl { get; set; }
        public string fbcomments
        {
            get;
            set;
        }
        public string Random_number
        {
            get;
            set;
        }
        public string ElectricianImage { get; set; }
        public string DriverImage { get; set; }
        public string DriverCustomiseImage { get; set; }
        public string ElctricianCustmiseImage { get; set; }
        public string Referralcode { get; set; }
        public string Refferal_Indicator { get; set; }
        public static Provider_ContactInfo Get_ContactInfo(int Reference_ID, int ReferenceType_ID)
{
    SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
    return DBLayer.Get_ContactInfo(Reference_ID, ReferenceType_ID);
}
    }
}