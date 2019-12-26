using System;
using System.Data.SqlClient;

namespace Auto_File_Transfer_Program
{
    public class LocalDB
    {
        SqlCommand comm = null;
        SqlConnection conn = null;
        SqlDataReader reader = null;

        string connectionStr = null;

        public LocalDB(string connStr)
        {
            connectionStr = connStr;
        }

        public void Open()
        {
            if (conn != null) Close();

            comm = new SqlCommand();
            conn = new SqlConnection();

            conn.ConnectionString = connectionStr;
            conn.Open();
            comm.Connection = conn;
        }

        public void Close()
        {
            if (conn != null) conn.Close();
            if (reader != null) reader.Close();
            conn = null;
            comm = null;
            reader = null;
        }

        public string Query(string sql)
        {
            string trimmedSQL = sql.Trim();
            string[] words = trimmedSQL.Split(' ');

            try
            {
                if (words[0].ToUpper().Equals("SELECT")) ExecuteReader(trimmedSQL);
                else ExecuteNonQuery(trimmedSQL);
            }
            catch (InvalidOperationException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return null;
        }

        public void ExecuteNonQuery(string sql)
        {
            if (reader != null) reader.Close();
            comm.CommandText = sql;
            comm.ExecuteNonQuery();
        }

        public void ExecuteReader(string sql)
        {
            if (reader != null) reader.Close();
            comm.CommandText = sql;
            reader = comm.ExecuteReader();
        }

        public bool Read()
        {
            if (reader == null) return false;

            return reader.Read();
        }

        public bool HasRows
        {
            get
            {
                if (reader == null) return false;
                return reader.HasRows;
            }
        }

        public int FieldCount
        {
            get
            {
                if (reader == null) return 0;
                return reader.FieldCount;
            }
        }

        public string GetName(int index)
        {
            if (reader == null) return "";

            return reader.GetName(index);
        }

        public object GetData(string dataName)
        {
            return reader[dataName];
        }

        public object GetData(int index)
        {
            return reader[index];
        }

    }
}
