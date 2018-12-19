using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL;
using MowerHelper.Models.DAL.DataAccessLayer;
using System.Data;
namespace MowerHelper.Models.BLL.Providers
{
    public class Provider_DocumentInfo
    {
        public Provider_DocumentInfo()
{
}
public Provider_DocumentInfo(int? ProviderDocument_ID, string Title, string DocDescription, string Path, string FileName, string Path_Suffix, int? NoOfRecords)
{
    this.ProviderDocument_ID = ProviderDocument_ID;
    this.Title = Title;
    this.DocDescription = DocDescription;
    this.Path = Path;
    this.FileName = FileName;
    this.Path_Suffix = Path_Suffix;
    this.NoOfRecords = NoOfRecords;
}
public Provider_DocumentInfo(int Slno, int ProviderDocument_ID, int? DocumentType_ID, string Path,  string Title,  string DocDescription, string CreatedOn, string ImagePathSuffix,
string Filename, int ViewedCount, string Displayinpublic)
{
    this.ProviderDocument_ID = ProviderDocument_ID;
    this.Path = Path;
    this.Title = Title;
    this.DocDescription = DocDescription;
    this.CreatedOn = CreatedOn;
    this.ImagePathSuffix = ImagePathSuffix;
    this.FileName = Filename;
    this.ViewedCount = ViewedCount;
    this.Displayinpublic = Displayinpublic;
}
public string verifieddocument
{
    get;
    set;
}
public string Category
{
    get;
    set;
}
public string ProvName
{
    get;
    set;
}
public int? Patient_ID
{
    get;
    set;
}
public string Patient_Name
{
    get;
    set;
}

public double Document_Size
{
    get;
    set;
}
public int R_No
{
    get;
    set;
}
public int CreatedBy
{
    get;
    set;
}
public int Slno
{
    get;
    set;
}
public static int TotalRecords
{
    get;
    set;
}

public int? Practice_ID
{
    get;
    set;
}
public int? Provider_ID
{
    get;
    set;
}
public int? ProviderDocument_ID
{
    get;
    set;
}
public int PageNo
{
    get;
    set;
}
public int? NoOfRecords
{
    get;
    set;
}

public string OrderBYItem
{
    get;
    set;
}

public string OrderBy
{
    get;
    set;
}
public string Path
{
    get;
    set;
}
public string Title
{
    get;
    set;
}
public string DocDescription
{
    get;
    set;
}
public string CreatedOn
{
    get;
    set;
}
public string ProviderName
{
    get;
    set;
}
public int Counts
{
    get;
    set;
}
public string Size_KB
{
    get;
    set;
}
public string ImagePathSuffix
{
    get;
    set;
}
public string FileName
{
    get;
    set;
}
public int ViewedCount
{
    get;
    set;
}
public string Path_Suffix
{
    get;
    set;
}
public string Displayinpublic
{
    get;
    set;
}
public static List<Provider_DocumentInfo> Get_DocumentInfo(Provider_DocumentInfo ObjDocument)
{
    SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
    return DBLayer.Get_DocumentInfo(ObjDocument);
}

public static bool Del_DocumentInfo(int ProviderDocument_ID, int DeletedBy)
{
    SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
    return DBLayer.Del_DocumentInfo(ProviderDocument_ID, DeletedBy);
}
public static string Insert_Attachment( int? Provider_ID, string Title, string DocDescription, string Path, string PathEncrypted, string FileName, string IsDisplayPublic, int CreatedBy,
string verifieddocument, ref string Out_Suffix, double? Document_Size = null)
{
    SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
    return DBLayer.Insert_Attachment( Provider_ID, Title,  DocDescription, Path, PathEncrypted, FileName, IsDisplayPublic, CreatedBy,
    verifieddocument, ref Out_Suffix, Document_Size);
}
public static bool Update_Attachment(int? ProviderDocument_ID,  string Title, string DocDescription, string Path, string PathEncrypted, string FileName, int UpdatedBy, string verifieddocument, double? Document_Size = null,
string Displayinpublic = null)
{
    SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
    return DBLayer.Update_Attachment(ProviderDocument_ID,  Title, DocDescription, Path, PathEncrypted, FileName, UpdatedBy, verifieddocument, Document_Size,
    Displayinpublic);
}
public static Provider_DocumentInfo GetAttachmentList( int? Provider_ID, int? ProviderDocument_ID)
{
    SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
    return DBLayer.GetAttachmentList( Provider_ID, ProviderDocument_ID);
}
public static DataSet Provider_DocumentuploadDetails(Provider_DocumentInfo ObjDocument)
{
    SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
    return DBLayer.Provider_DocumentuploadDetails(ObjDocument);
}

    }
}