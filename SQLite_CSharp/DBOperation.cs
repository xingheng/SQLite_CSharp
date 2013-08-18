using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data.SQLite;
using System.Data;

namespace SQLite_CSharp
{
    class DBOperation
    {
        public static string connectionString = "";

        public static Object SQLiteRequest_Read(string cmdString)
        {
            DataTable result = null;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(cmdString, connection))
                {
                    command.CommandTimeout = 10;
                    connection.Open();
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    result = new DataTable();
                    adapter.Fill(result);
                }
            }
            return result;
        }

        public static Exception SQLiteRequest_Write(string cmdString, params object[] argsList)
        {
#if DEBUG
            if (argsList.Length % 2 != 0)
                System.Windows.Forms.MessageBox.Show("The length of argsList is invalid, please check it!");
            if (argsList.Length / 2 != cmdString.Count(c => c.Equals('@')))
                System.Windows.Forms.MessageBox.Show("The length of argsList is invalid, please check it!");
#endif

            Exception result = null;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    using (SQLiteCommand command = new SQLiteCommand(cmdString, connection))
                    {
                        if (argsList != null && argsList.Length > 0)
                            for (int i = 0; i < argsList.Length; i += 2)
                                command.Parameters.AddWithValue(argsList[i].ToString(), argsList[i + 1]);

                        command.CommandTimeout = 10;
                        connection.Open(); 
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex;
            }
            return result;
        }
    }
}
