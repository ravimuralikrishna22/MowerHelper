using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Collections;
namespace MowerHelper.Models.Classes
{
    public static class clsWebConfigsettings
    {
        public static void SaveConfigSettings()
        {
            try
            {
                SqlDataReader oDataReader;
                clsCommonFunctions objclscommon = new clsCommonFunctions();
                IDataParameter[] Paramlist = { new SqlParameter("@In_Parameter_ID", null) };
                objclscommon.AddInParameters(Paramlist);
                oDataReader = objclscommon.GetDataReader("Help_dbo.st_Application_GET_WebConfigSettings_Cache");
                Hashtable ohtConfigSettings = new Hashtable();
                while (oDataReader.Read())
                {
                    ohtConfigSettings.Add(oDataReader.GetValue(oDataReader.GetOrdinal("Parameter_Key")), oDataReader.GetValue(oDataReader.GetOrdinal("Parameter_Value")));
                }
                ICacheManager ConfigCacheManager = CacheFactory.GetCacheManager();
                if (ConfigCacheManager.Contains("Webconfigsettings"))
                {
                    ConfigCacheManager.Remove("Webconfigsettings");
                }
                ConfigCacheManager.Add("Webconfigsettings", ohtConfigSettings);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetConfigSettingsValue(string key)
        {
            ICacheManager ConfigCacheManager = CacheFactory.GetCacheManager();
            if (ConfigCacheManager.Contains("Webconfigsettings"))
            {
                try
                {
                    Hashtable objHashTable = (Hashtable)ConfigCacheManager.GetData("Webconfigsettings");
                    if (objHashTable.Contains(key))
                    {
                        if (objHashTable[key] != null)
                        {
                            return (string)objHashTable[key];
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                SaveConfigSettings();
                return GetConfigSettingsValue(key);
            }
        }
    }
}