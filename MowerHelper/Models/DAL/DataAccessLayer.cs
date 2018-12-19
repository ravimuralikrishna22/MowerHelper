using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MowerHelper.Models.BLL.Admin;
using MowerHelper.Models.BLL.Billing;
using MowerHelper.Models.BLL.Common;
using MowerHelper.Models.BLL.ElectricianServices;
using MowerHelper.Models.BLL.Forums;
using MowerHelper.Models.BLL.LISTER;
using MowerHelper.Models.BLL.MessageStation;
using MowerHelper.Models.BLL.Patients;
using MowerHelper.Models.BLL.Practice;
using MowerHelper.Models.BLL.Providers;
using MowerHelper.Models.BLL.RemotePatient;
using MowerHelper.Models.BLL.Tasks;
using MowerHelper.Models.BLL.Technicians;
namespace MowerHelper.Models.DAL.DataAccessLayer
{
    public abstract class DataAccessLayerBaseClass
    {
         private string _connectionString;
         public string ConnectionString
        {
            get
            {
                string str = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                if (str == null || str.Length <= 0)
                {
                    throw new ApplicationException(@"ConnectionString configuration is missing from you web.config. It should contain  <appSettings><add key=ConnectionString value=database=IssueTrackerStarterKit;server=localhost;Trusted_Connection=true/></appSettings>");
                }
                else
                {
                    return str;
                }
            }
            set { _connectionString = value; }
        }
         public abstract DataSet list_Featuredtherapists(Provider_AdvancedSearch obj);
         public abstract Provider_ContactInfo Get_ContactInfo(int Reference_ID, int ReferenceType_ID);
         public abstract DataSet List_field_description(string id ) ;
         public abstract DataSet GetRenewChargeDetails(string Login_id);
         public abstract DataSet GetBillingChargeDetails(string Login_id);
         public abstract DataSet GetZipBasedOnLatLon(string latitude, string longitude);
         public abstract Provider_ProfilesInfo Get_PrividerProfilesInfo(int Provider_ID);
         public abstract Provider_ProfilesInfo Get_Prividerlicensedetails(int Provider_ID);
         public abstract string Insert_AdvancedSearchInfo(Provider_AdvancedSearch ObjInsAdvancedSearch);
         public abstract string Upd_SimpleProviderDetailsinDirectory(SimpleProvider_DetailsInfo objUpd, ref string Out_Msg);
         public abstract List<clsCountry> GetStatesByCountryId(int? CountryId);
         public abstract List<clsCountry> GetCityByStateId(int? stateid);
         public abstract List<clsCountry> Reg_GetStatesByZipCode(string ZipCode);
         public abstract List<clsCountry> Reg_GetCityByZipCode(string ZipCode, int? State_ID);
         protected CollectionBase GeneratemS_UserCollectionFromReader1(IDataReader returnData, ref int totalRecords)
         {
             var userCollection = new Ms_UserCollection();
             while (returnData.Read())
             {
                 var newMsUser = new Ms_user(Convert.ToInt32(returnData["Login_id"]), Convert.ToString(returnData["Name"]));
                 userCollection.Add(newMsUser);
             }
             if (returnData.NextResult() == true)
             {
                 if (returnData.Read())
                 {
                     totalRecords = Convert.ToInt32(returnData["Total_Records"]);
                 }
             }
             return userCollection;
         }
         public abstract List<Admin_postajob> ChildSections(string prov_id, string practice_id, string login_id, string section_id, string Role_id);
         public abstract List<PracticeblHome> Get_PracticeHomePagePatientsList(PracticeblHome objgethome);
         public abstract List<patients> Get_AppointmentHistory(patients objappHistory);
         public abstract List<Provider_DocumentInfo> Get_DocumentInfo(Provider_DocumentInfo ObjDocument);
         public abstract bool Del_DocumentInfo(int ProviderDocument_ID, int DeletedBy);
         public abstract string Insert_Attachment(int? Provider_ID, string Title, string DocDescription, string Path, string PathEncrypted, string FileName, string IsDisplayPublic, int CreatedBy,
         string verifieddocument, ref string Out_Suffix, double? Document_Size = null);
         public abstract bool Update_Attachment(int? ProviderDocument_ID,  string Title, string DocDescription, string Path, string PathEncrypted, string FileName, int UpdatedBy, string verifieddocument, double? Document_Size = null,
         string Displayinpublic = null);
         public abstract Provider_DocumentInfo GetAttachmentList(int? Provider_ID, int? ProviderDocument_ID);
         public abstract List<Category> GetMs_GetCategorieslist(int LoginID, int RoleID, int Prov_id);
         public abstract void ChangeStatusToArchive(int Message_id, Int32 Role_id, string sentArchive_ind);
         public abstract List<EmailMessageOption> ListEmailMessageOptions(EmailMessageOption EMP, ref int Total_records);
         public abstract List<EmailMessageOption> ListEmail_Main_MessageOptions(EmailMessageOption EMO, ref int Total_records);
         public abstract bool Contact_Us(ContactCog objcont);
         //public abstract List<Provider_TimaTable> GetFacilitiesName(int? Provider_ID, int? Practice_ID);
         public abstract List<Provider_Sitestastics> GET_Statistics(Provider_Sitestastics objstatastics);
         public abstract Provider_Sitestastics Get_StartYearOfProvider(int? Provider_ID);
         public abstract object Insert_NewRegProvidersDetails(Reg_ProvidersDetailInfo ObjInsertDetails, ref string Out_Msg, ref string Out_Provider_ID, ref int OutPractice_ID, ref int OutLogin_ID);
         public abstract object check_validRefferalCode(string CRM_Promocode, ref string Out_Msg);
         public abstract void INS_providerRefferalCode(string Promocode, int provider_id);
         public abstract int PatientInsertCCTransactionDetails(CCProcess objcc, int? loginhistoryid);
         public abstract bool ProviderConfirmation(Reg_ProviderConfirmation ObjConfirmation);
         public abstract bool Reg_Upd_Status(int? ProviderID, string Status_Ind, int? UpdatedBy, string PromocodeID, string vaultid, string customer_id);
         public abstract void Ins_trialpackage_users(int Login_ID);
         public abstract Reg_ProviderConfirmation Reg_GetUserNameAndPassword(int Provider_ID);
         public abstract EmailMessageOption GET_EmailMessage_OptionBasedonID(int id, int? EmailMessage_Option_ID = 0);
         public abstract string Reg_Insert_EmailMessage(Reg_ProviderConfirmation ObjEmailIns);
         public abstract Reg_ProvidersDetailInfo Get_ProviderAddressDetails(int Login_id);
         public abstract bool InsertPassword(string Password, int? UpdatedBy, int? Login_ID);
         public abstract List<ListMessage> ListMessages(int Reciever_ID, int RoleID, int MsgCategory_ID, string ListingMsgsType, int NoofRecords, int PageNo, string OrderBy, string OrderBYitem, ref int Total_Records);
         public abstract List<ListMessage> GetAptrequest_ListMessages(int Login_Id, int NoofRecords, int PageNo, ref int Total_Records);
         public abstract ListMessage LoadMessageDetails(int Message_id, Int32 Role_id);
         public abstract MessageAttachment GetMessageAttachmentById(int Message_ID);
         public abstract ListMessage ReplyLoadMessageDetails(int Message_id, Int32 Role_id);
         public abstract int InsertMessage(Message newMessage);
         public abstract int InsertGroupMessage(Message newMessage);
         public abstract int CreateNewMessageAttachment(MessageAttachment newAttachment);
         public abstract List<MsgPtRequests> GetMsgPatientRequests(MsgPtRequests ObjMsgPtReuests);
         public abstract bool MessageStation_INS_ArchivedMessages(int _id, int Reference_Id);
         public abstract DataSet Providerpublicmessages(Provider_Common obj, string Status);
         public abstract DataSet GetProviderpublicmessage(int Msg_id);
         public abstract Responseparty Get_ResponsibleParty_Info(int Patient_ID, int Practice_ID,  int Provider_ID);
         public abstract void Patient_Archive_Reject_Activate(int? Patient_ID, int Patient_Status_ID, int UpdatedBy, int Provider_ID);
         public abstract DataSet GetMs_GetCategoriesDS(int LoginID, int RoleID);
         public abstract bool Insert_NewRegProviderCreditcard(CCProcess objCreditCard, ref int? Out_CCID, string CardId);
         public abstract bool UpdateCreditCardVaultID(string ProviderID, string Vaultid, int? Out_CCID, string customer_id);
         public abstract bool DeleteCreditCard(string CreditCardInfo_ID, string UserID);
         public abstract List<CCProcess> CreditCard_Get_paymentinfo(CCProcess obj);
         public abstract string GetVaultID(string cardinfoid);
         public abstract List<Tasks> Tasklist(Tasks objTask);
         public abstract bool AddNewTask(Tasks objTask);
         public abstract bool AddNewActionItem(Tasks objAction);
         public abstract List<Tasks> ActionItemsList(Tasks objAction);
         public abstract bool UpdateTask(Tasks objTask);
         public abstract List<Tasks> GetActionItemsList(Tasks objAction);
         public abstract string Insert_Patinet_FT(PatientRegistration objIns, ref string Out_Msg, ref string Out_login_id);
         public abstract PatientRegistration Get_Random_UserCredentials();
         public abstract string Insert_Security_INS_FTUser(PatientRegistration objUser);
         public abstract List<NotesInfo> GetCustomerNotesInfo(NotesInfo objNotesInfo);
         public abstract List<NotesInfo> GetNotesInfo(NotesInfo objNotesInfo);
         public abstract bool InsNotesInfo(NotesInfo objInsNotesInfo);
         public abstract int GetProviderLoginID(int PatientProviderID);
         public abstract NotesInfo GetupdNotesInfo(int objGetNotesInfo);
         public abstract bool UpdNotesInfo(NotesInfo objUpdNotesInfo);
         public abstract DataSet Getgeneralnotesinfo(int? generalnoteid);
         public abstract List<Paymentmethods> CreditCard_list_paymentinfo(Paymentmethods objpayments);
         public abstract List<Paymentmethods> CreditCard_Expirylist_info(Paymentmethods objpayments);
         public abstract List<Provider_ProviderList> Get_ElProviderInProfile(string FirstName, string LastName, Nullable<int> Practice_ID, int PlaceOfService_ID, string OrderByItem, string OrderBy, Nullable<int> Login_ID, int Role_ID, string Status_Ind, int NoOfRecord,
int PageNo, string providertype, ref int TotalRecords, string Email, int? Provider_ID = 0);
         public abstract List<Provider_ProviderList> Get_NewElesignups(string OrderByItem, string OrderBy, int NoOfRecord, string Fromdate, string Todate,
int PageNo, ref int TotalRecords, string Email, int? Provider_ID = 0);
         public abstract bool Update_licensedetails(Provider_ProfilesInfo ObjProfileInfoProv);
         public abstract List<Provider_ProviderList> list_licenseExpired(Nullable<int> Practice_ID, string fromdate, string todate, string OrderByItem, string OrderBy, int NoOfRecord, int PageNo, string toreferenceids);
         public abstract List<Provider_ProviderList> signups_monthWisecount(string Year);
         public abstract bool InslicenseRenewalsMailStatus(string ToreferenceId, string FacilityIds = null);
         public abstract string UPDATE_Email_Message_Status(string EmailMessage_Transaction_IDs);
         public abstract List<PTHome> GetPracticesOfPatient(int PatientLogin_ID, int? practice_ID, int? ProviderID = null);
         public abstract Ms_UserCollection GetUsersByRoleid(int intSearchType, int RoleID, int PageNo, ref string DisplayName, ref int Total_Records, string lastname = null);
         public abstract List<Admin_postajob> verificationusersList(Admin_postajob objusersList, ref int Total_Records);
         public abstract bool Inactivate_Provider_Status(int Provider_ID);
         public abstract bool Upd_Vacation_Provider_Status(int Provider_ID);
         public abstract bool Del_Provider(int Provider_ID, int DeletedBy);
         public abstract List<Login_history> Admin_Provider_Login_history(Login_history objlogindata, ref int Total_Records);
         public abstract PracticeblHome Get_SendMailsToProviderDetails(int? providerlogin_id, string Provider_ID = null);
         public abstract bool Temp_Ins_Password(Provider_Password obj);
         public abstract object Insert_verificationusers(Admin_postajob ObjIns, ref string Out_Msg, ref int Out_verification_User_ID, ref int Out_Login_ID, int? loginhistoryid = null);
         public abstract List<Admin_postajob> verificationusersView(int? loginID, int? verification_User_ID);
         public abstract string verificationuserUpdate(Admin_postajob objposters);
         public abstract PracticeblHome UpdateDetails(int providerid, string EmailID);
         public abstract string InActive_verificationusers(Admin_postajob objstatus, int Loginhistory_id);
         public abstract List<WebConfigSettings> Admin_List_WebConfigSettings(WebConfigSettings objWebConfigSettings, ref int Total_Records);
         public abstract void Ad_UPD_WebConfigSettings(WebConfigSettings objWebConfigSettings);
         public abstract void Ad_INS_WebConfigSettings(WebConfigSettings objWebConfigSettings);
         public abstract List<ErrorsList> EList(ErrorsList objL);
         public abstract List<EmailMessageOption> ADMIN_List_Main_Email_OptionTypes();
         public abstract List<EmailMessageOption> ADMIN_List_ExistingEmail_OptionTypes(EmailMessageOption OptionTypeID = null);
         public abstract void InsertEmailMessageOption(EmailMessageOption EMP);
         public abstract void UpdateEmailMessageOption(EmailMessageOption EMP);
         public abstract void InsertMainEmailMessageOption(int Email_OptionType_ID, string Message_Title);
         public abstract void UpdatemainEmailMessageOption(int EmailMessage_Option_ID, string Message_Title);
         public abstract EmailMessageOption GetEmail_Main_MessageOptionbyID(int id);
         public abstract List<ErrorsList> DDL_CLS(ErrorsList objEL);
         public abstract List<ErrorsList> DDL_MTHDS(ErrorsList objM);
         public abstract List<Login_history> Admin_List_Login_history(Login_history objloginhistory, ref int Total_Records, string Rolename);
         public abstract List<pagenumber> admin_listpagenumbers(pagenumber objlistpages, ref int Total_Records);
         public abstract void updatepagelist(pagenumber objview, ref string outmsg);
         public abstract void inserpagenumber(pagenumber objview, ref string outmsg);
         public abstract void del_listpagenumbers(int Page_ID, int Application_ID);
         public abstract List<pagenumber> viewpagelist(pagenumber objview);
         public abstract List<pagenumber> dropdownlist(Nullable<int> Section_ID, string SearchBy);
         public abstract DataSet Admin_List_Site_history(string IPAddress, string ProjectName, string StateName, string CityName, string CountryName, string OrderBy, string OrderBYItem, int PageNo, int NoOfrec, string FromDate,
         string ToDate, ref int Total_Records);
         public abstract DataSet Admin_List_Site_history_Mobile(string FromDate, string ToDate, string orderby, string orderbyitem);
         public abstract DataSet Admin_Get_Site_history_Mobile(int Mobileowner_id);
         public abstract object Ins_module_field_description(Referrals list);
         public abstract List<Referrals> BindCaregories();

