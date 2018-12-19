using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MowerHelper.Models.DAL.DataAccessLayer;
using MowerHelper.Models.DAL;
namespace MowerHelper.Models.BLL.Providers
{
    public class Provider_ProfilesInfo
    {
     
        public Provider_ProfilesInfo()
{
}

        public Provider_ProfilesInfo(string Yearinpractice, string paragraph,string URL, string Payments)
        {
           this.Yearinpractice = Yearinpractice;
           this.paragraph = paragraph;
           this.URL = URL;
           this.Payments = Payments;

     


        }
public Provider_ProfilesInfo(int Provider_ID, string Yearinpractice, string Degree1, string School1, string Degree2, string School2, string Degree3, string School3,int UpdatedBy)
{
    this.Provider_ID = Provider_ID;
    this.Yearinpractice = Yearinpractice;
    this.Degree1 = Degree1;
    this.School1 = School1;
    this.Degree2 = Degree2;
    this.School2 = School2;
    this.Degree3 = Degree3;
    this.School3 = School3;
    this.UpdatedBy = UpdatedBy;
}
public Provider_ProfilesInfo(int Provider_ID, int UpdatedBy, string LicenseNo, string IsLicenseVerified, string licenseexpirydate)
{
    this.Provider_ID = Provider_ID;
    this.UpdatedBy = UpdatedBy;
    this.LicenseNo = LicenseNo;
    this.IsLicenseVerified = IsLicenseVerified;
    this.licenseexpirydate = licenseexpirydate;
}
public string licenseexpirydate
{
    get;
    set;
}

public string freesession { get; set; }

public string Is_Contact_Info { get; set; }
public string FeeRange_From
{
    get;
    set;
}

public string FeeRange_to
{
    get;
    set;
}

public int Provider_ID
{
    get;
    set;
}

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

public string paragraph
{
    get;
    set;
}

public string OtherServices
{
    get;
    set;
}

public string URL
{
    get;
    set;
}

public string Payments
{
    get;
    set;
}

public int? UpdatedBy
{
    get;
    set;
}

public string LicenseNo
{
    get;
    set;
}
public string IsLicenseVerified
{
    get;
    set;
}


public string DisplayFee_ind
{
    get;
    set;
}
public string showschedule { get; set; }
//public string fbcomments
//{
//    get;
//    set;
//}
public string Random_number { get; set; }
public string State { get; set; }
public string City { get; set; }
public string Zip { get; set; }
public string facebookurl { get; set; }

public string twitterurl { get; set; }
public string fbcomments
{
    get;
    set;
}
public string Website { get; set; }
public static Provider_ProfilesInfo Get_PrividerProfilesInfo(int Provider_ID)
{
    SQLDataAccessLayer DALLayer = new SQLDataAccessLayer();
    return DALLayer.Get_PrividerProfilesInfo(Provider_ID);
}
public static Provider_ProfilesInfo Get_Prividerlicensedetails(int Provider_ID)
{
    SQLDataAccessLayer DALLayer = new SQLDataAccessLayer();
    return DALLayer.Get_Prividerlicensedetails(Provider_ID);
}

public static bool Update_licensedetails(Provider_ProfilesInfo ObjProfileInfoProv)
{
    SQLDataAccessLayer DALLayer = new SQLDataAccessLayer();
    return DALLayer.Update_licensedetails(ObjProfileInfoProv);
}
    }
}