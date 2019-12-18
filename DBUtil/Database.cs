using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace DBUtil
{
    public class Database : IDatabase
    {
        private readonly SqlConnection _sqlConnection;
        private const string LocalDatabase = @"Server=.\SQLUAI;Database=master;Integrated Security = SSPI;";

        private Database(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
        }
        
        private static Database _instance;

        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Database Instance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            _instance = new Database(LocalDatabase);
            return _instance;
        }


        public SqlTransaction BeginTransaction()
        {
            ConnectionMustBeOpen();
            return _sqlConnection.BeginTransaction();
        }

        
        public object ExecuteScalar(string sqlStatement, SqlParameter[] parameters)
        {
            ConnectionMustBeOpen();
            var sqlTransaction = _sqlConnection.BeginTransaction();
            var result = ExecuteScalar(sqlStatement, parameters, sqlTransaction);
            sqlTransaction.Commit();
            return result;
        }
        
        public object ExecuteScalar(string sqlStatement, SqlParameter[] parameters, SqlTransaction transaction)
        {
            ConnectionMustBeOpen();
            var command = new SqlCommand(sqlStatement, _sqlConnection, transaction);
            command.Parameters.AddRange(parameters);
            return command.ExecuteScalar();
        }

        public void ExecuteNonQuery(string sqlStatement, SqlParameter[] parameters)
        {
            ConnectionMustBeOpen();
            var sqlTransaction = _sqlConnection.BeginTransaction();
            ExecuteNonQuery(sqlStatement, parameters, sqlTransaction);
            sqlTransaction.Commit();
        }

        public void ExecuteNonQuery(string sqlStatement, SqlParameter[] parameters, SqlTransaction transaction)
        {
            ConnectionMustBeOpen();
            var command = new SqlCommand(sqlStatement, _sqlConnection, transaction);
            command.Parameters.AddRange(parameters);
            command.ExecuteNonQuery();
        }
        
        public DataTable ExecuteQuery(string sqlStatement, SqlParameter[] parameters)
        {
            ConnectionMustBeOpen();
            var command = new SqlCommand(sqlStatement, _sqlConnection);
            command.Parameters.AddRange(parameters);
            var dataAdapter = new SqlDataAdapter();
            var dataTable = new DataTable();
            dataAdapter.SelectCommand = command;
            dataAdapter.Fill(dataTable);
            return dataTable;
        }
        
        private void ConnectionMustBeOpen()
        {
            if (_sqlConnection.State == ConnectionState.Closed)
            {
                _sqlConnection.Open();
            }
        }

    }
}