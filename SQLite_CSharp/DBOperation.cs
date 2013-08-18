﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data.SQLite;

namespace SQLite_CSharp
{
    class DBOperation
    {
        public static string connectionString = "";

        public static ArrayList SQLiteRequest_Read(string cmdString)
        {
            ArrayList result = new ArrayList();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(cmdString, connection))
                {
                    command.CommandTimeout = 10;
                    connection.Open();
                    using (SQLiteDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Person p = new Person();
                            p.Id = dataReader.GetInt32(0);
                            p.Name = dataReader.GetValue(1).ToString();
                            p.Age = string.IsNullOrEmpty(dataReader.GetValue(2).ToString()) ? 0 : dataReader.GetInt32(2);

                            result.Add(p);
                        }
                    }
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
