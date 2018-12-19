using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MowerHelper.Models.Classes;
namespace MowerHelper.Models.Classes
{
    public class clsDBWrapper
    {
        public Database objDB;
        public System.Data.Common.DbCommand objdbCommandWrapper;
        private IDataParameter[] _objInParameters;
        private IDataParameter[] _objOutParameters;
        private IDataParameter _objReturnParameters;
        private SqlCommand _objcurrentCommand;
        private string lvReturnParameterName;

        private enum ParameterType {   InParameter = 1, OutParameter = 2, ReturnParamter = 3 }

        public SqlConnection GetConnection
        {
            get { return (SqlConnection)objDB.CreateConnection(); }
        }
        public SqlCommand GetCurrentCommand
        {
            get { return _objcurrentCommand; }
           set
        {
            _objcurrentCommand = value;
        }

        }
        public clsDBWrapper(string InstanceName = "")
        {
            if (InstanceName == "")
            {
                objDB = DatabaseFactory.CreateDatabase();
            }
            else
            {
                objDB = DatabaseFactory.CreateDatabase(InstanceName);
            }
        }

        public void SetConnection(Database obj)
        {
            objDB = obj;
        }

        public void AddInParameters(IDataParameter[] parameters)
        {
            _objInParameters = parameters;
        }
        public void AddOutParameters(IDataParameter[] parameters)
        {
            _objOutParameters = parameters;
        }

        public void AddReturnParameters(IDataParameter parameters)
        {
            _objReturnParameters = parameters;
        }

        private System.Data.Common.DbCommand BuildQueryCommand(System.Data.Common.DbCommand objCommandWrapper, ParameterType EnumParameterType)
        {
            try
            {
               
                if (EnumParameterType == ParameterType.InParameter)
                {
                    if (_objInParameters != null)
                    {
                        foreach (SqlParameter objParameter in _objInParameters)
                        {
                            objDB.AddInParameter(objCommandWrapper, objParameter.ParameterName, objParameter.DbType, objParameter.Value);

                        }
                        _objInParameters = null;
                    }

                }
                if (EnumParameterType == ParameterType.OutParameter)
                {
                    if (_objOutParameters!= null)
                    {
                        foreach (SqlParameter objParameter in _objOutParameters)
                        {
                            objDB.AddOutParameter(objCommandWrapper, objParameter.ParameterName, objParameter.DbType, objParameter.Size);

                        }
                        _objOutParameters = null;
                    }

                }

                if (EnumParameterType == ParameterType.ReturnParamter)
                {
                    objDB.AddParameter(objCommandWrapper, _objReturnParameters.ParameterName,DbType.Int32, ParameterDirection.ReturnValue, null, DataRowVersion.Original, _objReturnParameters.Value);
                    lvReturnParameterName = _objReturnParameters.ParameterName;
                    _objReturnParameters = null;
                }
                GetCurrentCommand = (SqlCommand)(objCommandWrapper);
                objdbCommandWrapper = objCommandWrapper;
                return objCommandWrapper;
          

            }
            catch (Exception ex )
            {
                throw new System.Exception(ex.Message.ToString());
            }
        }

        public SqlDataReader GetDataReader(string storedProcName)
        {
            try
            {
                System.Data.Common.DbCommand dbCommandWrapper = objDB.GetStoredProcCommand(storedProcName);
                dbCommandWrapper.CommandTimeout = 180;
                if (_objInParameters != null)
                {
                    if (_objInParameters.Length > 0)
                    {
                        dbCommandWrapper = BuildQueryCommand(dbCommandWrapper, ParameterType.InParameter);
                    }
                }
                if (_objOutParameters != null)
                {
                    if (_objOutParameters.Length > 0)
                    {
                        dbCommandWrapper = BuildQueryCommand(dbCommandWrapper, ParameterType.OutParameter);
                    }
                }

                if (_objReturnParameters != null)
                {
                    dbCommandWrapper = BuildQueryCommand(dbCommandWrapper, ParameterType.ReturnParamter);
                }

                _objInParameters = null;
                _objOutParameters = null;
                _objReturnParameters = null;
                GetCurrentCommand =(SqlCommand) (dbCommandWrapper);
                return (SqlDataReader)((RefCountingDataReader)objDB.ExecuteReader(dbCommandWrapper)).InnerReader;
          
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message.ToString());
            }
        }
    
