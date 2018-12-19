using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace MowerHelper.Models.Classes
{
    public class Clshelp : clsDBWrapper
    {
        public string HelpText_ID
        {
            get;
            set;
        }
        public string HelpText
        {
            get;
            set;
        }
        public string AdminNote
        {
            get;
            set;
        }
        public SqlDataReader Get_DataReader(string spname, IDataParameter[] paramlist)
        {
            SqlDataReader drhelp = default(SqlDataReader);
            try
            {
                AddInParameters(paramlist);
                drhelp = GetDataReader(spname);
                return drhelp;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public object CommandDB(string spname, IDataParameter[] paramlist)
        {
            try
            {
                AddInParameters(paramlist);
                return ExecuteNonQuerySP(spname);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}