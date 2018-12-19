namespace MowerHelper.Models.BLL.Common
{
    public class Addresses
    {
      //  private int? _Address_ID;
      //  private string _Email;
      //  private string _Email2;
      //  private string _Address1;
      //  private string _Address2;
      //  private string _FullAddress;
      //  private string _CountryName;
      //  private string _StateName;
      //  private string _CityName;
      //  private int? _City_ID;
      //  private int? _State_ID;
      //  private int? _Country_ID;
      //  private string _ZIP;
      //  private string _HomePhone;
      //  private string _HPExtension;
      //  private string _WorkPhone;
      //  private string _WPExtension;
      //  private string _CellPhone;
      // // private string _MobilePhone;
      // // private char _PLeaveMsg_Ind;
      //  private string _Fax;
      //  private string _Website;
      // // private char _PrimaryAddress_Ind;
      ////  private char _Status_Ind;
      //  private int? _City_ID1;
      //  private int? _State_ID1;
      //  private string _ZIP1;
      //  private string _Address3;
      //  private string _Address4;
      //  private string _ind;
        public string Fullname { get; set; }

        public int? Address_ID
        {
            get;
            set;
        }

        public int AddressType_ID { get; set; }

        public int ReferenceType_ID { get; set; }

        public int Reference_ID { get; set; }

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
        public string FullAddress
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
        public int? Country_ID
        {
            get;
            set;
        }
        public string Zip
        {
            get;
            set;
        }
        public string HomePhone
        {
            get;
            set;
        }
        //public string HPExtension
        //{
        //    get;
        //    set;
        //}

        //public char HPIsUSFormat_Ind { get; set; }

        //public char HPLeaveMsg_Ind { get; set; }


        public string WorkPhone
        {
            get;
            set;
        }
        //public string WPExtension
        //{
        //    get;
        //    set;
        //}

        //public char WPIsUSFormat_Ind { get; set; }

        //public char WPLeaveMsg_Ind { get; set; }

        public string Fax
        {
            get;
            set;
        }

        //public char FLeaveMsg_Ind { get; set; }

        //public char FIsUSFormat_Ind { get; set; }

        public string Website
        {
            get;
            set;
        }
        public string CountryName
        {
            get;
            set;
        }
        public string StateName
        {
            get;
            set;
        }
        public string CityName
        {
            get;
            set;
        }
        public string CellPhone
        {
            get;
            set;
        }
        //public string fbcomments
        //{
        //    get;
        //    set;
        //}
        //public string MPToText { get; set; }

        //public string MPLeaveMsg_Ind { get; set; }

        //public string Pager { get; set; }

        //public string Address3
        //{
        //    get;
        //    set;
        //}
        //public string Address4
        //{
        //    get;
        //    set;
        //}
        //public int? City_ID1
        //{
        //    get;
        //    set;
        //}
        //public int? State_ID1
        //{
        //    get;
        //    set;
        //}

        //public string Zip1
        //{
        //    get;
        //    set;
        //}
        //public string ind
        //{
        //    get;
        //    set;
        //}
    }
}