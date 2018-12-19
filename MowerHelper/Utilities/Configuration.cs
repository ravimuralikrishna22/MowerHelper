using System.Collections.Generic;
using PayPal;
using PayPal.Api;
using MowerHelper.Models.Classes;
using System.Web;
using System;

namespace PayPal.Tenniscoach
{
    public static class Configuration
    {
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        // Static constructor for setting the readonly static members.
        static Configuration()
        {
            try
            {
                var config = GetConfig();
                //clsWebConfigsettings obj = new clsWebConfigsettings();
                if (clsWebConfigsettings.GetConfigSettingsValue("PaypalLiveMode").ToUpper() == "NO")
                {
                    if (clsWebConfigsettings.GetConfigSettingsValue("PaypalRestAPITestClientid") != "")
                    {
                        ClientId = clsWebConfigsettings.GetConfigSettingsValue("PaypalRestAPITestClientid");
                    }
                    if (clsWebConfigsettings.GetConfigSettingsValue("Paypalrestapitestsecretkey") != "")
                    {
                        ClientSecret = clsWebConfigsettings.GetConfigSettingsValue("Paypalrestapitestsecretkey");
                    }
                }
                else if (clsWebConfigsettings.GetConfigSettingsValue("PaypalLiveMode").ToUpper() == "YES")
                {
                    if (clsWebConfigsettings.GetConfigSettingsValue("PaypalRestAPILiveClientid") != "")
                    {
                        ClientId = clsWebConfigsettings.GetConfigSettingsValue("PaypalRestAPILiveClientid");
                    }
                    if (clsWebConfigsettings.GetConfigSettingsValue("PaypalrestapiLivesecretkey") != "")
                    {
                        ClientSecret = clsWebConfigsettings.GetConfigSettingsValue("PaypalrestapiLivesecretkey");
                    }
                }

            }
            catch (Exception ex)
            {

                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "Configuration", "BindCategory", HttpContext.Current.Request, HttpContext.Current.Session);
            }
            //ClientId = config["clientId"];
            //ClientSecret = config["clientSecret"];
        }

        // Create the configuration map that contains mode and other optional configuration details.
        public static Dictionary<string, string> GetConfig()
        {
            try
            {
            return ConfigManager.Instance.GetProperties();
            }
        catch (Exception ex)
        {

            clsExceptionLog clsex = new clsExceptionLog();
            clsex.LogException(ex, "Configuration", "GetConfig", HttpContext.Current.Request, HttpContext.Current.Session);
            return null;
        }
        }

        // Create accessToken
        private static string GetAccessToken()
        {
            // ###AccessToken
            // Retrieve the access token from
            // OAuthTokenCredential by passing in
            // ClientID and ClientSecret
            // It is not mandatory to generate Access Token on a per call basis.
            // Typically the access token can be generated once and
            // reused within the expiry window      
            try
            {
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
            }
            catch (Exception ex)
            {

                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "Configuration", "GetAccessToken", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }

        // Returns APIContext object
        public static APIContext GetAPIContext(string accessToken = "")
        {
            // ### Api Context
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 
            try{
            var apiContext = new APIContext(string.IsNullOrEmpty(accessToken) ? GetAccessToken() : accessToken);
            apiContext.Config = GetConfig();

            // Use this variant if you want to pass in a request id  
            // that is meaningful in your application, ideally 
            // a order id.
            // String requestId = Long.toString(System.nanoTime();
            // APIContext apiContext = new APIContext(GetAccessToken(), requestId ));

            return apiContext;
            }
            catch (Exception ex)
            {

                clsExceptionLog clsex = new clsExceptionLog();
                clsex.LogException(ex, "Configuration", "GetAPIContext", HttpContext.Current.Request, HttpContext.Current.Session);
                return null;
            }
        }

    }
    //public static class Configuration
    //{
    //    // Create the configuration map that contains mode and other optional configuration details.
    //    private static Dictionary<string, string> GetConfig()
    //    {
    //        Dictionary<string, string> configMap = new Dictionary<string, string>();

