using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.VisualBasic;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;

namespace MowerHelper.Models.Classes
{
        

public class ClsControls : clsDBWrapper
{
	#region "Get_DataSet:Get the dataset"
	public DataSet Get_DataSet(string SpName)
	{
		return GetDataSet(SpName);
	}
	#endregion
	#region "Get_DataSet:Accepts SpName and IParameter,Gets the dataSet"
	public DataSet Get_DataSet(string SpName, IDataParameter[] objParameter)
	{
		AddInParameters(objParameter);
		return GetDataSet(SpName);
	}
	#endregion
    #region "Get_DataReader:Get the datareader"
	public SqlDataReader Get_DataReader(string SpName)
	{
		return GetDataReader(SpName);
	}
	#endregion
	#region "Get_DataReader:Get the datareader"
	public SqlDataReader Get_DataReader(string SpName, IDataParameter[] objParameter)
	{
		AddInParameters(objParameter);
		return GetDataReader(SpName);
	}
	#endregion
    #region "CommandDB:CommandDB which gets data sp name and parameters"
	public void CommandDB(string SpName, IDataParameter[] objParameters)
	{
		try {
			string sqlCommand = SpName;
			AddInParameters(objParameters);
			ExecuteNonQuerySP(SpName);
		} catch (Exception ex) {
			throw;
		}

	}
	#endregion
	#region "CommandDB:CommandDB Function which gets data sp name and parameters returns integer value"

    public int CommandDB(string SpName, IDataParameter[] objParameters, IDataParameter objRetParm)
    {
        try
        {
            string sqlCommand = SpName;
            AddReturnParameters(objRetParm);
            AddInParameters(objParameters);
            int intretval = ExecuteFunction(SpName);
            return intretval;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

	#endregion

}

}