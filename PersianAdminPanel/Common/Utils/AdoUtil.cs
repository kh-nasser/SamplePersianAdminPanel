using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Common.Utils
{
    public sealed class AdoUtil
    {
        private static readonly Lazy<AdoUtil> lazy = new Lazy<AdoUtil>(() => new AdoUtil());

        public static AdoUtil Instance { get { return lazy.Value; } }

        private AdoUtil()
        {
        }

        public enum WhereJoinWith
        {
            OR = 1,
            AND = 2
        };

        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(GetConnectionString());
        }

        private string GetConnectionString()
        {
            string strCon = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
            return strCon;
        }

        /// <summary>
        /// Execute Sql Command
        /// </summary>
        /// <param name="SQLCommand">SQL Command String</param>
        /// <returns>True In Successfull Else Throw Exeption</returns>
        public bool ExecuteCommand(string SQLCommand)
        {
            var MyConnection = GetSqlConnection();

            try
            {
                var Comm = new SqlCommand(SQLCommand, MyConnection);
                MyConnection.Open();
                Comm.ExecuteNonQuery();
                Comm.Dispose();
                MyConnection.Close();

                return true;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                MyConnection.Close();
            }
        }

        /// <summary>
        /// Insert One Record To Table
        /// </summary>
        /// <param name="tableName">Table Name To Insert</param>
        /// <param name="fields">Fields Name, Type: Array String</param>
        /// <param name="values">Values To Insert, Type: Array Object</param>
        /// <returns>Return Inserted ID In Successfull Insert Else Return Exeption</returns>
        public long Insert(string tableName, string[] fields, object[] values)
        {
            var MyConnection = GetSqlConnection();

            try
            {
                string strFields = "";
                string strParams = "";

                var Comm = new SqlCommand();
                Comm.Connection = MyConnection;
                Comm.Parameters.Clear();

                string SQLCommand = "Insert Into " + tableName;

                foreach (string Field in fields)
                {
                    strFields += Field + ",";
                    strParams += "@Param" + Field + ",";
                }

                strFields = strFields.Substring(0, strFields.Length - 1);
                strParams = strParams.Substring(0, strParams.Length - 1);

                SQLCommand += " (" + strFields + ") ";
                SQLCommand += "Values (" + strParams + "); ";

                SQLCommand += @"declare @newId bigint ;
							SET @newId = (SELECT NEWID = SCOPE_IDENTITY());		
							SELECT  @newId;";

                Comm.CommandText = SQLCommand;

                string[] Params = strParams.Split(',');

                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] == null)
                    {
                        Comm.Parameters.AddWithValue(Params[i], DBNull.Value);
                    }
                    else
                    {
                        Comm.Parameters.AddWithValue(Params[i], values[i]);
                    }
                }

                MyConnection.Open();
                long InsertedID = Int64.Parse(Comm.ExecuteScalar().ToString());
                Comm.Dispose();
                MyConnection.Close();

                return InsertedID;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                MyConnection.Close();
            }
        }

        /// <summary>
        /// Update Records In Table
        /// </summary>
        /// <param name="tableName">Table Name To Update</param>
        /// <param name="fields">Fields Name, Type: Array String</param>
        /// <param name="values">Values To Insert, Type: Array Object</param>
        /// <returns>Return Boolean True In Successfull Insert Else Return Exeption</returns>
        public bool Update(string tableName, string[] fields, object[] values, string[] WhereFields, object[] WhereValues)
        {
            var MyConnection = GetSqlConnection();

            try
            {
                string SQLCommand = "";

                SqlCommand Comm = new SqlCommand();
                Comm.Connection = MyConnection;
                Comm.Parameters.Clear();

                SQLCommand = "Update " + tableName + " SET ";

                foreach (string Field in fields)
                {
                    SQLCommand += Field + " = " + "@Param" + Field + ",";
                }

                SQLCommand = SQLCommand.Substring(0, SQLCommand.Length - 1);

                SQLCommand += " WHERE ";

                foreach (string WhereField in WhereFields)
                {
                    SQLCommand += WhereField + " = " + "@WhereParam" + WhereField + ",";
                }

                SQLCommand = SQLCommand.Substring(0, SQLCommand.Length - 1);

                Comm.CommandText = SQLCommand;

                for (int i = 0; i < fields.Length; i++)
                {
                    //Comm.Parameters.AddWithValue("@Param" + Fields[i], Values[i]);

                    if (values[i] == null)
                    {
                        Comm.Parameters.AddWithValue("@Param" + fields[i], DBNull.Value);
                    }
                    else
                    {
                        Comm.Parameters.AddWithValue("@Param" + fields[i], values[i]);
                    }
                }

                for (int i = 0; i < WhereFields.Length; i++)
                {
                    Comm.Parameters.AddWithValue("@WhereParam" + WhereFields[i], WhereValues[i]);
                }

                MyConnection.Open();
                int RowCount = Comm.ExecuteNonQuery();
                Comm.Dispose();
                MyConnection.Close();

                return (RowCount > 0 ? true : false);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                MyConnection.Close();
            }
        }

        /// <summary>
        /// Delete Row(s) From Table
        /// </summary>
        /// <param name="TableName">TableName In DB</param>
        /// <param name="WhereFields">WhereFields For Delete</param>
        /// <param name="WhereValues">WhereValues</param>
        /// <returns></returns>
        public bool Delete(string TableName, string[] WhereFields, object[] WhereValues)
        {
            var MyConnection = GetSqlConnection();

            try
            {
                var Comm = new SqlCommand();
                Comm.Connection = MyConnection;
                Comm.Parameters.Clear();

                string SQLCommand = "Delete From " + TableName + " WHERE ";

                foreach (string WhereField in WhereFields)
                {
                    SQLCommand += WhereField + " = " + "@WhereParam" + WhereField + ",";
                }

                SQLCommand = SQLCommand.Substring(0, SQLCommand.Length - 1);

                Comm.CommandText = SQLCommand;

                for (int i = 0; i < WhereFields.Length; i++)
                {
                    Comm.Parameters.AddWithValue("@WhereParam" + WhereFields[i], WhereValues[i]);
                }

                MyConnection.Open();
                int RowCount = Comm.ExecuteNonQuery();
                Comm.Dispose();
                MyConnection.Close();

                return (RowCount > 0 ? true : false);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                MyConnection.Close();
            }
        }

        /// <summary>
        /// Get Data From Table
        /// </summary>
        /// <param name="SelectCommand">SQL Select Command String</param>
        /// <returns>Return Data In DataTable Type</returns>
        public DataTable GetData(string SelectCommand)
        {
            var MyConnection = GetSqlConnection();

            try
            {
                var dtTable = new DataTable();

                if (MyConnection.State == ConnectionState.Closed)
                    MyConnection.Open();

                var dtAdapter = new SqlDataAdapter(SelectCommand, MyConnection);
                dtAdapter.Fill(dtTable);
                dtAdapter.Dispose();

                MyConnection.Close();

                return dtTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MyConnection.Close();
            }
        }

        /// <summary>
        /// Get Data From Table
        /// </summary>
        /// <param name="SelectCommand">SQL Select Command String</param>
        /// <param name="WhereFields">If Command Has Where Insert Where Field</param>
        /// <param name="WhereValue">Value For Where Fieds</param>
        /// /// <param name="WhereJoinWithType">Value For Where Fieds</param>
        /// <returns>Return Data In DataTable Type</returns>
        public DataTable GetData(string SelectCommand, string[] WhereFields, object[] WhereValue, WhereJoinWith WhereJoinWithType)
        {
            var MyConnection = GetSqlConnection();

            try
            {
                string strParam = "";
                string strWhere = "";

                SqlCommand Comm = new SqlCommand();
                Comm.Connection = MyConnection;
                Comm.Parameters.Clear();

                if (SelectCommand.IndexOf("WHERE") < 0)
                {
                    strWhere += " WHERE ";
                }
                else
                {
                    strWhere = " AND ";
                }

                for (int i = 0; i < WhereFields.Length; i++)
                {
                    strParam = "@Param" + WhereFields[i];
                    strWhere += WhereFields[i] + " = " + strParam;

                    if (i < WhereFields.Length - 1)
                        strWhere += " " + WhereJoinWithType.ToString() + " ";

                    Comm.Parameters.AddWithValue(strParam, WhereValue[i]);
                }

                Comm.CommandText = SelectCommand + strWhere;

                DataTable dtTable = new DataTable();

                MyConnection.Open();

                SqlDataAdapter dtAdapter = new SqlDataAdapter(Comm);
                dtAdapter.Fill(dtTable);
                dtAdapter.Dispose();

                return dtTable;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                MyConnection.Close();
            }
        }

        /// <summary>
        /// Search For Has Row In Table
        /// </summary>
        /// <param name="selectCommand">Select Command String</param>
        /// <returns>Is Successfull Execute Command Return True Or False Else Throw Exeption</returns>
        public bool HasRow(string selectCommand)
        {
            var MyConnection = GetSqlConnection();

            try
            {
                if (MyConnection.State == ConnectionState.Closed)
                    MyConnection.Open();

                var Comm = new SqlCommand(selectCommand, MyConnection);
                SqlDataReader dtReader = Comm.ExecuteReader();
                return dtReader.HasRows;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                MyConnection.Close();
            }
        }

        public long GetBackId(string field, string table, long maxField)
        {
            var MyConnection = GetSqlConnection();

            var Comm = new SqlCommand("Select Max(" + field + ") as Coe from " + table + " where " + field + " < " + maxField, MyConnection);
            MyConnection.Open();
            try
            {
                SqlDataReader rdr = Comm.ExecuteReader();
                if (rdr.Read())
                {
                    return Convert.ToInt32(rdr[0]);
                }
            }
            catch (Exception)
            {
                return 1;
            }
            finally
            {
                Comm.Dispose();
                MyConnection.Close();
            }
            return 0;
        }

        public long GetNextId(string field, string table, long maxField)
        {
            var MyConnection = GetSqlConnection();

            var comm = new SqlCommand("Select Min(" + field + ") as Code from " + table + " where " + field + " > " + maxField, MyConnection);
            MyConnection.Open();
            try
            {
                SqlDataReader rdr = comm.ExecuteReader();
                if (rdr.Read())
                {
                    return Convert.ToInt32(rdr[0]);
                }
            }
            catch (Exception)
            {
                return 1;
            }
            finally
            {
                comm.Dispose();
                MyConnection.Close();
            }
            return 0;
        }
    }
}