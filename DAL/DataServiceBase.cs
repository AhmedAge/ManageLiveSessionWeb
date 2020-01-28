﻿//'/////////////////////////////////////////////////////////////////////////
//'<summary>
//'----------------------------------------------
//' Copyright 20113 Inegrated Business Solution 
//' www.isb-me.com All Rights Reserved.
//'----------------------------------------------
//' Comment: Defines common DataService routines for transaction management,stored procedure execution, parameter creation, and null value checking
//' Authors: Malik Hourani
//' Reviewers:
//'</summary>
//'----------------------------------------------  
//'/////////////////////////////////////////////////////////////////////////
#region "References"
using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
#endregion
namespace ManageLiveSessionWeb.DAL
{
    public  class DataServiceBase
    {
        #region "Fields"
        //True if service owns the transaction  
        private bool _isOwner = false;
        private OracleTransaction _txn;
        public static string _ConnectioString = "data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=vlsdbscan-sc)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=VLSDB)));user id=KFUBAN;password=kfuban01;pooling=true;Max Pool Size=10000;Connection Timeout=100;";


        #endregion
        #region "Properties"
        //Reference to the current transaction
        public OracleTransaction Txn
        {
            get { return (OracleTransaction)_txn; }
            set { _txn = (OracleTransaction)value; }
        }
        #endregion
        #region "Constructors"
        public DataServiceBase()
            : this(null)
        {
        }
        public DataServiceBase(OracleTransaction txn)
            : base()
        {
            if ((txn == null))
            {
                _isOwner = true;
            }
            else
            {
                _txn = (OracleTransaction)txn;
                _isOwner = false;
            }
        }
        #endregion
        #region "Methods"
        // Connection and Transaction Methods
        protected static string GetConnectionString()
        {
            return _ConnectioString;
        }
        public static OracleTransaction BeginTransaction()
        {
            OracleConnection txnConnection = new OracleConnection(GetConnectionString());
            txnConnection.Open();
            return txnConnection.BeginTransaction();
        }
        // ExecuteDataSet Methods
        protected DataSet ExecuteDataSet(string procName, params IDataParameter[] procParams)
        {
            OracleCommand cmd = null;
            return ExecuteDataSet(ref cmd, procName, procParams);

        }

