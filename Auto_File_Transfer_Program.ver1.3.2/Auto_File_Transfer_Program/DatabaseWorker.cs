using System;
using System.IO;
using System.Text;
using System.Windows;


namespace Auto_File_Transfer_Program
{
    public class DatabaseWorker
    {
        private LocalDB aftDB = null;

        public DatabaseWorker()
        {
            aftDB = new LocalDB("Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = " +
                                ModifyCurrentDBPath() + "AFT_DB.mdf;" +
                                "Integrated Security = True; Connect Timeout = 30");
            
        }

        private string ModifyCurrentDBPath()
        {
            string[] splitPath = AppDomain.CurrentDomain.BaseDirectory.Split('\\');

            StringBuilder modifyPath = new StringBuilder();

            for (int i = 0; i < splitPath.Length - 4; i++)
            {
                modifyPath.Append(splitPath[i]);
                modifyPath.Append('\\');                
            }
            modifyPath.Append("Database");
            modifyPath.Append('\\');

            return modifyPath.ToString();
        }

        public void DropTable(string tableName)
        {
            aftDB.Open();

            string queryStr = string.Format("DROP TABLE {0}", tableName);

            aftDB.ExecuteNonQuery(queryStr);

            Console.WriteLine(tableName + " dropped");

            aftDB.Close();
        }

        public void CreateTable(string tableName, string dataStr)
        {
            aftDB.Open();

            string queryStr = string.Format("CREATE TABLE {0} ({1})", tableName, dataStr);

            aftDB.ExecuteNonQuery(queryStr);

            Console.WriteLine(tableName + " created");

            aftDB.Close();
        }

        public void DeleteTable(string tableName)
        {            
            aftDB.Open();

            string queryStr = string.Format("DELETE FROM {0}", tableName);
            aftDB.ExecuteNonQuery(queryStr);

            Console.WriteLine(tableName + " deleted");

            aftDB.Close();
        }

        public void DeleteTable_WithCondition(string tableName, int year, int month, int day)
        {
            aftDB.Open();

            string queryStr = string.Format("DELETE FROM {0} WHERE xYear = {1} AND xMonth = {2} AND xDay = {3}", tableName, year, month, day);
            aftDB.ExecuteNonQuery(queryStr);

            Console.WriteLine(tableName + " deleted");

            aftDB.Close();
        }

        public void DeleteTable_WithCondition_WithoutDay(string tableName, int year, int month)
        {
            aftDB.Open();

            string queryStr = string.Format("DELETE FROM {0} WHERE xYear = {1} AND xMonth = {2}", tableName, year, month);
            aftDB.ExecuteNonQuery(queryStr);

            Console.WriteLine(tableName + " deleted");

            aftDB.Close();
        }

        public void DeleteTable_WithCondition_Without_MonthDay(string tableName, int year)
        {
            aftDB.Open();

            string queryStr = string.Format("DELETE FROM {0} WHERE xYear = {1}", tableName, year);
            aftDB.ExecuteNonQuery(queryStr);

            Console.WriteLine(tableName + " deleted");

            aftDB.Close();
        }

        public void InsertTable(string tableName, string dataStr)
        {
            aftDB.Open();

            string queryStr = string.Format("INSERT INTO {0} VALUES({1})", tableName, dataStr);

            aftDB.ExecuteNonQuery(queryStr);

            Console.WriteLine(tableName + " inserted");

            aftDB.Close();
        }

        public void UpdateTable(string tableName, string dataStr, string condition)
        {
            aftDB.Open();

            string queryStr = string.Format("UPDATE {0} SET {1} WHERE {2}", tableName, dataStr, condition);

            aftDB.ExecuteNonQuery(queryStr);

            Console.WriteLine(tableName + " updated");

            aftDB.Close();
        }

        public LocalDB LoadTable_ETC()
        {
            aftDB.Open();

            string queryStr = string.Format(@"SELECT * FROM ETC_Table");

            aftDB.Query(queryStr);

            return aftDB;
        }

        public LocalDB LoadTable_Now()
        {
            aftDB.Open();

            string queryStr = string.Format(@"SELECT W.xName, W.xKeyword, W.xSendPath, N.xWorkFolderPath, 
                                            N.xTempPath, N.xNowPath, N.xTodayPath, N.xTime 
                                            FROM Worker_Table W, Now_Table N");
            aftDB.Query(queryStr);

            return aftDB;
        }

        public LocalDB LoadTable_Normal(int year, int month, int day)
        {
            aftDB.Open();

            string queryStr = string.Format(@"SELECT xName, xPath FROM Normal_Table
                                            WHERE xYear = {0} AND xMonth = {1} AND xDay = {2}",
                                            year, month, day);

            aftDB.Query(queryStr);

            return aftDB;
        }

        public void Test_SQL()
        {
            aftDB.Open();

            string queryStr = string.Format(@"SELECT xHour, xMinute,xName, xPath FROM Normal_Table
                                            WHERE xYear = {0} AND xMonth = {1} AND xDay = {2} ORDER BY xHour ASC",
                                            2019, 8, 15);

            string error_msg = aftDB.Query(queryStr);

            if (error_msg != null)
            {
                MessageBox.Show(queryStr + "\n\n" + error_msg, "SQL Error");
            }

            Console.WriteLine("\n" + queryStr);

            if (aftDB.HasRows)
            {

                Console.WriteLine("==========================================================");
                for (int i = 0; i < aftDB.FieldCount; i++)
                {
                    Console.Write(aftDB.GetName(i) + "\t");
                }
                Console.WriteLine();
                Console.WriteLine("==========================================================");

                while (aftDB.Read())
                {
                    for (int i = 0; i < aftDB.FieldCount; i++)
                    {
                        Console.Write(aftDB.GetData(i) + "\t");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("==========================================================\n");
            }

            aftDB.Close();
        }

    }
}
