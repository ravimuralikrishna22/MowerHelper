using System.Collections.Generic;
using System.Data;
using MowerHelper.Models.BLL.BLLSchedule;
using MowerHelper.Models.BLL.MessageStation;
namespace MowerHelper.Models.DAL.DALSchedule
{
    public abstract class SCHDataAccessLayerBaseClass
    {
        public abstract DataSet GetDayTemplate(string ProviderID, string ApptDate, string ShowAll, string WeekDay);
        public abstract DataSet GetDayAppointments(string ProviderID,  string ApptDate, string ShowAll,string Tech_id);
        public abstract List<GetAppointment> GetPatientAppointment(string AppointmentID, string GroupNo, string AppointmentDate);
        public abstract List<GetAppointment> GetPendingAppointment(string AppointmentID, string AppointmentDate);
        public abstract List<Category> SentEmailLog(Category objCategory);
        public abstract int EmailCategoryCount(int intCategoryID);
        public abstract DataSet GetWeekAppointments(string ProviderID,  string FromDate, string ToDate,string Techid);
        public abstract DataSet GetMonthAppointments(string ProviderID, string Month, string Year, string ApptStatusID,string Tech_id);
        public abstract DataSet GetProviderOpenSlots(int? ProviderId, string ApptStartdate, string ApptEnddate, string Starttime, string Endtime, int? Duration);
        public abstract List<GetAppointment> GetCustomerdetails(string Email, string Phonenumber, string Provider_ID);
    }
}