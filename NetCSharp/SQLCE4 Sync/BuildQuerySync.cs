using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Linq;
using System.Data;

namespace PEG.Data
{
    public class BuildQuerySync
    {

        /// <summary>
        /// Builds the update query.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="connection">The connection.</param>
        /// <returns>update query string</returns>
        /// <remarks>Builds update query for Sync service</remarks>
        public string BuildUpdateQuery(string tableName,string connection)
        {
            var theString = "Update " + tableName + " set ";
            using (var conn = new SqlCeConnection(connection))
            {
                conn.Open();
                var query = "SELECT * FROM information_schema.columns c " +
                           " WHERE c.TABLE_NAME = '" + tableName + "'   " +
                           " order by ordinal_position";

                using (var _sqlCeDataAdapter = new SqlCeDataAdapter(query, conn)) {
                    var _ds = new DataSet();
                    _sqlCeDataAdapter.Fill(_ds, "builder");
                    int i = 1;
                    foreach (DataRow row in _ds.Tables[0].Rows)
                    {
                        if(i != 1)
                        theString = theString + " [" + row[3] + "] = @"+row[3];
                        if (i < _ds.Tables[0].Rows.Count && i != 1)
                            theString = theString + " , ";
                        i++;
                    }
                    theString = theString + " where " + _ds.Tables[0].Rows[0][3] + " = @" + _ds.Tables[0].Rows[0][3];
                }
                conn.Close();
            }
            return theString;
        }
        /// <summary>
        /// Builds the parameters.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="connection">The connection.</param>
        /// <returns>list of strings for building the parameters</returns>
        /// <remarks>Porbably cant use this very much</remarks>
        public List<string> BuildParameters(string tableName, string connection)
        {
            var theString = "Update " + tableName + " set ";
            List<string> result = null;
            using (var conn = new SqlCeConnection(connection))
            {
                conn.Open();
                var query = "SELECT * FROM information_schema.columns c " +
                            " WHERE c.TABLE_NAME = '" + tableName + "'   " +
                            " order by ordinal_position";
                using (var _sqlCeDataAdapter = new SqlCeDataAdapter(query, conn))
                {
                    var _ds = new DataSet();
                    _sqlCeDataAdapter.Fill(_ds, "builder");
                    result = (from DataRow row in _ds.Tables[0].Rows select "@" + row[3]).ToList();
                }
                conn.Close();
            }
            return result;
            
        }
        /// <summary>
        /// Builds the insert query.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="connection">The connection.</param>
        /// <returns>Insert Query - string</returns>
        /// <remarks>Builds insert query for sync service</remarks>
        public string BuildInsertQuery(string tableName, string connection)
        {
            var theString = "";
            using (var conn = new SqlCeConnection(connection))
            {
                conn.Open();
                theString = "INSERT INTO " + tableName + " (";
                var query = "SELECT * FROM information_schema.columns c " +
                            " WHERE c.TABLE_NAME = '" + tableName + "'   " +
                            " order by ordinal_position";
                using (var _sqlCeDataAdapter = new SqlCeDataAdapter(query, conn))
                {
                    var _ds = new DataSet();
                    _sqlCeDataAdapter.Fill(_ds, "builder");
                    int i = 1;
                    foreach (DataRow row in _ds.Tables[0].Rows)
                    {
                        theString = theString + " [" + row[3] + "] ";
                        if (i < _ds.Tables[0].Rows.Count)
                            theString = theString + " , ";
                        i++;
                    }
                    theString = theString + " ) Values ( ";
                    i = 1;
                    foreach (DataRow row in _ds.Tables[0].Rows)
                    {
                        theString = theString + " @" + row[3];
                        if (i < _ds.Tables[0].Rows.Count)
                            theString = theString + " , ";
                        i++;
                    }
                    theString = theString + " )";
                }
                conn.Close();
            }
            return theString;
        }
        //
        //
        //
        /// <summary>
        /// Builds the update query.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="connection">The connection.</param>
        /// <returns>update query string</returns>
        /// <remarks>Builds update query for Sync service</remarks>
        public string BuildFullSqlUpdateQuery(string tableName, string connection)
        {
            var theString = "Update " + tableName + " set ";

            using (var conn = new SqlConnection(connection))
            {
                conn.Open();
                var query = "SELECT * FROM information_schema.columns c " +
                            " WHERE c.TABLE_NAME = '" + tableName + "'   " +
                            " order by ordinal_position";
                using (var _sqlDataAdapter = new SqlDataAdapter(query, conn))
                {
                   var  _ds = new DataSet();
                    _sqlDataAdapter.Fill(_ds, "builder");
                    int i = 1;
                    foreach (DataRow row in _ds.Tables[0].Rows)
                    {
                        if (i != 1)
                            theString = theString + " [" + row[3] + "] = @" + row[3];
                        if (i < _ds.Tables[0].Rows.Count && i != 1)
                            theString = theString + " , ";
                        i++;
                    }
                    theString = theString + " where " + _ds.Tables[0].Rows[0][3] + " = @" + _ds.Tables[0].Rows[0][3];
                }
                conn.Close();
            }
            return theString;
        }

        /// <summary>
        /// Builds the parameters.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="connection">The connection.</param>
        /// <returns>list of strings for building the parameters</returns>
        /// <remarks>Porbably cant use this very much</remarks>
        public List<string> BuildFullSqlParameters(string tableName, string connection)
        {
            List<string> result = null;
            using (var conn = new SqlConnection(connection))
            {
                conn.Open();
                var query = "SELECT * FROM information_schema.columns c " +
                            " WHERE c.TABLE_NAME = '" + tableName + "'   " +
                            " order by ordinal_position";
                using (var _sqlDataAdapter = new SqlDataAdapter(query, conn))
                {
                    var _ds = new DataSet();
                    _sqlDataAdapter.Fill(_ds, "builder");
                    result = (from DataRow row in _ds.Tables[0].Rows select "@" + row[3]).ToList();
                }
                conn.Close();
            }
            return result;

        }
        /// <summary>
        /// Builds the insert query.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="connection">The connection.</param>
        /// <returns>Insert Query - string</returns>
        /// <remarks>Builds insert query for sync service</remarks>
        public string BuildFullSqlInsertQuery(string tableName, string connection)
        {
            var theString = "";
            using (var conn = new SqlConnection(connection))
            {
                conn.Open();
                theString = "INSERT INTO " + tableName + " (";
                var query = "SELECT * FROM information_schema.columns c " +
                            " WHERE c.TABLE_NAME = '" + tableName + "'   " +
                            " order by ordinal_position";
                using (var _sqlDataAdapter = new SqlDataAdapter(query, conn))
                {
                    var _ds = new DataSet();
                    _sqlDataAdapter.Fill(_ds, "builder");
                    int i = 1;
                    foreach (DataRow row in _ds.Tables[0].Rows)
                    {
                        theString = theString + " [" + row[3] + "] ";
                        if (i < _ds.Tables[0].Rows.Count)
                            theString = theString + " , ";
                        i++;
                    }
                    theString = theString + " ) Values ( ";
                    i = 1;
                    foreach (DataRow row in _ds.Tables[0].Rows)
                    {
                        theString = theString + " @" + row[3];
                        if (i < _ds.Tables[0].Rows.Count)
                            theString = theString + " , ";
                        i++;
                    }
                    theString = theString + " )";
                }
                conn.Close();
            }
            return theString;
        }
    }
}
