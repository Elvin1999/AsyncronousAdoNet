using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeginEnd
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    string commandText = "WAITFOR DELAY '00:00:03';" +
        //        "SELECT * FROM Authors";

        //    RunCommandAsync(commandText, ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString);
        //}

        //private static void RunCommandAsync(string commandText, string connectionString)
        //{

        //    try
        //    {
        //        SqlConnection connection = new SqlConnection(connectionString);
        //        SqlCommand command = new SqlCommand(commandText, connection);
        //        connection.Open();

        //        IAsyncResult result = command.BeginExecuteReader(CommandBehavior.CloseConnection);

        //        int count = 0;
        //        while (!result.IsCompleted)
        //        {
        //            Console.WriteLine($"Waiting {++count}");
        //            Thread.Sleep(100);
        //        }

        //        using (SqlDataReader reader = command.EndExecuteReader(result))
        //        {
        //            DisplayResults(reader);
        //        }

        //    }
        //    catch (Exception)
        //    {
        //    }

        //}

        //private static void DisplayResults(SqlDataReader reader)
        //{
        //    while (reader.Read())
        //    {
        //        for (int i = 0; i < reader.FieldCount; i++)
        //        {
        //            Console.Write("{0}\t",reader.GetValue(i));
        //        }
        //        Console.WriteLine();
        //    }
        //}

        static void Main(string[] args)
        {
            string commandText = "WAITFOR DELAY '00:00:03';" +
                "SELECT * FROM Authors";

            RunCommandAsync(commandText, ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString);
        }

        public delegate void AsyncCallback(IAsyncResult result);

        private static void RunCommandAsync(string commandText, string connectionString)
        {

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(commandText, connection);
                connection.Open();
                //with call back
                ////IAsyncResult result = command.BeginExecuteReader(new System.AsyncCallback(GetDataCallback),command);


                //with handle
                IAsyncResult result = command.BeginExecuteReader();

                WaitHandle handle = result.AsyncWaitHandle;
                if (handle.WaitOne(5000))
                {
                    GetDataCallback(command, result);
                }
                //int count = 0;
                //while (!result.IsCompleted)
                //{
                //    Console.WriteLine($"Waiting {++count}");
                //    Thread.Sleep(100);
                //}

            }
            catch (Exception)
            {
            }

        }
        private static void GetDataCallback(SqlCommand command,IAsyncResult result)
        {

            

            using (SqlDataReader reader = command.EndExecuteReader(result))
            {
                DisplayResults(reader);
            }
        }
        private static void GetDataCallback(IAsyncResult result)
        {

            SqlCommand command = (SqlCommand)result.AsyncState;

            using (SqlDataReader reader = command.EndExecuteReader(result))
            {
                DisplayResults(reader);
            }
        }

        private static void DisplayResults(SqlDataReader reader)
        {
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write("{0}\t", reader.GetValue(i));
                }
                Console.WriteLine();
            }
        }



    }
}
