using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.DAL.CDAL;

namespace MowerHelper.Models.DAL.CDAL
{
    public abstract class ResellerDataAccessLayer
    {
        public abstract string Ins_Upd_CRM_Agent_Details(CRMAgent obj);
        public abstract List<CRMAgent> GET_CRM_Agent_Details(CRMAgent obj);
        public abstract string Active_InActive_CRM_Agent_Details(CRMAgent obj);
    }
}