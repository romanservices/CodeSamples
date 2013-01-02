using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using NHibernate.Mapping;
using PEG.Data.Sync;


// ReSharper disable CheckNamespace
namespace PEG.Data
// ReSharper restore CheckNamespace
{
    public class ExtrapolateData
    {

        public void FulltoCE(string sourceDb, string targetDb, List<string> tables, Guid userID, List<string> criteriaTable )
        {
            var sqlCommands = new List<string>();

            var _buildQuerySync = new BuildQuerySync();
            List<string> _tables = tables;
            using (var _fSourceConn = new SqlConnection(sourceDb))
            using(var _targetConn = new SqlCeConnection(targetDb))
            {

                _targetConn.Open();
                _fSourceConn.Open();

                foreach (var table in _tables)
                {

                    using (var insertCommand = new SqlCeCommand(_buildQuerySync.BuildFullSqlInsertQuery(table, sourceDb),
                                                                _targetConn))
                    using (var updateCommand =
                            new SqlCeCommand(_buildQuerySync.BuildFullSqlUpdateQuery(table, sourceDb),
                                                _targetConn))
                    {
                        var selectTable = "select * from " + table;

                        var selectID = "select surveyid from surveys where assessorid = '" + userID +
                                                   "' and surveystatus in (4,7,8)";
                       
                        foreach (var criteria in criteriaTable)
                        {
                            if (criteria == table)
                            {
                                switch (criteria)
                                {
                                    case "ResponseValues":
                                        selectTable = "select * from " + table + " where surveyid in  (" + selectID +
                                                        ")";
                                        break;
                                    case "Surveys":
                                        selectTable = "select * from " + table + " where surveyid in  (" + selectID +
                                                        ")";
                                        break;
                                    case "Documents":
                                        selectTable = "select * from " + table + " where surveyid in  (" + selectID +
                                                        ")";
                                        break;
                                    case "HasardMatrix":
                                        selectTable = "select * from " + table + " where surveyid in  (" + selectID +
                                                        ")";
                                        break;
                                    case "FnrValues":
                                        selectTable = "select * from " + table + " where surveyid in  (" + selectID +
                                                        ")";
                                        break;
                                    case "Notifications":
                                        selectTable = "select * from " + table + " where recipientID  = '" + userID +
                                                        "'";
                                        break;
                                    default:
                                        selectTable = "select * from " + table;
                                        break;
                                }

                            }
                          
                        }

                        using (var _fGetSourceData = new SqlDataAdapter(selectTable, _fSourceConn))
                        {
                            var _dt = new DataTable();
                            _fGetSourceData.Fill(_dt);

                            foreach (DataRow row in _dt.Rows)
                            {
                                var reRunCommand = insertCommand.CommandText;
                                foreach (var column in _dt.Columns)
                                {
                                    var columnName = column.ToString();
                                    var value = row[Convert.ToString(column)];
                                    insertCommand.Parameters.AddWithValue("@" + columnName, value);
                                    updateCommand.Parameters.AddWithValue("@" + columnName, value);
                                    if (Convert.ToString(value) == "")
                                        value = "null";
                                    else
                                    {
                                        value = "'" + value + "'";
                                    }
                                    reRunCommand = reRunCommand.Replace("@" + columnName, Convert.ToString(value));
                                }
                                try
                                {
                                    insertCommand.ExecuteScalar();
                                }
                                catch (Exception ex)
                                {
                                    if (
                                        ex.Message.Contains(
                                            "A duplicate value cannot be inserted into a unique index."))
                                    {
                                        if (updateCommand.CommandText.Contains("Update SurveyVersions")) break;
                                        updateCommand.ExecuteScalar();
                                    }
                                    else
                                    {

                                        sqlCommands.Add(reRunCommand);
                                    }
                                }
                                insertCommand.Parameters.Clear();
                                updateCommand.Parameters.Clear();
                            }
                        }
                        foreach (var sqlCeCommand in sqlCommands)
                        {
                            try
                            {
                                insertCommand.CommandText = sqlCeCommand;
                                insertCommand.ExecuteScalar();
                            }
                            catch (Exception)
                            {
                            }
                        }
                        sqlCommands.Clear();
                    }
                }
                _targetConn.Close();
                _fSourceConn.Close();
            }
        }


        /// <summary>
        /// Processes the source data.
        /// </summary>
        /// <param name="sourceDb">The source db.</param>
        /// <remarks>This will loop over each table specified in the list.  Builds both an insert query & update query
        /// Attempts to insert first, if fails on duplicate it will update (SurveyVersions is the exception, it's only one column GUID) 
        /// Usce this method to connect to SQL CE</remarks>
        public void CEtoCe(string sourceDb, string targetDb, List<string> tables)
        {
            var sqlCommands = new List<string>(); 
            var buildQuerySync = new BuildQuerySync();
            IList<string> _tables = tables;
            var ignoreList = new SyncUpdateIgnore();
            using (var _sourceConn = new SqlCeConnection(sourceDb))
            using(var _targetConn = new SqlCeConnection(targetDb)) 
            {

                _targetConn.Open();
                _sourceConn.Open();

                foreach (var table in _tables)
                {
                    string insertCommandText = buildQuerySync.BuildInsertQuery(table, sourceDb);
                    string updateCommandText = buildQuerySync.BuildUpdateQuery(table, sourceDb);

                    using (var insertCommand = new SqlCeCommand(insertCommandText, _targetConn))
                    using (var updateCommand = new SqlCeCommand(updateCommandText, _targetConn))
                    {

                        var selectTable = "select * from " + table;

                        var _dt = new DataTable();
                        using (var _getSourceData = new SqlCeDataAdapter(selectTable, _sourceConn))
                        {
                            _getSourceData.Fill(_dt);
                        }

                        foreach (DataRow row in _dt.Rows)
                        {
                            var reRunCommand = insertCommand.CommandText;
                            foreach (var column in _dt.Columns)
                            {
                                var columnName = column.ToString();
                                var value = row[Convert.ToString(column)];
                                insertCommand.Parameters.AddWithValue("@" + columnName, value);
                                updateCommand.Parameters.AddWithValue("@" + columnName, value);
                                if (Convert.ToString(value) == "")
                                    value = "null";
                                else
                                {
                                    value = "'" + value + "'";
                                }
                                reRunCommand = reRunCommand.Replace("@" + columnName, Convert.ToString(value));
                            }
                            try
                            {
                                insertCommand.ExecuteScalar();
                            }
                            catch (Exception ex)
                            {
                                //Use SyncUpdateIgnore list here to only allow inserting new rows into static tables
                                if (!ignoreList.IgnoreList.Contains(table))
                                {
                                    if (ex.Message.Contains("A duplicate value cannot be inserted into a unique index."))
                                    {
                                        if (updateCommand.CommandText.Contains("Update SurveyVersions")) break;
                                        updateCommand.ExecuteScalar();
                                    }
                                    else
                                    {
                                        sqlCommands.Add(reRunCommand);
                                    }
                                }
                            }
                            insertCommand.Parameters.Clear();
                            updateCommand.Parameters.Clear();
                        }
                        foreach (var sqlCeCommand in sqlCommands)
                        {
                            try
                            {
                                insertCommand.CommandText = sqlCeCommand;
                                insertCommand.ExecuteScalar();
                            }
                            catch (Exception)
                            {
                            }
                        }
                        sqlCommands.Clear();
                    }
                }
                _targetConn.Close();
                _sourceConn.Close();
            }
        }


        /// <summary>
        /// Processes the source data full.
        /// </summary>
        /// <param name="sourceDb">The source db.</param>
        /// <remarks>Use this method for connecting to Full SQL</remarks>
        public void CEtoFull(string sourceDb, string targetDb, List<string> tables)
        {
            var approvedSurveysID = new DataTable();
            var filteredSurveys = string.Empty;
            var filteredOffices = string.Empty;
            var sqlCommands = new List<string>(); 
            var ignoreList = new SyncUpdateIgnore();
            var _buildQuerySync = new BuildQuerySync();
            IList<string> _tables = tables;
            using (var _sourceConn = new SqlCeConnection(sourceDb))
            using (var _fTargetConn = new SqlConnection(targetDb))
            {

                _fTargetConn.Open();
                var selectSubmitedSurveys = "select surveyID, officeID from surveys where surveystatus in (5,6)";
                using (var getSurveysID = new SqlCeDataAdapter(selectSubmitedSurveys, _sourceConn))
                {
                    getSurveysID.Fill(approvedSurveysID);
                }
                if(approvedSurveysID.Rows.Count == 0) return;
                foreach (DataRow row in approvedSurveysID.Rows)
                {
                    filteredSurveys = filteredSurveys + "'"+row[0] + "'" + ",";
                    filteredOffices = filteredOffices + "'" + row[1] + "'" + ",";
                }
                filteredSurveys = filteredSurveys.TrimEnd(',');
                filteredOffices = filteredOffices.TrimEnd(',');
                foreach (var table in _tables)
                {
                    if (table == "Surveys")
                    {
                        var s = "";
                    }

                    using (var _fInsertCommand = new SqlCommand(_buildQuerySync.BuildInsertQuery(table, sourceDb),
                                                                _fTargetConn))
                    using (var _fUpdateCommand = new SqlCommand(_buildQuerySync.BuildUpdateQuery(table, sourceDb),
                                                                _fTargetConn))
                    {
                        var selectTable = string.Empty; 
                        switch (table)
                        {
                            case "MitigationMeasures":
                                selectTable = "select * from " + table + " where officeid in ("+filteredOffices+")";
                                break;
                            case "SystemLogs":
                                selectTable = "select * from " + table;
                                    break;
                            default:
                                selectTable = "select * from " + table + " where surveyid in ("+filteredSurveys+")";
                                break;
              
                        }
                        
                        var _dt = new DataTable();

                        using (var _getSourceData = new SqlCeDataAdapter(selectTable, _sourceConn))
                        {
                            _getSourceData.Fill(_dt);
                        }

                        foreach (DataRow row in _dt.Rows)
                        {
                            var reRunCommand = _fInsertCommand.CommandText;
                            foreach (var column in _dt.Columns)
                            {
                                var columnName = column.ToString();
                                var value = row[Convert.ToString(column)];
                                _fInsertCommand.Parameters.AddWithValue("@" + columnName, value);
                                _fUpdateCommand.Parameters.AddWithValue("@" + columnName, value);
                                if (Convert.ToString(value) == "")
                                    value = "null";
                                else
                                {
                                    value = "'" + value + "'";
                                }
                                reRunCommand = reRunCommand.Replace("@" + columnName, Convert.ToString(value));
                            }
                            try
                            {
                                _fInsertCommand.ExecuteScalar();
                            }
                            catch (Exception ex)
                            {
                                //Use SyncUpdateIgnore list here to only allow inserting new rows into static tables
                                if (!ignoreList.IgnoreList.Contains(table))
                                {
                                    if (ex.Message.Contains(
                                        "A duplicate value cannot be inserted into a unique index.")
                                        || ex.Message.Contains("Violation of PRIMARY KEY constraint"))
                                    {
                                        if (_fUpdateCommand.CommandText.Contains("Update SurveyVersions")) break;
                                        _fUpdateCommand.ExecuteScalar();
                                    }
                                    else
                                    {

                                        sqlCommands.Add(reRunCommand);
                                    }
                                }
                            }
                            _fInsertCommand.Parameters.Clear();
                            _fUpdateCommand.Parameters.Clear();
                        }
                    }
                    
                    sqlCommands.Clear();
                }

                _fTargetConn.Close();
                _sourceConn.Close();
            }
        }
        
    }
}