         public abstract List<Referrals> BindArticalIndex(Referrals OptionTypeID = null);
         public abstract object upd_fieldname_description(Referrals list);
         public abstract List<MediaInfo> GetMediaInfo(MediaInfo objMediaInfo);
         public abstract List<MediaInfo> GetVideoInfo(MediaInfo objMediaInfo);
         public abstract string InsertMediaInfo(MediaInfo ObjInsMediaInfo, ref string out_Msg, ref string Loc_ImagePathSuffix);
         public abstract List<MediaInfo> GetNewMediaInfo(MediaInfo ObjNewMediaInfo);
         public abstract string UpdateMediaInfo(MediaInfo ObjUpdMediaInfo, ref string Out_Msg);
         public abstract bool DeleteMediaInfo(MediaInfo ObjDelMediaInfo);
         public abstract List<ListContactInfo> List_ContactUs(ListContactInfo objLstCont);
         public abstract ListContactInfo View_Contacts(int Contact_ID);
         public abstract ListContactInfo UPD_ReplyStatus(int Contact_ID);
         public abstract bool DEL_Contacts(int Contact_ID);
         public abstract List<MediaInfo> GetMediaInfo1(MediaInfo objMediaInfo);
         public abstract ErrorsList Get_Error_Details(int Log_ID);
         public abstract DataSet Provider_DocumentuploadDetails(Provider_DocumentInfo ObjDocument);
         public abstract List<LogInfo> Get_EvenLogInfo(LogInfo objLogInfo);
         public abstract List<LogInfo> Get_PatientLoginfo(string EventLogID, string EventTypeID);
         public abstract int InsertCreditCardResponse(CCProcess obj);
         public abstract string LoadCreditCardInfo(CCProcess obj);
         public abstract bool InsTechniciansInfo(TechniciansInfo objInsTechniciansInfo,ref string Out_Msg);
         public abstract List<TechniciansInfo> GetTechniciansInfo(TechniciansInfo objNotesInfo);
         public abstract TechniciansInfo GetupdTechnicianInfo(int Technician_id);
         public abstract bool UpdTechnicianInfo(TechniciansInfo objUpdTechnicianInfo, ref string Out_Msg);
         public abstract bool upd_technicianimage(int Technician_id, string imagepath);
         public abstract List<Electrician_Services> GET_ElectricianServices(Electrician_Services objservices, ref int Totalnoofrecords);
         public abstract string Insert_ElectricianServices(Electrician_Services objservices);
         public abstract bool Del_ServiceInfo(int BillingService_ID, int UpdatedBy);
         public abstract string Insert_ServiceChargeInfo(CCProcess ObjInsServiceCharge);
         public abstract string upgrade_ServiceCharge(CCProcess ObjInsServiceCharge);
         public abstract string Insert_ServicePaymentInfo(CCProcess ObjInsServicePayment);
         public abstract List<Emaillog> List_Emaillog(Emaillog obj, ref int Total_records);
         public abstract List<Emaillog> DDL_category(Emaillog objct);
         public abstract List<Referrals> Admin_List_Articles(Referrals list);
         //public abstract DataSet Get_videos(int id);
         public abstract object upd_videos(Articlevideos list);
         public abstract object Del_videos(int id);
         public abstract List<Articlevideos> List_Relatedvideos(Articlevideos list);
         public abstract object Insert_videos(Articlevideos list);
         public abstract List<Articlevideos> List_videos(Articlevideos list, string videoIDs, string tabInd = null);
         public abstract DataSet Get_Relatedvideos(int id);
         public abstract DataSet Get_RelatedArticle(int id);
         public abstract object Ins_Article(Referrals list);
         public abstract object upd_Articles(Referrals id);
         public abstract object Del_Article(int id);
         public abstract List<Referrals> List_publicArticles(Referrals list, ref int Total_Records);
         public abstract List<Referrals> List_publicArticles1(Referrals list,string ArticleIDs, ref int Total_Records);
         public abstract List<Referrals> ListBindArticles(string strSearchCondition, string strText, Referrals list, ref int Total_Records);
         public abstract object Ins_ArticleAuthor(Referrals list);
         public abstract object Ins_ArticleIndexing(Referrals list);
         public abstract object Upd_ArticleIndexing(Referrals list);
         public abstract List<Referrals> Getarticles1(Referrals objarticles);
         public abstract System.Data.DataSet Get_Articles(int id);
         public abstract DataSet Get_ArticleIndexing(int id);
         public abstract DataSet GetArticlesORVideosCount(string SiteStatisticID, string AVInd, string Title = null, string Desc = null);
         public abstract DataSet Get_RelatedArticleID(int id);
         public abstract DataSet Get_RelatedvideoID(int id);
         public abstract DataSet list_ArticlesCategories(string strSearchCondition, string strText);
         public abstract List<Articlevideos> list_CategorieswiseIndexes(int CategoryID, string strSearchCondition, string strText);
         public abstract string Insert_Forum(Forums objforum);

