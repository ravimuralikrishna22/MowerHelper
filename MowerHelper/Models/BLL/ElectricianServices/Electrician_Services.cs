using MowerHelper.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.DAL.DataAccessLayer;
using System.Data;

namespace MowerHelper.Models.BLL.ElectricianServices
{
    public class Electrician_Services
    {
        public int? BillingService_ID { get; set; }
        public int? Billing_ServiceType_ID { get; set; }
        public int? Quantity { get; set; }
        public string BillingServiceDescription { get; set; }
        public string BillingServiceTitle { get; set; }
        public int? CreatedBy { get; set; }
        public string StatusInd { get; set; }
        public string Chargevalues { get; set; }
        public string IsRenewed { get; set; }
        public string IsUpgradable { get; set; }
        public int? NoOfRecords { get; set; }
        public int? PageNo { get; set; }
        public string OrderByItem { get; set; }
        public string OrderBy { get; set; }
        public int Totalnoofrecords { get; set; }
        public static List<Electrician_Services> GET_ElectricianServices(Electrician_Services objservices, ref int Totalnoofrecords)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.GET_ElectricianServices(objservices, ref Totalnoofrecords);
        }
        public static string Insert_ElectricianServices(Electrician_Services objservices)
        {
            SQLDataAccessLayer DALLayer = new SQLDataAccessLayer();
            return DALLayer.Insert_ElectricianServices(objservices);
        }
      
        public DataSet showElectricianService(Electrician_Services objservice)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.showElectricianService(objservice);
        }
        public DataSet GetlectricianServiceDetails(Electrician_Services objservice)
        {
            SQLDataAccessLayer DBlayer = new SQLDataAccessLayer();
            return DBlayer.GetlectricianServiceDetails(objservice);
        }
        public static bool Del_ServiceInfo(int BillingService_ID, int UpdatedBy)
        {
            SQLDataAccessLayer DBLayer = new SQLDataAccessLayer();
            return DBLayer.Del_ServiceInfo(BillingService_ID, UpdatedBy);
        }
    }
}