using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.Admin
{
    public class MediaInfo
    {
        
#region "Properties"

public string IsDisplayToPractice {
	get;
	set;
}
public string AdminLevel_Ind {
get;
	set;
}
public string DispalyToProvider_Ind {
get;
	set;
}

public int PracticeHelp_Media_ID {

get;
	set;
}

public int Practice_ID {

	get;
	set;
}
public static DataTable dtTable {
get;
	set;
}

public static int TotalRecords {
get;
	set;
}
public int SINo {
get;
	set;
}

public string Helpmedia_ID {

get;
	set;
}

public int? Reference_ID {
get;
	set;
}

public int? ReferenceType_ID {
get;
	set;
}

public string Title {
get;
	set;
}
public string Description {
get;
	set;
}

public string Duration {
get;
	set;
}


public string MediaFee {
get;
	set;
}

public string FileType {
get;
	set;
}
public string FilePath {
get;
	set;
}

public string FilePath_Encrypted {
get;
	set;
}

public string ImagepathSuffix {
get;
	set;
}


public string IsPracticeMedia_Ind {
get;
	set;
}

public string Status_Ind {

get;
	set;
}

public string createdBy {
get;
	set;
}

public string CreadtedOn {
get;
	set;
}


public string UpDatedBy {

get;
	set;
}

public string UpDatedOn {
get;
	set;
}
public int NoOfrecords {
get;
	set;
}

public int PageNO {
get;
	set;
}

public string MediaTitle {
get;
	set;
}

public string OrderBy {
get;
	set;
}
public string OrderByItem {
get;
	set;
}
public string strexplictids {
get;
	set;
}
#endregion
public string Loginhistory_ID {
get;
	set;
}
#region "Constructurs"



public MediaInfo()
{
}


public MediaInfo(string _Helpmedia_ID)
{
	Helpmedia_ID = _Helpmedia_ID;

}


public MediaInfo(string _MediaTitle, string _OrderBy, string _OrderByItem, int _NoOfrecords, int _PageNO)
{
	MediaTitle = _MediaTitle;
	OrderBy = _OrderBy;
	OrderByItem = _OrderByItem;
	NoOfrecords = _NoOfrecords;
	PageNO = _PageNO;

}


public MediaInfo(int _Reference_ID, int _ReferenceType_ID, string _Title, string _Description, string _Duration, string _MediaFee, string _FileType, string _FilePath, string _FilePath_Encrypted, string _ImagepathSuffix)
{
	Reference_ID = _Reference_ID;
	ReferenceType_ID = _ReferenceType_ID;
	Title = _Title;
	Description = _Description;
	Duration = _Duration;
	MediaFee = _MediaFee;
	FileType = _FileType;
	FilePath = _FilePath;
	FilePath_Encrypted = _FilePath_Encrypted;
	ImagepathSuffix = _ImagepathSuffix;


}

public MediaInfo(int _Practice_ID, string _DispalyToProvider_Ind, string _Title, string _Description, string _Duration, string _AdminLevel_Ind, string _FileType, string _FilePath, string _ImagepathSuffix)
{
	Practice_ID = Practice_ID;
	DispalyToProvider_Ind = DispalyToProvider_Ind;
	Title = Title;
	Description = Description;
	Duration = Duration;
	_AdminLevel_Ind = AdminLevel_Ind;
	_FileType = FileType;
	_FilePath = FilePath;

	_ImagepathSuffix = ImagepathSuffix;


}

public MediaInfo(string _Title, string _Description, string _Duration, string _FileType, string _FilePath, string _FilePath_Encrypted, string _ImagepathSuffix, string _Loginhistory_ID)
{


	Title = _Title;
	Description = _Description;
	Duration = _Duration;
	FileType = _FileType;
	FilePath = _FilePath;
	FilePath_Encrypted = _FilePath_Encrypted;
	ImagepathSuffix = _ImagepathSuffix;
	Loginhistory_ID = _Loginhistory_ID;


}

#endregion

public static List<MediaInfo> GetMediaInfo(MediaInfo objMediaInfo)
{
    SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
    return DBlayer.GetMediaInfo(objMediaInfo);
}
public static List<MediaInfo> GetVideoInfo(MediaInfo objMediaInfo)
{
    SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
    return DBlayer.GetVideoInfo(objMediaInfo);
}
public static string InsertMediaInfo(MediaInfo ObjInsMediaInfo, ref string out_Msg, ref string Loc_ImagePathSuffix)
{
    SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
    return DBlayer.InsertMediaInfo(ObjInsMediaInfo,ref out_Msg,ref Loc_ImagePathSuffix);

}
public static List<MediaInfo> GetNewMediaInfo(MediaInfo ObjNewMediaInfo)
{
    SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
    return DBlayer.GetNewMediaInfo(ObjNewMediaInfo);

}
public static string UpdateMediaInfo(MediaInfo ObjUpdMediaInfo, ref string out_Msg)
{
    SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
    return DBlayer.UpdateMediaInfo(ObjUpdMediaInfo,ref out_Msg);

}
public static bool DeleteMediaInfo(MediaInfo ObjDelMediaInfo)
{
    SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
    return DBlayer.DeleteMediaInfo(ObjDelMediaInfo);

}
public static List<MediaInfo> GetMediaInfo1(MediaInfo objMediaInfo)
{
    SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
    return DBlayer.GetMediaInfo1(objMediaInfo);
}
    }
}