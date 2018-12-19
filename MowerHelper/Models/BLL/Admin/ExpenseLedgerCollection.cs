using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.Classes;
using MowerHelper.Models.DAL;

namespace MowerHelper.Models.BLL.Admin
{
    public class ExpenseLedgerCollection
    {
       
        public int? ddlrecords { get; set; }
        public string txt_FromDate { get; set; }
        public string txt_ToDate { get; set; }
        public string dt_filter { get; set; }
        public string sortdir { get; set; }
        public string sort { get; set; }
        public int? page { get; set; }
        public string txtCategory { get; set; }
        //public StateCity GetCategorylist { get; set; }

        public List<ExpenseLedgerList> GetExpLedgerDataList(int Provider_ID, int NoofRecords, int PageNo, string OrderBy, string OrderByItem, string strStartDate, string strEndDate, string strCategory)
        {
            SQLDataAccessLayer objLedgerList = new SQLDataAccessLayer();
            return objLedgerList.GetExpLedgerDataList(Provider_ID, NoofRecords, PageNo, OrderBy, OrderByItem, strStartDate, strEndDate, strCategory);
        }

        public List<ExpenseLedgerList> GetAllCatagories()
        {
            SQLDataAccessLayer objCatagory = new SQLDataAccessLayer();
            return objCatagory.GetAllCatagories();
        }
        public object InsertExpense_Ledger(int? Reference_ID, string Exp_Category, ref string Out_Msg)
        {
            SQLDataAccessLayer ObjDelExpense = new SQLDataAccessLayer();
            return ObjDelExpense.InsertExpense_Ledger(Reference_ID, Exp_Category,ref Out_Msg);
        }
        public int Insert_Expense_Ledger( int? Provider_ID, int? Exp_Category_ID, string Exp_Date, string Item, string Quantity, string TotalCost, string CheckNumber, string Vendor,
          string Deductible, string Notes, int? CreatedBy, int? RoleID, string Imagepath)
        {
            SQLDataAccessLayer objINSExp = new SQLDataAccessLayer();
            return objINSExp.Insert_Expense_Ledger(Provider_ID, Exp_Category_ID, Exp_Date, Item, Quantity, TotalCost, CheckNumber, Vendor,
            Deductible, Notes, CreatedBy, RoleID, Imagepath);
        }
        public int UPT_ExpenseLedger(int? Exp_Ledger_ID, int? Provider_ID, int? PlaceOfService_ID, int? Exp_Category_ID, string Exp_Date, string Item, string Quantity, string TotalCost, string CheckNumber, string Vendor,
string Deductible, string Notes, string UpdatedBy, int? RoleID, string Imagepath)
        {
            SQLDataAccessLayer objUpdateExpense = new SQLDataAccessLayer();
            return objUpdateExpense.UPT_ExpenseLedger(Exp_Ledger_ID, Provider_ID, PlaceOfService_ID, Exp_Category_ID, Exp_Date, Item, Quantity, TotalCost, CheckNumber, Vendor,
            Deductible, Notes, UpdatedBy, RoleID,Imagepath);
        }


        public List<ExpenseLedgerList> GetExpenseLedgerDetailsBasedOnID(int Exp_Ledger_ID)
        {
            SQLDataAccessLayer objExpenses = new SQLDataAccessLayer();
            return objExpenses.GetExpenseLedgerDetailsBasedOnID(Exp_Ledger_ID);
        }
        public object DeleteExpense_Ledger(int? Exp_Ledger_ID)
        {
            SQLDataAccessLayer ObjDelExpense = new SQLDataAccessLayer();
            return ObjDelExpense.DeleteExpense_Ledger(Exp_Ledger_ID);
        }
    }
}