using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL.DataAccessLayer;
using MowerHelper.Models.DAL;
using System.Data.SqlClient;
using System.Data;

namespace MowerHelper.Models.BLL.Patients
{
    public class Responseparty
    {
        public int? Practice_ID
        {
            get;
            set;
        }
        public int Provider_ID
        {
            get;
            set;
        }
        public string Prefix
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
                    public string Gender
        {
            get;
            set;
        }
                public string HomePhone
        {
            get;
            set;
        }
                public string WPhone
                {
                    get;
                    set;
                }
                public string MPhone
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
                public int City_ID
                {
                    get;
                    set;
                }
                public string City
                {
                    get;
                    set;
                }
                public string Country_ID
                {
                    get;
                    set;

                }
                public int Country
                {
                    get;
                    set;
                }
                public int State_ID
                {
                    get;
                    set;
                }
                public string Zip
                {
                    get;
                    set;
                }
                public string DrivingLicenceNo
                {
                    get;
                    set;
                }
                public string State
                {
                    get;
                    set;
                }
                public string Status_Ind
                {
                    get;
                    set;
                }
                public char IsPatient
                {
                    get;
                    set;
                }

                public string ImagePath
                {
                    get;
                    set;
                }
                public string MPToText
                {
                    get;
                    set;
                }
                public int? Patient_ID
                {
                    get;
                    set;
                }
                public int UserId
                {
                    get;
                    set;
                }
                public int Login_ID
                {
                    get;
                    set;
                }
                public int ProviderPatient_ID
                {
                    get;
                    set;
                }
                public int AuthorizedProvider_ID
                {
                    get;
                    set;
                }
                public int ActualProviderLogin_ID
                {
                    get;
                    set;
                }
                public string ProviderName
                {
                    get;
                    set;
                }
                public string PatientName
                {
                    get;
                    set;
                }
                public int PlaceOfService_ID
                {
                    get;
                    set;
                }
                public string PlaceOfService
                {
                    get;
                    set;
                }
                public int Patient_Status_ID
                {
                    get;
                    set;
                }
                public string Username
                {
                    get;
                    set;
                }
                public int PracticePatient_RespParty_ID
                {
                    get;
                    set;
                }
                public string ispatientfile
                {
                    get;
                    set;
                }
                public string TotalAddress
                {
                    get;
                    set;
                }
       

        public static Responseparty Get_ResponsibleParty_Info(int Patient_ID, int Practice_ID, int Provider_ID)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Get_ResponsibleParty_Info(Patient_ID, Practice_ID, Provider_ID);
        }
        public static void Patient_Archive_Reject_Activate(int? Patient_ID, int Patient_Status_ID, int UpdatedBy, int Provider_ID)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            DBLayer.Patient_Archive_Reject_Activate(Patient_ID, Patient_Status_ID, UpdatedBy, Provider_ID);
        }

    }
}