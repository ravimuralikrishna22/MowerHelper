using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL.DataAccessLayer;
using MowerHelper.Models.DAL;
namespace MowerHelper.Models.BLL.Providers
{
    public class Provider_TimaTable
    {

public Provider_TimaTable()
{
}
public Provider_TimaTable(int PlaceOfService_ID, string FacilityName)
{
	this.PlaceOfService_ID = PlaceOfService_ID;
    this.FacilityName = FacilityName;
}

public int Provider_ID
{
    get;
    set;
}
public int Practice_ID {
    get;
    set;
}
public int PlaceOfService_ID {
    get;
    set;
}
public string FacilityName {
    get;
    set;
}
//public static List<Provider_TimaTable> GetFacilitiesName(int? Provider_ID, int? Practice_ID)
//{
//    SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
//    return DBLayer.GetFacilitiesName(Provider_ID, Practice_ID);
//}
    }
}