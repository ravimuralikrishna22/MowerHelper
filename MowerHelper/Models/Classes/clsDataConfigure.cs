using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Diagnostics;
using MowerHelper.Models.Classes;
using System.Web.UI.WebControls;
using MowerHelper.Models.DAL;
namespace MowerHelper.Models.Classes
{
    public class clsDataConfigure
    {
        int int_ReturnID;

        #region "GetConfigureDataset:Get the dataset"
        public DataSet GetConfigureDataset(string SpName)
        {
            clsCommonFunctions cls = new clsCommonFunctions();
            DataSet dataset = new DataSet();
            dataset = cls.GetDataSet(SpName);
            return dataset;
        }
        #endregion
        #region "GetConfigureDataReader:Get the datareader"
        public SqlDataReader GetConfigureDataReader(string SpName)
        {
            SqlDataReader objread;
            clsCommonFunctions objcommon = new clsCommonFunctions();
           
            objread=objcommon.GetDataReader(SpName);
            return objread;
        }
        #endregion
        #region "GetConfigureDataReader:Get the datareader"
        public SqlDataReader GetConfigureDataReader(string SpName, IDataParameter[] objParameter)
        {
            SqlDataReader objread;
            clsCommonFunctions objcommon = new clsCommonFunctions();
           objcommon.AddInParameters(objParameter);
           objread =objcommon.GetDataReader(SpName);
           return objread;
        }
        #endregion
        #region "GetConfigureDataset:Accepts SpName and IParameter,Gets the dataSet"
        public DataSet GetConfigureDataset(string SpName, IDataParameter[] objParameter)
        {
            clsCommonFunctions objcommon = new clsCommonFunctions();
            clsCommonFunctions cls = new clsCommonFunctions();
            DataSet dataset = new DataSet();
            objcommon.AddInParameters(objParameter);
            dataset=cls.GetDataSet(SpName);
            return dataset;
        }
        #endregion
        #region "AddSlNoToDataTable:Function which add serial no column to datatable first table and assign the unique values "
        public DataTable AddSlNoToDataTable(DataTable dtResult)
        {
            try
            {
                DataColumn dc_SlNo = new DataColumn();
                dc_SlNo.ColumnName = "SlNo";
                dtResult.Columns.Add(dc_SlNo);
                DataRow dr = null;
                int int_RowCount = 0;
                int_RowCount = 1;
                foreach (DataRow dr_loopVariable in dtResult.Select())
                {
                    dr = dr_loopVariable;
                    dr.BeginEdit();
                    dr["SlNo"] = int_RowCount;
                    dr.EndEdit();
                    int_RowCount += 1;
                }
                return dtResult;

            }
            catch (Exception ex)
            {
                throw;

            }
            finally
            {
            }
        }
        #endregion
        #region "CommandDB:CommandDB which gets data sp name and parameters"
        public void CommandDB(string SpName, IDataParameter[] InobjParameters)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                string sqlCommand = SpName;
                objcommon.AddInParameters(InobjParameters);
                objcommon.ExecuteNonQuerySP(SpName);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion
        #region "CommandDB:CommandDB Function which gets data sp name and parameters returns integer value"
        public int CommandDB(string SpName, IDataParameter[] InobjParameters, IDataParameter RetObjParm)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                string sqlCommand = SpName;
                objcommon.AddReturnParameters(RetObjParm);
                objcommon.AddInParameters(InobjParameters);
                object intretval = objcommon.ExecuteFunction(SpName);
                return Convert.ToInt32(intretval);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region "CommandDB:CommandDB Function which gets data sp name and parameters returns the out put msg and return value "
        public SqlCommand CommandDB(string SpName, IDataParameter[] InobjParameters, IDataParameter[] OutobjParameters, IDataParameter objRetParm)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                string sqlCommand = SpName;
               objcommon.AddReturnParameters(objRetParm);
               objcommon.AddInParameters(InobjParameters);
               objcommon.AddOutParameters(OutobjParameters);
               int_ReturnID = objcommon.ExecuteFunction(sqlCommand);
               return objcommon.GetCurrentCommand;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region "CommandDB:CommandDB Function which gets data sp name and parameters returns the out put msg "
        public SqlCommand CommandDB(string SpName, IDataParameter[] InobjParameters, IDataParameter[] OutobjParameters)
        {
            try
            {
                clsCommonFunctions objcommon = new clsCommonFunctions();
                string sqlCommand = SpName;
                objcommon.AddInParameters(InobjParameters);
                objcommon.AddOutParameters(OutobjParameters);
                objcommon.ExecuteNonQuerySP(SpName);
                return objcommon.GetCurrentCommand;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion



        

        

        

        

        


        
        

    }
}