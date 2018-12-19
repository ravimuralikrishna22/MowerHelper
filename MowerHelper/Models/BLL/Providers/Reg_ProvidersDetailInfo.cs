using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL.DataAccessLayer;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.Providers
{
    public class Reg_ProvidersDetailInfo
    {

        public string UserName
        {
            get;
            set;
        }
        public string Password
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
        public string Email
        {
            get;
            set;
        }
        public string Email2
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
        public int City_ID
        {
            get;
            set;
        }
        public int State_ID
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
        public string WorkPhone
        {
            get;
            set;
        }
        public string PrimaryAddress_Ind
        {
            get;
            set;
        }
        public string Businessname
        {
            get;
            set;
        }
        public string IsSimplePractice
        {
            get;
            set;
        }
        public string Licensenumber
        {
            get;
            set;
        }
        public string Statename
        {
            get;
            set;
        }
        public string Cityname
        {
            get;
            set;
        }
        public string CellPhone
        {
            get;
            set;
        }
        public string Promocode_ID
        {
            get;
            set;
        }
        public string IssuingBank
        {
            get;
            set;
        }
        public Reg_ProvidersDetailInfo()
        {
        }
        public Reg_ProvidersDetailInfo(string _UserName, string _Password, string _FirstName, string _MI, string _LastName, string _Email, string _Address1, string _Address2,
                                        int _City_ID, int _State_ID, int _Country_ID, string _Zip, string _WorkPhone, string _PrimaryAddress_Ind,
                                        string _Businessname, string _IsSimplePractice, string _Licensenumber, string _CellPhone)
        {
            UserName = _UserName;
            Password = _Password;

            FirstName = _FirstName;
            MI = _MI;
            LastName = _LastName;

            Email = _Email;

            Address1 = _Address1;
            Address2 = _Address2;
            City_ID = _City_ID;
            State_ID = _State_ID;
            Country_ID = _Country_ID;
            Zip = _Zip;
            WorkPhone = _WorkPhone;
            PrimaryAddress_Ind = _PrimaryAddress_Ind;
            Businessname = _Businessname;
            IsSimplePractice = _IsSimplePractice;
            Licensenumber = _Licensenumber;
            CellPhone = _CellPhone;
        }
        public static object Insert_NewRegProvidersDetails(Reg_ProvidersDetailInfo ObjInsertDetails, ref string Out_Msg, ref string Out_Provider_ID, ref int OutPractice_ID, ref int OutLogin_ID)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Insert_NewRegProvidersDetails(ObjInsertDetails, ref Out_Msg, ref Out_Provider_ID, ref OutPractice_ID, ref OutLogin_ID);
        }
        public static Reg_ProvidersDetailInfo Get_ProviderAddressDetails(int prov_id)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.Get_ProviderAddressDetails(prov_id);
        }
        public static object check_validRefferalCode(string CRM_Promocode, ref string Out_Msg)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            return layer.check_validRefferalCode(CRM_Promocode, ref Out_Msg);
        }
        public static void INS_providerRefferalCode(string Promocode, int provider_id)
        {
            SQLDataAccessLayer layer = new SQLDataAccessLayer();
            layer.INS_providerRefferalCode(Promocode, provider_id);
        }
    }
}