        protected DataSet ExecuteSelectCommandParamList(string commandText, List<OracleParameter> procParams)
        {
            OracleConnection cnx = null;
            DataSet ds = new DataSet();
            OracleDataAdapter da = new OracleDataAdapter();
            OracleCommand cmd = null;
            try
            {
                //Setup command object
                cmd = new OracleCommand(commandText);
                cmd.CommandType = CommandType.Text;
                if ((((procParams) != null)))
                {
                    int index = 0;
                    while ((index < procParams.Count))
                    {
                        cmd.Parameters.Add(procParams[index]);
                        index = (index + 1);
                    }
                }

                da.SelectCommand = (OracleCommand)cmd;


                //Determine the transaction owner and process accordingly
                if (_isOwner)
                {
                    cnx = new OracleConnection(GetConnectionString());
                    cmd.Connection = cnx;
                    cnx.Open();
                }
                else
                {
                    cmd.Connection = _txn.Connection;
                    cmd.Transaction = _txn;
                }
                //Fill the dataset

                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
            finally
            {
                if ((((da) != null)))
                {
                    da.Dispose();
                }
                if ((((cmd) != null)))
                { 
                    cmd.Dispose();  
                }
                if (_isOwner)
                {
                    cnx.Dispose();
                    cnx.Close();
                    //Implicitly calls cnx.Close()
                }
            }
            return ds;

        }

        protected DataSet ExecuteSelectCommand(string commandText, params IDataParameter[] procParams)
        {
            OracleConnection cnx = null;
            DataSet ds = new DataSet();
            OracleDataAdapter da = new OracleDataAdapter();
            OracleCommand cmd = null;
            try
            {
                //Setup command object
                cmd = new OracleCommand(commandText);
                cmd.CommandType = CommandType.Text;
                if ((((procParams) != null)))
                {
                    int index = 0;
                    while ((index < procParams.Length))
                    {
                        cmd.Parameters.Add(procParams[index]);
                        index = (index + 1);
                    }
                }

                da.SelectCommand = (OracleCommand)cmd;


                //Determine the transaction owner and process accordingly
                if (_isOwner)
                {
                    cnx = new OracleConnection(GetConnectionString());
                    cmd.Connection = cnx;
                    cnx.Open();
                }
                else
                {
                    cmd.Connection = _txn.Connection;
                    cmd.Transaction = _txn;
                }
                //Fill the dataset

                try
                {
                    da.Fill(ds);
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
            finally
            {
                if ((((da) != null)))
                {
                    da.Dispose();
                }
                if ((((cmd) != null)))
                {

                    cmd.Dispose();
                    cmd.Parameters.AddRange(procParams); // Added from malik hourani to put the parameters
                }
                if (_isOwner)
                {
                    cnx.Dispose();
                    cnx.Close();
                    //Implicitly calls cnx.Close()
                }
            }
            return ds;

        }
        protected DataSet ExecuteDataSet(ref OracleCommand cmd, string procName, params IDataParameter[] procParams)
        {
            OracleConnection cnx = null;
            DataSet ds = new DataSet();
            OracleDataAdapter da = new OracleDataAdapter();
            cmd = null;
            try
            {
                //Setup command object
                cmd = new OracleCommand(procName);
                cmd.CommandType = CommandType.StoredProcedure;
                if ((((procParams) != null)))
                {
                    int index = 0;
                    while ((index < procParams.Length))
                    {
                        cmd.Parameters.Add(procParams[index]);
                        index = (index + 1);
                    }
                }

                da.SelectCommand = (OracleCommand)cmd;

               
                //Determine the transaction owner and process accordingly
                if (_isOwner)
                {
                    cnx = new OracleConnection(GetConnectionString());
                    cmd.Connection = cnx;
                    cnx.Open();
                }
                else
                {
                    cmd.Connection = _txn.Connection;
                    cmd.Transaction = _txn;
                }
                //Fill the dataset

                try
                {
                    da.Fill(ds);
                }
                catch {
                
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
            finally
            {
                if ((((da) != null)))
                {
                    da.Dispose();
                }
                if ((((cmd) != null)))
                {
                    
                    cmd.Dispose();
                    cmd.Parameters.AddRange(procParams); // Added from malik hourani to put the parameters
                }
                if (_isOwner)
                {
                    cnx.Dispose();
                    cnx.Close();
                    //Implicitly calls cnx.Close()
                }
            }
            return ds;
        }

        protected int ExecuteNonQueryText(string commandText, params IDataParameter[] procParams)
        {
            //Method variables
            OracleConnection cnx = null;
            OracleCommand cmd = null;
            //Avoids "Use of unassigned variable" compiler error
            try
            {
                //Setup command object
                cmd = new OracleCommand(commandText);
                cmd.CommandType = CommandType.Text;
                int index = 0;
                while ((index < procParams.Length))
                {
                    cmd.Parameters.Add(procParams[index]);
                    index = (index + 1);
                }
                //Determine the transaction owner and process accordingly
                if (_isOwner)
                {
                    cnx = new OracleConnection(GetConnectionString());
                    cmd.Connection = cnx;
                    cnx.Open();
                }
                else
                {
                    cmd.Connection = _txn.Connection;
                    cmd.Transaction = _txn;
                }
                //Execute the command
               return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (_isOwner)
                {
                    cnx.Dispose();
                    cnx.Close();
                    //Implicitly calls cnx.Close()
                }
                if ((((cmd) != null)))
                {
                    cmd.Dispose();
                    cmd.Parameters.AddRange(procParams);
                }
            }
        }
        protected int ExecuteNonQueryText(string commandText, List<OracleParameter> procParams)
        {
            //Method variables
            OracleConnection cnx = null;
            OracleCommand cmd = null;
            //Avoids "Use of unassigned variable" compiler error
            try
            {
                //Setup command object
                cmd = new OracleCommand(commandText);
                cmd.CommandType = CommandType.Text;
                int index = 0;
                while ((index < procParams.Count))
                {
                    cmd.Parameters.Add(procParams[index]);
                    index = (index + 1);
                }
                //Determine the transaction owner and process accordingly
                if (_isOwner)
                {
                    cnx = new OracleConnection(GetConnectionString());
                    cmd.Connection = cnx;
                    cnx.Open();
                }
                else
                {
                    cmd.Connection = _txn.Connection;
                    cmd.Transaction = _txn;
                }
                //Execute the command
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (_isOwner)
                {
                    cnx.Dispose();
                    cnx.Close();
                    //Implicitly calls cnx.Close()
                }
                if ((((cmd) != null)))
                {
                    cmd.Dispose();
                   // cmd.Parameters.AddRange(procParams);
                }
            }
        }
        // ExecuteNonQuery Methods
        protected void ExecuteNonQuery(string procName, params IDataParameter[] procParams)
        {
            OracleCommand cmd = null;
            ExecuteNonQuery(ref cmd, procName, procParams);
        }
        protected void ExecuteNonQuery(ref OracleCommand cmd, string procName, params IDataParameter[] procParams)
        {
            //Method variables
            OracleConnection cnx = null;
            cmd = null;
            //Avoids "Use of unassigned variable" compiler error
            try
            {
                //Setup command object
                cmd = new OracleCommand(procName);
                cmd.CommandType = CommandType.StoredProcedure;
                int index = 0;
                while ((index < procParams.Length))
                {
                    cmd.Parameters.Add(procParams[index]);
                    index = (index + 1);
                }
                //Determine the transaction owner and process accordingly
                if (_isOwner)
                {
                    cnx = new OracleConnection(GetConnectionString());
                    cmd.Connection = cnx;
                    cnx.Open();
                }
                else
                {
                    cmd.Connection = _txn.Connection;
                    cmd.Transaction = _txn;
                }
                //Execute the command
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (_isOwner)
                {
                    cnx.Dispose();
                    cnx.Close();
                    //Implicitly calls cnx.Close()
                }
                if ((((cmd) != null)))
                {
                    cmd.Dispose();
                    cmd.Parameters.AddRange(procParams);
                }
            }
        }
        protected void ExecuteNonQuery(ref OracleCommand cmd, string procName, int optionalClose, params IDataParameter[] procParams)
        {
            //Method variables
            OracleConnection cnx = null;
            cmd = null;
            //Avoids "Use of unassigned variable" compiler error
            try
            {
                //Setup command object
                cmd = new OracleCommand(procName);
                cmd.CommandType = CommandType.StoredProcedure;
                int index = 0;
                while ((index < procParams.Length))
                {
                    cmd.Parameters.Add(procParams[index]);
                    index = (index + 1);
                }
                //Determine the transaction owner and process accordingly
                if (_isOwner)
                {
                    cnx = new OracleConnection(GetConnectionString());
                    cmd.Connection = cnx;
                    cnx.Open();
                }
                else
                {
                    cmd.Connection = _txn.Connection;
                    cmd.Transaction = _txn;
                }
                //Execute the command
                cmd.ExecuteNonQuery();
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (_isOwner)
                {
                    if (optionalClose > 0)
                    {
                        cnx.Dispose();
                        cnx.Close();
                        //Implicitly calls cnx.Close()
                    }
                }
                if ((((cmd) != null)))
                {
                    if (optionalClose > 0)
                    {
                        cmd.Dispose();
                    }
                    cmd.Parameters.AddRange(procParams);
                }
            }
        }
        // CreateParameter Methods
        protected OracleParameter CreateParameter(string paramName, OracleDbType paramType, object paramValue)
        {
            OracleParameter param = new OracleParameter(paramName, paramType);
            //If (paramValue <> DBNull.Value) Then
            if ((!object.ReferenceEquals(paramValue, DBNull.Value)))
            {
                switch ((paramType))
                {
                    case OracleDbType.Varchar2:
                    case OracleDbType.Char:
                        paramValue = CheckParamValue(Convert.ToString(paramValue));
                        break;
                    case OracleDbType.Date:
                        paramValue = CheckParamValue((DateTime)paramValue);
                        break;
                    case OracleDbType.Int32:
                        paramValue = CheckParamValue(Convert.ToInt32(paramValue));
                        break;
                    case OracleDbType.Decimal:
                        paramValue = CheckParamValue(Convert.ToDecimal(paramValue));
                        break;
                }
                //TODO: Warning!!!, inline IF is not supported ?
            }
            param.Value = paramValue;
            return param;
        }
        protected OracleParameter CreateParameter(string paramName, OracleDbType paramType, ParameterDirection direction)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, DBNull.Value);
            returnVal.Direction = direction;
            return returnVal;
        }
        protected OracleParameter CreateParameter(string paramName, OracleDbType paramType, object paramValue, ParameterDirection direction)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Direction = direction;
            return returnVal;
        }
        protected OracleParameter CreateParameter(string paramName, OracleDbType paramType, object paramValue, int size)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Size = size;
            return returnVal;
        }
        protected OracleParameter CreateParameter(string paramName, OracleDbType paramType, object paramValue, int size, ParameterDirection direction)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Direction = direction;
            returnVal.Size = size;
            return returnVal;
        }
        protected OracleParameter CreateParameter(string paramName, OracleDbType paramType, object paramValue, int size, byte precision)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Size = size;
            ((OracleParameter)returnVal).Precision = precision;
            return returnVal;
        }
        protected OracleParameter CreateParameter(string paramName, OracleDbType paramType, object paramValue, int size, byte precision, ParameterDirection direction)
        {
            OracleParameter returnVal = CreateParameter(paramName, paramType, paramValue);
            returnVal.Direction = direction;
            returnVal.Size = size;
            returnVal.Precision = precision;
            return returnVal;
        }
        // CheckParamValue Methods
        protected Guid GetGuid(object value)
        {
            Guid returnVal = Constants.NullGuid;
            if ((value is string))
            {
                returnVal = new Guid(Convert.ToString(value));
            }
            else if ((value is Guid))
            {
                returnVal = (Guid)value;
            }
            return returnVal;
        }
        protected object CheckParamValue(string paramValue)
        {
            if (string.IsNullOrEmpty(paramValue))
            {
                return DBNull.Value;
            }
            else
            {
                return paramValue;
            }
        }
        protected object CheckParamValue(Guid paramValue)
        {
            if (paramValue.Equals(Constants.NullGuid))
            {
                return DBNull.Value;
            }
            else
            {
                return paramValue;
            }
        }
        protected object CheckParamValue(DateTime paramValue)
        {
            if (paramValue.Equals(Constants.NullDateTime))
            {
                return DBNull.Value;
            }
            else
            {
                return paramValue;
            }
        }
        protected object CheckParamValue(double paramValue)
        {
            if (paramValue.Equals(Constants.NullDouble))
            {
                return DBNull.Value;
            }
            else
            {
                return paramValue;
            }
        }
        protected object CheckParamValue(float paramValue)
        {
            if (paramValue.Equals(Constants.NullFloat))
            {
                return DBNull.Value;
            }
            else
            {
                return paramValue;
            }
        }
        protected object CheckParamValue(decimal paramValue)
        {
            if (paramValue.Equals(Constants.NullDecimal))
            {
                return DBNull.Value;
            }
            else
            {
                return paramValue;
            }
        }
        protected object CheckParamValue(int paramValue)
        {
            if (paramValue.Equals(Constants.NullInt))
            {
                return DBNull.Value;
            }
            else
            {
                return paramValue;
            }
        }
        #endregion
    }
}
