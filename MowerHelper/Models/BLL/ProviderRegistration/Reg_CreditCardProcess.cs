using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MowerHelper.Models.Classes;
namespace MowerHelper.Models.BLL.ProviderRegistration
{
    public class Reg_CreditCardProcess
    {
        private int _Provider_ID;
        private int _Practice_ID;
        public int Provider_ID
        {
            get { return _Provider_ID; }
            set { _Provider_ID = value; }
        }
        public int Practice_ID
        {
            get { return _Practice_ID; }
            set { _Practice_ID = value; }
        }
        public int CreditCardType_ID { get; set; }
        public string code { get; set; }
        public string RegInd { get; set; }
        public Reg_CreditCardProcess()
        {
        }
        public string FullName { get; set; }
        public string EFullName { get; set; }
        public List<Reg_CreditCardProcess> Get_Creditcardtype()
        {
            List<Reg_CreditCardProcess> creditTypeList = new List<Reg_CreditCardProcess>();
            clsCommonFunctions objcomman = new clsCommonFunctions();
            DataSet ds = new DataSet();
            ds = objcomman.GetDataSet("Help_dbo.st_CreditCard_List_CreditCardTypes");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Reg_CreditCardProcess list = new Reg_CreditCardProcess();
                        if (dr["CreditCardType_ID"]!=null)
                        {
                            list.CreditCardType_ID =Convert.ToInt32(dr["CreditCardType_ID"]);
                        }
                        if (dr["Code"] != null)
                        {
                            list.code = dr["Code"].ToString();
                        }
                        creditTypeList.Add(list);
                    }
                }
            }
            return creditTypeList;
        }
    }
}