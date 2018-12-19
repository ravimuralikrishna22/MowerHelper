using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MowerHelper.Models.Classes;
namespace MowerHelper.Models.BLL.BLLSchedule
{

	#region "ListofPatients:Public class get the list of patients based on provider selection"

    public class ListofPatients
	{

    //    #region "Private varibles(UIL)"
    //    private string _FirstName;
    //    private string _LastName;
    //    static int _TotalRecords;
    //    private string _HomePhone;
    //    private string _City;
    //    private string _State;
    //    private string _Zip;
    //    private string _PatientType;
    //    private string _OrderBy;
    //    private string _OrderByitem;
    //    private int _Provider_ID;
    //    private string _PlaceOfService_ID;
    //    private string _Practice_ID;
    //    private int _PageNo;
    //        #endregion
    //    private int _NoofRecords;

    //    #region "Private varibles(DAL)"
    //    private int _SlNo;
    //    private int _Patient_ID;
    //    private int _PatientLogin_ID;
    //    private string _PatientName;
    //    private string _Practicename;
    //    private string _Providername;
    //    private string _Address1;
    //    private string _Phone;
    //   // private string _dob;
    //    private string _PatientEmail;
    ////	private int _CPTCode;
    //    private string _Address;
    //        #endregion
		//private string _Nickname;

        //#region "Construtor:This construtor is intialized by the UIL"

        //public ListofPatients()
        //{
        //}
        //public ListofPatients(string FirstName, string LastName, string HomePhone, string City, string State, string Zip, string PatientType, string OrderBy, string OrderByitem, int Provider_ID,
        //string PlaceOfService_ID, string Practice_ID, int PageNo, int NoofRecords)
        //{
        //    _FirstName = FirstName;
        //    _LastName = LastName;
        //    _HomePhone = HomePhone;
        //    _City = City;
        //    _State = State;
        //    _Zip = Zip;
        //    _PatientType = PatientType;
        //    _OrderBy = OrderBy;
        //    _OrderByitem = OrderByitem;
        //    _Provider_ID = Provider_ID;
        //    _PlaceOfService_ID = PlaceOfService_ID;
        //    _Practice_ID = Practice_ID;
        //    _PageNo = PageNo;
        //    _NoofRecords = NoofRecords;
        //}
        //#endregion

        //#region "Construtor:This construtor is intialized by the DAL"
        //public ListofPatients(int SlNo, int Patient_ID, int PatientLogin_ID, string Practice_ID, string PatientName, string Practicename, string Providername, string Address, string Phone, string PatientEmail,
        //int CPTCode)
        //{
        //    _SlNo = SlNo;
        //    _Patient_ID = Patient_ID;
        //    _PatientLogin_ID = PatientLogin_ID;
        //    _Practice_ID = Practice_ID;
        //    _PatientName = PatientName;
        //    _Practicename = Practicename;
        //    _Providername = Providername;
        //    _Address = Address;
        //    _Phone = Phone;
        //    _PatientEmail = PatientEmail;
        //    //_CPTCode = CPTCode;
        //}
        //#endregion

		#region "Properties:UIL variables"
        //public string Nickname {
        //    get { return _Nickname; }
        //    set { _Nickname = value; }
        //}
        public string FirstName
        {
            get;
            set;
        }
		public string LastName {
            get;
            set;
		}
		public string HomePhone {
            get;
            set;
		}
		public string City {
            get;
            set;
		}
		public string State {
            get;
            set;
		}
		public string Zip {
            get;
            set;
		}
		public string PatientType {
            get;
            set;
		}
		public string OrderBy {
            get;
            set;
		}
		public string OrderByitem {
            get;
            set;
		}
		public int Provider_ID {
            get;
            set;
		}
		public static int TotalRecords {
            get;
            set;
		}
		public string PlaceOfService_ID {
            get;
            set;
		}
		public string Practice_ID {
            get;
            set;
		}
		public int PageNo {
            get;
            set;
		}
		public int NoofRecords {
            get;
            set;
		}
		#endregion

		#region "Properties:DAL variables"
		public string Address {
            get;
            set;
		}
		public int SlNo {
            get;
            set;
		}

		public int Patient_ID {
            get;
            set;
		}

		public int PatientLogin_ID {
            get;
            set;
		}

		public string PatientName {
            get;
            set;
		}
		public string Practicename {
            get;
            set;
		}
		public string Providername {
            get;
            set;
		}

		public string Address1 {
            get;
            set;
		}

		public string Phone {
            get;
            set;
		}

		public string PatientEmail {
            get;
            set;
		}
        //public int CPTCode {
        //    get { return _CPTCode; }
        //    set { _CPTCode = value; }
        //}
        //public string Dob
        //{
        //    get { return _dob; }
        //    set { _dob = value; }
        //}
        public string Balance { get; set; }

		#endregion

