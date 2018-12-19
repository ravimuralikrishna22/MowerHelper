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
    public class SimpleProvider_DetailsInfo
    {

        public SimpleProvider_DetailsInfo()
{
}


        public SimpleProvider_DetailsInfo(int Provider_ID, string Medicare, string Medicaid,
        string SSN, string TaxID, string NationalID, string GroupId,  string SignatureImage = null)
        {
           this.Provider_ID = Provider_ID;
           this.Medicare = Medicare;
           this.Medicaid = Medicaid;
           this.SSN = SSN;
           this.TaxID = TaxID;
           this.NationalID = NationalID;
           this.GroupId = GroupId;
           this.SignatureImage = SignatureImage;
        }

public string PracticeName { get; set; }
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

public string certification
{
    get;
    set;
}


public string licenseexpirydate
{
    get;
    set;
}
public string ProviderImage
{
    get;
    set;
}
public int? Provider_ID
{
    get;
    set;
}


public string Practice_ID
{
    get;
    set;
}

public string Title
{
    get;
    set;
}

public string FirstName
{
    get;
    set;
}

public string MI
{
    get;
    set;
}

public string LastName
{
    get;
    set;
}

public string Suffix
{
    get;
    set;
}



public string GENDER
{
    get;
    set;
}


public int UpdatedBy
{
    get;
    set;
}

public string Email
{
    get;
    set;
}

public string Address1
{
    get;
    set;
}

public string Address2
{
    get;
    set;
}

public int? City_ID
{
    get;
    set;
}

public int? State_ID
{
    get;
    set;
}

public int Country_ID
{
    get;
    set;
}

public string Zip
{
    get;
    set;
}

public string CellPhone
{
    get;
    set;
}

public string CellPhoneLeaveMsg
{
    get;
    set;
}

public string WorkPhone
{
    get;
    set;
}

public string WorkPhoneExtn
{
    get;
    set;
}

public string WorkPhoneLeaveMsg
{
    get;
    set;
}

public string FAX
{
    get;
    set;
}

public string Website
{
    get;
    set;
}
public string facebookurl
{
    get;
    set;
}
public string twitterurl
{
    get;
    set;
}

public string Out_Msg
{
    get;
    set;
}

public string Medicare
{
      get;
    set;
}
public string SSN
{
    get;
    set;
}
public string TaxID
{
    get;
    set;
}
public string NationalID
{
    get;
    set;
}
public string IsSign
{
    get;
    set;
}
public string Medicaid
{
    get;
    set;
}
public string GroupId
{
    get;
    set;
}

public string SignatureImage
{
    get;
    set;
}
public string Address3
{
    get;
    set;
}

public string Address4
{
    get;
    set;
}

public int? City_ID1
{
    get;
    set;
}

public int? State_ID1
{
    get;
    set;
}

public string Zip1
{
    get;
    set;
}
public string ind
{
    get;
    set;
}
public string MPtotext
{
    get;
    set;
}
public string Fbcomments
{
    get;
    set;
}
public string DriverLicsenseImage { get; set; }
public string ElctricianLicenseImage { get; set; }
public string DriverCustomiseImage { get; set; }
public string ElctricianCustmiseImage { get; set; }
public static string Upd_SimpleProviderDetailsinDirectory(SimpleProvider_DetailsInfo objUpd, ref string Out_Msg)
{
    SQLDataAccessLayer DALLayer = new SQLDataAccessLayer();
    return DALLayer.Upd_SimpleProviderDetailsinDirectory(objUpd, ref Out_Msg);
}




    }
}