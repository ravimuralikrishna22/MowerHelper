using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL.DataAccessLayer;
using MowerHelper.Models.DAL;
namespace MowerHelper.Models.BLL.Providers
{
    public class Reg_ProviderConfirmation
    {
        private int? _Provider_ID;
        private int? _CreatedBy;
        private int? _Practice_ID;
        private int? _ProviderLogin_ID;
        private string _Email;
        private string _UserName;
        private string _Password;
        private string _Status_Ind;
        private int _UpdatedBy;
        private string _HIPAAContent;
        public int? Provider_ID
        {
            get { return _Provider_ID; }
            set { _Provider_ID = value; }
        }
        public int? CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        public int? Practice_ID
        {
            get { return _Practice_ID; }
            set { _Practice_ID = value; }
        }
        public int? ProviderLogin_ID
        {
            get { return _ProviderLogin_ID; }
            set { _ProviderLogin_ID = value; }
        }
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        public string Status_Ind
        {
            get { return _Status_Ind; }
            set { _Status_Ind = value; }
        }
        public int UpdatedBy
        {
            get { return _UpdatedBy; }
            set { _UpdatedBy = value; }
        }
        public string HIPAAContent
        {
            get { return _HIPAAContent; }
            set { _HIPAAContent = value; }
        }
        public string providername { get; set; }
        
public Reg_ProviderConfirmation()
{
}
public Reg_ProviderConfirmation(int Provider_ID)
{
	_Provider_ID = Provider_ID;
}

public Reg_ProviderConfirmation(int Provider_ID, int CreatedBy)
{
	_Provider_ID = Provider_ID;
	_CreatedBy = CreatedBy;
}
public Reg_ProviderConfirmation(int ProviderLogin_ID, int Practice_ID, string Email)
{
	_ProviderLogin_ID = ProviderLogin_ID;
	_Practice_ID = Practice_ID;
	_Email = Email;
}
public Reg_ProviderConfirmation(int ProviderLogin_ID, string UserName, string Password, string Email)
{
	_ProviderLogin_ID = ProviderLogin_ID;
	_UserName = UserName;
	_Password = Password;
	_Email = Email;
}

public static bool ProviderConfirmation(Reg_ProviderConfirmation ObjConfirmation)
{
    SQLDataAccessLayer DALLayer = new SQLDataAccessLayer();
    return DALLayer.ProviderConfirmation(ObjConfirmation);
}
public static bool Reg_Upd_Status(int? ProviderID, string Status_Ind, int? UpdatedBy, string PromocodeID, string vaultid, string customer_id = null)
{
    SQLDataAccessLayer DALLayer = new SQLDataAccessLayer();
    return DALLayer.Reg_Upd_Status(ProviderID, Status_Ind, UpdatedBy, PromocodeID, vaultid, customer_id);
}
public static void Ins_trialpackage_users(int Login_ID)
{
    SQLDataAccessLayer DALLayer = new SQLDataAccessLayer();
    DALLayer.Ins_trialpackage_users(Login_ID);
}
public static Reg_ProviderConfirmation Reg_GetUserNameAndPassword(int Provider_ID)
{
    SQLDataAccessLayer DALLayer = new SQLDataAccessLayer();
    return DALLayer.Reg_GetUserNameAndPassword(Provider_ID);
}
public static string Reg_Insert_EmailMessage(Reg_ProviderConfirmation ObjEmailIns)
{
    SQLDataAccessLayer DALLayer = new SQLDataAccessLayer();
    return DALLayer.Reg_Insert_EmailMessage(ObjEmailIns);
}
public static string PasswordManage_ins_EmailMessage(Reg_ProviderConfirmation ObjEmailIns)
{
    SQLDataAccessLayer DALLayer = new SQLDataAccessLayer();
    return DALLayer.PasswordManage_ins_EmailMessage(ObjEmailIns);
}
    }
}