		#region "ListPatients:Get the list of patients"
        //public static List<ListofPatients> ListPatients(ListofPatients objListofPatients)
        //{
        //    return SCHDataAccessLayerBaseClassHelper.GetDataAccessLayer.ListPatients(objListofPatients);
        //}
		#endregion
        public List<ListofPatients> getPatientList(int ProviderID)
        {
            List<ListofPatients> patientList = new List<ListofPatients>();
            clsCommonFunctions objcommon = new clsCommonFunctions();
            IDataParameter[] objparam = {
		new SqlParameter("@in_provider_id", (ProviderID != 0 ? ProviderID.ToString() : null))
	};
            objcommon.AddInParameters(objparam);
            SqlDataReader dr = default(SqlDataReader);
            dr = objcommon.GetDataReader("Help_dbo.St_practice_Typeahead_Patientname");
           
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ListofPatients obj = new ListofPatients();
                    if (dr["Patient_ID"] != null)
                    {
                        obj.Patient_ID = Convert.ToInt32(dr["Patient_ID"]);
                    }
                    if (dr["Patientname"] != null)
                    {
                        obj.PatientName = dr["Patientname"].ToString();
                    }
                    if (dr["PatientLogin_ID"].ToString() !="")
                    {
                        obj.PatientLogin_ID = Convert.ToInt32(dr["PatientLogin_ID"]);
                    }
                    //if(dr["DOB"] !=null){
                    //    obj.Dob=dr["DOB"].ToString();
                    //    }
                    if (dr["Phonenumber"] != null)
                    {
                        obj.Phone = dr["Phonenumber"].ToString();
                    }
                    patientList.Add(obj);
                }
            }
            return patientList;
        }
        public List<ListofPatients> getPatientDetails(int ProviderID)
        {
            List<ListofPatients> patientList = new List<ListofPatients>();
            clsCommonFunctions objcommon = new clsCommonFunctions();
            IDataParameter[] objparam = {
		new SqlParameter("@in_provider_id", (ProviderID != 0 ? ProviderID.ToString() : null))
	};
            objcommon.AddInParameters(objparam);
            SqlDataReader dr = default(SqlDataReader);
            dr = objcommon.GetDataReader("Help_dbo.St_Typeahead_Patientdetails");

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ListofPatients obj = new ListofPatients();
                    if (dr["Patient_ID"] != null)
                    {
                        obj.Patient_ID = Convert.ToInt32(dr["Patient_ID"]);
                    }
                    if (dr["Patientname"] != null)
                    {
                        obj.PatientName = dr["Patientname"].ToString();
                    }
                    if (dr["PatientLogin_ID"].ToString() != "")
                    {
                        obj.PatientLogin_ID = Convert.ToInt32(dr["PatientLogin_ID"]);
                    }
                    //if(dr["DOB"] !=null){
                    //    obj.Dob=dr["DOB"].ToString();
                    //    }
                    if (dr["Phonenumber"] != null)
                    {
                        obj.Phone = dr["Phonenumber"].ToString();
                    }
                    if (dr["Balance"] != null)
                    {
                        //obj.Balance = dr["Balance"].ToString();
                        obj.Balance =string.Format("{0:c}", dr["Balance"]).Replace("$","");//	String.Format("{0:0.00}", (decimal)(dr["Balance"]));
                    }
                    patientList.Add(obj);
                }
            }
            return patientList;
        }
	}
	#endregion

	#region "GetPatientDetails:Public class get the patient details alog with CPT info"
	public class GetPatientDetails
	{

		#region "Private Varirables"
		private int _Patient_ID;
		private int _PatientLogin_ID;
		private string _PatientName;
	//	private Nullable<int> _CPTCode;
		private int _Duration;
		private int _PracticeID;
		private string _Auto_Reminder_Allow_Ind;
		private string _PatientEmail;
			#endregion
		private string _cellphone;
        private string _DefaultCourtId;

		#region "Construtor"
		public GetPatientDetails(int Patient_ID, int PatientLogin_ID, string PatientName,  int Duration,  string PatientEmail, string DefaultCourtId)
		{
			_Patient_ID = Patient_ID;
			_PatientLogin_ID = PatientLogin_ID;
			_PatientName = PatientName;
				_Duration = Duration;
			_PatientEmail = PatientEmail;
            _DefaultCourtId = DefaultCourtId;
			
		}

		#endregion

		#region "Properties"
		public string cellphone {
			get { return _cellphone; }
			set { _cellphone = value; }
		}
		public string PatientEmail {
			get { return _PatientEmail; }
			set { _PatientEmail = value; }
		}
		public string Auto_Reminder_Allow_Ind {
			get { return _Auto_Reminder_Allow_Ind; }
			set { _Auto_Reminder_Allow_Ind = value; }
		}
		public int Patient_ID {
			get { return _Patient_ID; }
			set { _Patient_ID = value; }
		}

		public int PatientLogin_ID {
			get { return _PatientLogin_ID; }
			set { _PatientLogin_ID = value; }
		}

		public string PatientName {
			get { return _PatientName; }
			set { _PatientName = value; }
		}

        //public Nullable<int> CPTCode {
        //    get { return _CPTCode; }
        //    set { _CPTCode = value; }
        //}

		public int Duration {
			get { return _Duration; }
			set { _Duration = value; }
		}

		public int PracticeID {
			get { return _PracticeID; }
			set { _PracticeID = value; }
		}
        public string Bussinessname { get; set; }
        public string Provider_cellphoneno { get; set; }
        public string DefaultCourtId {
            get { return _DefaultCourtId; }
            set { _DefaultCourtId = value; }
        }
		#endregion

		#region "GetPatientDetails:Public function gets the patient details"

        //public static List<GetPatientDetails> GetPatientDetailsinfo(string PatientIDs, string PracticeID = null, string Provider_ID = null)
        //{
        //    return SCHDataAccessLayerBaseClassHelper.GetDataAccessLayer().GetPatientDetailsinfo(PatientIDs, PracticeID, Provider_ID);
        //}

		#endregion
        

	}
	#endregion


}
