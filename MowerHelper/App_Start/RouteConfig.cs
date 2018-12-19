using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MowerHelper
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.Add(new Route("Home/Login", new MvcRouteHandler())
            //{
            //    Defaults = new RouteValueDictionary(new { controller = "Home", action = "Login" }),
            //    DataTokens = new RouteValueDictionary(new { scheme = "https" })
            //});
           // routes.MapRoute(
           //name: "ZipcodeCityTherapists",
           //url: "Zipcodes/{city}/{distance}/{id}-Therapists",
           //defaults: new { Controller = "Search", action = "ListOfProviders" });
            //routes.Clear();
          //  routes.MapRoute(
          //name: "ZIPStateCities",
          //url: "ZIPStateCities/{id}/{State}/{city}-Therapists",
          //defaults: new { Controller = "Search", action = "ListOfProviders", id = UrlParameter.Optional });

            routes.MapRoute(
        name: "ZipcodeSearch",
        url: "Zipcodes/{id}-MowerHelpers",
        defaults: new { Controller = "Search", action = "ListOfProviders" },
        constraints: new { id = @"\d+" });

            routes.MapRoute(
        name: "StateCities",
        url: "StateCities/{ctyid}/{StateNm}/{city}-MowerHelpers",
        defaults: new { Controller = "Search", action = "ListOfProviders" });

            routes.MapRoute(
        name: "TopCityTherapists",
        url: "Statecity/{StateNm}/{city}-MowerHelpers",
        defaults: new { Controller = "Search", action = "ListOfProviders" });


            routes.MapRoute(
           name: "ZipcodeTherapists",
           url: "Zipcodes/{distance}/{id}-MowerHelpers",
           defaults: new { Controller = "Search", action = "ListOfProviders" });

            routes.MapRoute(
          name: "Zipstatecity",
          url: "Zipstatecities/{id}/{StateNm}/{city}-MowerHelpers",
          defaults: new { Controller = "Search", action = "ListOfProviders"});

            routes.MapRoute(
          name: "ZIPRadiusStateCities",
          url: "ZIPRadiusStateCities/{id}/{distance}/{StateNm}/{city}-MowerHelpers",
          defaults: new { Controller = "Search", action = "ListOfProviders" });

           

            routes.MapRoute(
            name: "Directory",
            url: "Directory",
            defaults: new { Controller = "Search", action = "Directory" });

         //   routes.MapRoute(
         //name: "stateCityid",
         //url: "{State}/{ctyid}/{city}-Therapists",
         //defaults: new { Controller = "Search", action = "ListOfProviders" });

            routes.MapRoute(
          name: "Cities",
          url: "City/{city}-MowerHelpers",
          defaults: new { Controller = "Search", action = "ListOfProviders"});



            routes.MapRoute(
            name: "ListOfProviders",
            url: "{StateNm}-MowerHelpers",
            defaults: new { Controller = "Search", action = "ListOfProviders" });


            routes.MapRoute(
           name: "International",
           url: "Country/{countryname}-Mower Helpers",
           defaults: new { Controller = "Search", action = "ListOfProviders" });


            routes.MapRoute(
        name: "Issuestatecity",
        url: "Issuestatecity/{issuesnm}/{issueid}/{StateNm}/{city}-MowerHelpers",
        defaults: new { Controller = "Search", action = "ListOfProviders" });

            routes.MapRoute(
       name: "languagestatecity",
       url: "languagestatecity/{language}/{StateNm}/{city}-Mower Helpers",
       defaults: new { Controller = "Search", action = "ListOfProviders" });
            routes.MapRoute(
       name: "Issuecity",
       url: "Issuecity/{issuesnm}/{issueid}/{city}-MowerHelpers",
       defaults: new { Controller = "Search", action = "ListOfProviders" });

            routes.MapRoute(
       name: "languagecity",
       url: "languagecity/{language}/{city}-MowerHelpers",
       defaults: new { Controller = "Search", action = "ListOfProviders" });

            routes.MapRoute(
      name: "Issuestate",
      url: "Issuestate/{issuesnm}/{issueid}/{StateNm}-MowerHelpers",
      defaults: new { Controller = "Search", action = "ListOfProviders" });

            routes.MapRoute(
       name: "languagestate",
       url: "languagestate/{language}/{StateNm}-MowerHelpers",
       defaults: new { Controller = "Search", action = "ListOfProviders" });

            routes.MapRoute(
      name: "ZipIssue",
      url: "ZipIssue/{issuesnm}/{issueid}/{id}-MowerHelpers",
      defaults: new { Controller = "Search", action = "ListOfProviders" });

            routes.MapRoute(
   name: "ZipMilesIssue",
   url: "ZipMilesIssue/{issuesnm}/{issueid}/{distance}/{id}-MowerHelpers",
   defaults: new { Controller = "Search", action = "ListOfProviders" });

            routes.MapRoute(
       name: "Ziplanguage",
       url: "Ziplanguage/{language}/{id}-Mower Helpers",
       defaults: new { Controller = "Search", action = "ListOfProviders" });

            routes.MapRoute(
       name: "ZipMileslanguage",
       url: "ZipMileslanguage/{language}/{distance}/{id}-MowerHelpers",
       defaults: new { Controller = "Search", action = "ListOfProviders" });

            routes.MapRoute(
       name: "Issues",
       url: "Issues/{issueid}/{issuesnm}-Mower Helpers",
       defaults: new { Controller = "Search", action = "ListOfProviders" });

            routes.MapRoute(
                   name: "speaks",
                   url: "speaks/{language}-Mower Helpers",
                   defaults: new { Controller = "Search", action = "ListOfProviders" });

            routes.MapRoute(
             name: "DefaultSearch",
             url: "Directories/Directory",
             defaults: new { Controller = "Search", action = "ListOfProviders" });

            routes.MapRoute(
           name: "FAQSHome",
           url: "Public/FrequentlyAskedQuestions",
           defaults: new { Controller = "Search", action = "FAQs" });

            routes.MapRoute(
           name: "Login",
           url: "Private/Memberlogin",
           defaults: new { Controller = "Home", action = "login" });

            routes.MapRoute(
           name: "Contact",
           url: "ContactUs",
           defaults: new { Controller = "Home", action = "contact" });
            routes.MapRoute(
          name: "Termsofuse",
          url: "Termsofuse",
          defaults: new { Controller = "Home", action = "Termsofuse" });
            routes.MapRoute(
       name: "MoreHelp",
       url: "MoreHelp",
       defaults: new { Controller = "Home", action = "PartnerSites" });

            routes.MapRoute(
        name: "LearnExplore/Articles",
        url: "LearnExplore/Articles",
        defaults: new { Controller = "LearnExplore", action = "Articles" });
            routes.MapRoute(
          name: "AdvancedSearch",
          url: "AdvancedSearch",
          defaults: new { Controller = "AdvancedSearch", action = "ProviderSearch" });

            routes.MapRoute(
          name: "Register",
          url: "Registration/signup",
          defaults: new { Controller = "Registration", action = "Index" });
           
            routes.MapRoute(
         name: "Register2",
         url: "RegisterNew/Index",
         defaults: new { Controller = "Registernew", action = "Index" });

            routes.MapRoute(
name: "Register1",
url: "refferal/{Promocode}",
defaults: new { Controller = "Registration", action = "Index" });

            routes.MapRoute(
          name: "CCProcess",
          url: "Registration/{Uid1}/RegCreditCardProcess/{Uid2}/Process",
          defaults: new { Controller = "Registration", action = "CCProcess" });
            routes.MapRoute(
               name: "verification",
               url: "Home/ResetPassword/{accesscode}",
               defaults: new { Controller = "Home", action = "verification" }
               );
            routes.MapRoute(
         name: "Confirmation",
         url: "Registration/{Uid1}/Confirmation/{Uid2}/Registrationcomplete",
         defaults: new { Controller = "Registration", action = "DisplayLogindetails" });
            routes.MapRoute(
name: "Home",
url: "Home",
defaults: new { Controller = "Home", action = "Index" });

            routes.MapRoute(
         name: "Logout",
         url: "Logout",
         defaults: new { Controller = "Home", action = "Logout" });

            routes.MapRoute(
        name: "SessionExpire",
        url: "SessionExpire",
        defaults: new { Controller = "Home", action = "SessionExpire" });

            routes.MapRoute(
       name: "SessionExpirepopup",
       url: "SessionExpirepopup",
       defaults: new { Controller = "Home", action = "SessionExpirepopup" });

            routes.MapRoute(
      name: "SubmitFaq",
      url: "Faqs/SubmitQuestion",
      defaults: new { Controller = "Search", action = "SubmitFAQ" });

            routes.MapRoute(
      name: "AnswerFaq",
      url: "Faqs/Answer/{id}",
      defaults: new { Controller = "Search", action = "AnswerFAQ" });
            /* Added by Pavan For ProviderProfile*/
            routes.MapRoute(
             name: "ProviderProfile",
             url: "MowerHelpers/{Therapistname}/{state}/{city}/{zip}/{randomnumber}/Profile",
             defaults: new { controller = "Public", action = "Profile" }
                 );

            routes.MapRoute(
            name: "Providerservices",
            url: "MowerHelpers/{Therapistname}/{state}/{city}/{zip}/{randomnumber}/Services",
            defaults: new { controller = "Public", action = "Services" }
                );

            routes.MapRoute(
          name: "ProviderContact",
          url: "MowerHelpers/{Therapistname}/{state}/{city}/{zip}/{randomnumber}/Contact",
          defaults: new { controller = "Public", action = "Contact" }
              );
            routes.MapRoute(
          name: "ProviderComments",
          url: "TennisCoaches/{Therapistname}/{state}/{city}/{zip}/{randomnumber}/Comments",
          defaults: new { controller = "Public", action = "Comments" }
              );
            routes.MapRoute(
         name: "ProviderDirections",
         url: "MowerHelpers/{Therapistname}/{state}/{city}/{zip}/{randomnumber}/Directions",
         defaults: new { controller = "Public", action = "Directions" }
             );

            routes.MapRoute(
        name: "Sendmailtofriend",
        url: "MowerHelpers/{Therapistname}/{state}/{city}/{zip}/{randomnumber}/MailToFriend",
        defaults: new { controller = "Public", action = "MailToFriend" }
            );

            routes.MapRoute(
        name: "PrintProviderProfile",
        url: "MowerHelpers/{Therapistname}/{state}/{city}/{zip}/{randomnumber}/PrintPages",
        defaults: new { controller = "Public", action = "PrintProfile" }
            );
            routes.MapRoute(
       name: "ProviderGreetings",
       url: "MowerHelpers/{Therapistname}/{state}/{city}/{zip}/{randomnumber}/MowerHelpersGreetings",
       defaults: new { controller = "Public", action = "ProviderGreetings" }
           );
            routes.MapRoute(
     name: "ElectricianSchedule",
     url: "MowerHelpers/{Therapistname}/{state}/{city}/{zip}/{randomnumber}/MowerHelpersSchedule/{date}",
     defaults: new { controller = "Public", action = "ElectricianSchedule",date=UrlParameter.Optional }
         );

            routes.MapRoute(
     name: "ProviderDocuments",
     url: "ProviderDocuments/{id}/{Therapistname}",
     defaults: new { controller = "Public", action = "ProviderDocuments" }
         );
            routes.MapRoute(
    name: "ProviderVerification",
    url: "MowerHelpers/{Therapistname}/{state}/{city}/{zip}/{randomnumber}/MowerHelpersVerification",
    defaults: new { controller = "Public", action = "ProviderVerification" }
        );
            routes.MapRoute(
                            name: "VideoPlay",
                            url: "MowerHelpers/{Therapistname}/{state}/{city}/{zip}/{randomnumber}/Video/{greetingid}",
                            defaults: new { controller = "Public", action = "Playvideo" }
                        );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Authorization", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "Activation",
               url: "Home/Activation/user/{code}",
               defaults: new { Controller = "Home", action = "Activate" }
               );
            
        }
    }
}