        public SqlCommand ExecuteCommandDB(string SpName, IDataParameter[] InobjParameters, IDataParameter[] OutobjParameters, IDataParameter objRetParm)
        {
            try
            {
                int in_ReturnID;
                if (objRetParm != null)
                {
                    AddReturnParameters(objRetParm);
                }
                AddInParameters(InobjParameters);
                AddOutParameters(OutobjParameters);
                in_ReturnID = ExecuteFunction(SpName);
                return GetCurrentCommand;

            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message.ToString());
            }
        }
        public int ExecuteFunction(string storedProcName)
        {
            try
            {
                SqlCommand objCmd;
                System.Data.Common.DbCommand dbCommandWrapper = objDB.GetStoredProcCommand(storedProcName);
                if (_objInParameters != null)
                {
                    if (_objInParameters.Length > 0)
                    {
                        dbCommandWrapper = BuildQueryCommand(dbCommandWrapper, ParameterType.InParameter);
                    }

                }
                if (_objOutParameters != null)
                {
                    if (_objOutParameters.Length > 0)
                    {
                        dbCommandWrapper = BuildQueryCommand(dbCommandWrapper, ParameterType.OutParameter);
                    }

                }
                if (_objReturnParameters != null)
                {
                    dbCommandWrapper = BuildQueryCommand(dbCommandWrapper, ParameterType.ReturnParamter);
                }
                objDB.ExecuteNonQuery(dbCommandWrapper);
                objCmd =(SqlCommand)dbCommandWrapper;
                _objInParameters = null;
                _objOutParameters = null;
                _objReturnParameters = null;
                GetCurrentCommand = (SqlCommand)(dbCommandWrapper);
                if (lvReturnParameterName != null)
                {
                    return (int)objCmd.Parameters[lvReturnParameterName].Value;
                }
                else
                {
                    return 0;
                }
          


            }
            catch(Exception ex)
            {
                throw new System.Exception(ex.Message.ToString());
            }
        }
        public DataSet GetDataSet(string storedProcName)
        {
            try
            {
                DataSet obDataset = null;
                System.Data.Common.DbCommand dbCommandWrapper = objDB.GetStoredProcCommand(storedProcName);
                dbCommandWrapper.CommandTimeout = 180;
                if (_objInParameters != null)
                {
                    if (_objInParameters.Length > 0)
                    {
                        dbCommandWrapper = BuildQueryCommand(dbCommandWrapper, ParameterType.InParameter);
                    }
                }

                if (_objOutParameters != null)
                {
                    if (_objOutParameters.Length > 0)
                    {
                        dbCommandWrapper = BuildQueryCommand(dbCommandWrapper, ParameterType.OutParameter);
                    }
                }
                if (_objReturnParameters != null)
                {
                    dbCommandWrapper = BuildQueryCommand(objdbCommandWrapper, ParameterType.ReturnParamter);
                }
                obDataset = objDB.ExecuteDataSet(dbCommandWrapper);
                _objInParameters = null;
                _objOutParameters = null;
                _objReturnParameters = null;
                GetCurrentCommand = (SqlCommand)(dbCommandWrapper);
                return obDataset;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public Int32 ExecuteNonQuerySP(string storedProcName)
        {
            try
            {
                System.Data.Common.DbCommand dbCommandWrapper = objDB.GetStoredProcCommand(storedProcName);
                if (_objInParameters != null)
                {
                    if (_objInParameters.Length > 0)
                    {
                        dbCommandWrapper = BuildQueryCommand(dbCommandWrapper, ParameterType.InParameter);
                    }
                }
                if (_objOutParameters != null)
                {
                    if (_objOutParameters.Length > 0)
                    {
                        dbCommandWrapper = BuildQueryCommand(dbCommandWrapper, ParameterType.OutParameter);
                    }
                }
                 GetCurrentCommand =(SqlCommand) (dbCommandWrapper);
            objdbCommandWrapper = dbCommandWrapper;
            return objDB.ExecuteNonQuery(dbCommandWrapper);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public SqlCommand ExecuteQuery(string SqlQuery)
        {
            try
            {
                SqlCommand objCmd;
                System.Data.Common.DbCommand dbCommandWrapper = objDB.GetSqlStringCommand(SqlQuery);
                if (_objInParameters != null)
                {
                    if (_objInParameters.Length > 0)
                    {
                        dbCommandWrapper = BuildQueryCommand(dbCommandWrapper, ParameterType.InParameter);
                    }
                }
                if (_objOutParameters != null)
                {
                    if (_objOutParameters.Length > 0)
                    {
                        dbCommandWrapper = BuildQueryCommand(dbCommandWrapper, ParameterType.OutParameter);
                    }
                }
                if (_objReturnParameters != null)
                {
                    dbCommandWrapper = BuildQueryCommand(dbCommandWrapper, ParameterType.ReturnParamter);
                }
                objDB.ExecuteNonQuery(dbCommandWrapper);
                objCmd = (SqlCommand)dbCommandWrapper;

                _objInParameters = null;
                _objOutParameters = null;
                _objReturnParameters = null;

                GetCurrentCommand = (SqlCommand)(dbCommandWrapper);
                return objCmd;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string ExecuteStringFunction(string storedProcName)
        {
            try
            {
                SqlCommand objCmd = default(SqlCommand);
                string str = "";

                System.Data.Common.DbCommand dbCommandWrapper = objDB.GetStoredProcCommand(storedProcName);
                if ((_objInParameters != null))
                {
                    if (_objInParameters.Length > 0)
                    {
                        dbCommandWrapper = BuildQueryCommand(dbCommandWrapper, ParameterType.InParameter);
                    }
                }

                if ((_objOutParameters != null))
                {
                    if (_objOutParameters.Length > 0)
                    {
                        dbCommandWrapper = BuildQueryCommand(dbCommandWrapper, ParameterType.OutParameter);
                    }
                }

                if ((_objReturnParameters != null))
                {
                    dbCommandWrapper = BuildQueryCommand(dbCommandWrapper, ParameterType.ReturnParamter);
                }
                objDB.ExecuteNonQuery(dbCommandWrapper);
                objCmd = (SqlCommand)dbCommandWrapper;

                _objInParameters = null;
                _objOutParameters = null;
                _objReturnParameters = null;

                GetCurrentCommand = (SqlCommand)(dbCommandWrapper);
                if (lvReturnParameterName != null)
                {
                    str=objCmd.Parameters[lvReturnParameterName].Value.ToString();
                }
                else
                {
                    str = null;
                }
                return str;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}