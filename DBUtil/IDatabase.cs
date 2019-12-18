using System.Data;
using System.Data;
using System.Data.SqlClient;

namespace DBUtil
{
    public interface IDatabase
    {
        SqlTransaction BeginTransaction();
        object ExecuteScalar(string sqlStatement, SqlParameter[] parameters);
        object ExecuteScalar(string sqlStatement, SqlParameter[] parameters, SqlTransaction transaction);
        void ExecuteNonQuery(string sqlStatement, SqlParameter[] parameters);
        void ExecuteNonQuery(string sqlStatement, SqlParameter[] parameters, SqlTransaction transaction);
        DataTable ExecuteQuery(string sqlStatement, SqlParameter[] parameters);
    }
}