         public abstract string INS_UPD_Forum_Moderators(Forums obj);
         public abstract List<Forums> Get_Forum(Forums objforum);
         public abstract bool Del_Forum(int Forum_ID);

         public abstract bool Update_Forum(Forums objforum);

         public abstract List<Forums> Get_Topics_forum(Forums objforum);
         public abstract List<Forums> Search_ForumTopic(Forums obj);
         public abstract List<Forums> Search_ForumMessages(Forums obj);
         public abstract bool Insert_Topic(Forums objforum);
         public abstract bool Del_FormTopics(int TopicID, int UpdatedBy);
         public abstract bool Upd_TopicStatus(int TopicID, string Status_Ind);
         public abstract List<Forums> Get_FourmTopicDescription(int Forum_ID, int? Topic_ID);
         public abstract List<Forums> Get_Moderators(int? Forum_ID);
         public abstract int Insert_ReplyMsg(Forums objforum);

         public abstract bool Del_MessagesForTopics(int Message_ID, int UpdatedBy);
         public abstract bool Upd_MessageStatus(int Message_ID, string Status_Ind);
         public abstract DataSet Get_FourmNameTopicDescription(int Forum_ID, int? Topic_ID, string Login_id);
         public abstract List<Forums> Get_Reply_ForumTopicMessages(int Reply_Message_Id);
         public abstract List<PracticeblHome> DebugItemsList(PracticeblHome objdebug);
         public abstract List<Login_history> WebLoginHistoryDetails(Login_history objloginhistory, ref int Total_Records);
    }
    
}