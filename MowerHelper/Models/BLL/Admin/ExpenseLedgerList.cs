using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MowerHelper.Models.BLL.Admin
{
    public class ExpenseLedgerList
    {
        public static int NoofRecords
        {
            get;
            set;
        }
        public int SLNo
        {
            get;
            set;
        }
        public int? Practice_ID
        {
            get;
            set;
        }
        public int Exp_Ledger_ID
        {
            get;
            set;
        }
        public int Provider_ID
        {
            get;
            set;
        }
        public DateTime Exp_Date
        {
            get;
            set;
        }
        public string Item
        {
            get;
            set;
        }
        public string Quantity
        {
            get;
            set;
        }
        public string TotalCost
        {
            get;
            set;
        }
        public string CheckNumber
        {
            get;
            set;
        }
        public string Vendor
        {
            get;
            set;
        }
        public string Deductible
        {
            get;
            set;
        }
        public string Category
        {
            get;
            set;
        }
        public string Notes
        {
            get;
            set;
        }
                public int Exp_Category_ID
        {
            get;
            set;
        }
        public string Exp_Category
        {
            get;
            set;
        }
        public string tax
        {
            get;
            set;
        }
        public string PracticeName
        {
            get;
            set;
        }
        public string Imagepath { get; set; }
        public string providername { get; set; }
        public ExpenseLedgerList()
        {
        }
        public ExpenseLedgerList(int Dummyint, int Dummyint1, int VExp_Category_ID, string VExp_Category)
{
	Exp_Category_ID = VExp_Category_ID;
	Exp_Category = VExp_Category;
}


        public ExpenseLedgerList(int VProvider_ID, string VProviderName, int VExp_Ledger_ID, int VExp_Category_ID, string VCategory, string VExp_Date,
string VItem, string VQuantity, string VTotalCost, string VCheckNumber, string VVendor, string VDeductible, string VNotes, string VImagepath)
{
	Provider_ID = VProvider_ID;
    providername = VProviderName;
	Exp_Ledger_ID = VExp_Ledger_ID;
	Exp_Category_ID = VExp_Category_ID;
	Category = VCategory;
    Exp_Date = Convert.ToDateTime(VExp_Date);
	Item = VItem;
	Quantity = VQuantity;
	TotalCost = VTotalCost;
	CheckNumber = VCheckNumber;
	Vendor = VVendor;
	Deductible = VDeductible;
	Notes = VNotes;
    Imagepath = VImagepath;
}



        public ExpenseLedgerList(int VSLNo, int? VPractice_ID, int VExp_Ledger_ID, int VProvider_ID, DateTime VExp_Date, string VItem, string VQuantity, string VTotalCost, string VCheckNumber, string VVendor,
string VDeductible, string VCategory, string VNotes,string VImagepath)
{
	SLNo = VSLNo;
	Practice_ID = VPractice_ID;
	Exp_Ledger_ID = VExp_Ledger_ID;
	Provider_ID = VProvider_ID;
	Exp_Date = VExp_Date;
	Item = VItem;
	Quantity = VQuantity;
	TotalCost = VTotalCost;
	CheckNumber = VCheckNumber;
	Vendor = VVendor;
	Deductible = VDeductible;
    if (VDeductible == "0")
    {
        tax = "Yes";
    }
    else if (VDeductible == "1")
    {
        tax = "No";
    }
    else
    {
        tax = "Not sure";
    }
	Category = VCategory;
	Notes = VNotes;
    Imagepath = VImagepath;
}
    }

}