    //        // Endpoints are varied depending on whether sandbox OR live is chosen for mode
    //         clsWebConfigsettings obj = new clsWebConfigsettings();
    //         if (obj.GetConfigSettingsValue("PaypalLiveMode").ToUpper() == "NO")
    //         {
    //             configMap.Add("mode", "sandbox");
    //         }
    //         else
    //         {
    //             configMap.Add("mode", "live");
    //         }

    //        // These values are defaulted in SDK. If you want to override default values, uncomment it and add your value
    //        // configMap.Add("connectionTimeout", "360000");
    //        // configMap.Add("requestRetries", "1");
    //        return configMap;
    //    }

    //    // Create accessToken
    //    private static string GetAccessToken()
    //    {
    //        // ###AccessToken
    //        // Retrieve the access token from
    //        // OAuthTokenCredential by passing in
    //        // ClientID and ClientSecret
    //        // It is not mandatory to generate Access Token on a per call basis.
    //        // Typically the access token can be generated once and
    //        // reused within the expiry window                
    //       // for vbvspl string accessToken = new OAuthTokenCredential("AZeG3xD7_dd3uSnMKw2mjsEoUyzjhraSeOzQnmoSX-Jcy40qI6PamWWRD1OI", "EIsdjRD_oGiDM0Mz0uB-urZwQtp0citr4zobBlUJocXZH3zzZvt21D5zxwgT", GetConfig()).GetAccessToken();
    //       // for urmuralikrishna string accessToken = new OAuthTokenCredential("AXqLtxBqsUbOzT5jcv44F7lWKQGgPLtIvf8tQ2hNF7QGNWs1IdKhrAnBAB7Q", "EA6lixD_NaqbGCk8oaWy5J1_5Jzwwaln1hD0wGztNqt8nJpf_RAh6N_0mHP3", GetConfig()).GetAccessToken();
    //        string clientid = null;
    //        string secretid = null;
    //        clsWebConfigsettings obj = new clsWebConfigsettings();
    //        if (obj.GetConfigSettingsValue("PaypalLiveMode").ToUpper() == "NO")
    //        {
    //            if (obj.GetConfigSettingsValue("PaypalRestAPITestClientid") != "")
    //            {
    //                clientid = obj.GetConfigSettingsValue("PaypalRestAPITestClientid");
    //            }
    //            if (obj.GetConfigSettingsValue("Paypalrestapitestsecretkey") != "")
    //            {
    //                secretid = obj.GetConfigSettingsValue("Paypalrestapitestsecretkey");
    //            }
    //        }
    //        else if (obj.GetConfigSettingsValue("PaypalLiveMode").ToUpper() == "YES")
    //        {
    //            if (obj.GetConfigSettingsValue("PaypalRestAPILiveClientid") != "")
    //            {
    //                clientid = obj.GetConfigSettingsValue("PaypalRestAPILiveClientid");
    //            }
    //            if (obj.GetConfigSettingsValue("PaypalrestapiLivesecretkey") != "")
    //            {
    //                secretid = obj.GetConfigSettingsValue("PaypalrestapiLivesecretkey");
    //            }
    //        }
    //        string accessToken = new OAuthTokenCredential(clientid, secretid, GetConfig()).GetAccessToken();
    //        return accessToken;
    //    }

    //    // Returns APIContext object
    //    public static APIContext GetAPIContext()
    //    {
    //        // ### Api Context
    //        // Pass in a `APIContext` object to authenticate 
    //        // the call and to send a unique request id 
    //        // (that ensures idempotency). The SDK generates
    //        // a request id if you do not pass one explicitly. 
    //        APIContext apiContext = new APIContext(GetAccessToken());
    //        apiContext.Config = GetConfig();

    //        // Use this variant if you want to pass in a request id  
    //        // that is meaningful in your application, ideally 
    //        // a order id.
    //        // String requestId = Long.toString(System.nanoTime();
    //        // APIContext apiContext = new APIContext(GetAccessToken(), requestId ));

    //        return apiContext;
    //    }

    //}
}
 