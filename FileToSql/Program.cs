using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileToSql
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines(@"D:\DATA\mogujie.txt");
            foreach (var item in lines)
            {
                using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=root;database=cloudsoft_global"))
                {
                    DataSet ds = new DataSet();
                    var first = item.Split(',')[0];
                    var sql = "select * from mogujie where name='" + first+"'";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                    try
                    {
                        adapter.Fill(ds);
                        
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    
                }
            }

        }
    }
}
